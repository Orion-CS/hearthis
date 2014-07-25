using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DesktopAnalytics;
using HearThis.Properties;
using HearThis.Publishing;
using HearThis.Script;
using L10NSharp;
using Palaso.Code;
using Palaso.Media.Naudio;

namespace HearThis.UI
{
	public partial class RecordingToolControl : UserControl, IMessageFilter
	{
		private Project _project;
		private int _previousLine;
		private bool _alreadyShutdown;
		private string _lineCountLabelFormat;
		public event EventHandler ChooseProject;
		private bool _changingChapter = false;

		private readonly string _endOfBook = LocalizationManager.GetString("RecordingControl.EndOf", "End of {0}",
			"{0} is typically a book name");
		private readonly string _chapterFinished = LocalizationManager.GetString("RecordingControl.Finished", "{0} Finished",
			"{0} is a chapter number");
		private readonly string _gotoLink = LocalizationManager.GetString("RecordingControl.GoTo", "Go To {0}",
			"{0} is a chapter number");

		public RecordingToolControl()
		{
			InitializeComponent();
			_lineCountLabelFormat = _lineCountLabel.Text;
			BackColor = AppPallette.Background;

			//_upButton.Initialize(Resources.up, Resources.upDisabled);
			//_nextButton.Initialize(Resources.down, Resources.downDisabled);

			if (DesignMode)
				return;

			_peakMeter.Start(33); //the number here is how often it updates
			_peakMeter.ColorMedium = AppPallette.Blue;
			_peakMeter.ColorNormal = AppPallette.EmptyBoxColor;
			_peakMeter.ColorHigh = AppPallette.Red;
			_peakMeter.SetRange(5, 80, 100);
			_audioButtonsControl.Recorder.PeakLevelChanged += ((s, e) => _peakMeter.PeakLevel = e.Level);
			_audioButtonsControl.RecordingDevice = RecordingDevice.Devices.FirstOrDefault();
			if (_audioButtonsControl.RecordingDevice == null)
				_audioButtonsControl.ReportNoMicrophone();
			recordingDeviceButton1.Recorder = _audioButtonsControl.Recorder;
			MouseWheel += OnRecordingToolControl_MouseWheel;

			_toolStrip.Renderer = new NoBorderToolStripRenderer();
			toolStripButton4.ForeColor = AppPallette.NavigationTextColor;

			_endOfUnitMessage.ForeColor = AppPallette.Blue;
			_nextChapterLink.ActiveLinkColor = AppPallette.HilightColor;
			_nextChapterLink.DisabledLinkColor = AppPallette.NavigationTextColor;
			_nextChapterLink.LinkColor = AppPallette.HilightColor;

			_audioButtonsControl.SoundFileCreated += OnSoundFileCreated;

			SetupUILanguageMenu();
			UpdateBreakClausesImage();

			_lineCountLabel.ForeColor = AppPallette.NavigationTextColor;

			_scriptControl.ShowSkippedBlocks = _showSkippedBlocksButton.Checked = Settings.Default.ShowSkippedBlocks;
		}

		private void OnSoundFileCreated(object sender, EventArgs eventArgs)
		{
			if (CurrentScriptLine.Skipped)
			{
				var skipPath = Path.ChangeExtension(_project.GetPathToRecordingForSelectedLine(), "skip");
				if (File.Exists(skipPath))
				{
					try
					{
						File.Delete(skipPath);
					}
					catch (Exception e)
					{
						// Bummer. But we can probably ignore this.
						Analytics.ReportException(e);
					}
				}
			}
			_project.SelectedChapterInfo.OnScriptBlockRecorded(CurrentScriptLine);
			OnSoundFileCreatedOrDeleted();
		}

		private void OnSoundFileCreatedOrDeleted()
		{
			_scriptSlider.Invalidate();
			// deletion is done in LineRecordingRepository and affects audioButtons
			UpdateDisplay();
		}

		/// <summary>
		/// This invokes the message filter that allows the control to interpret various keystrokes as button presses.
		/// It is tempting to try to manage this from within this control, e.g., in the constructor and Dispose method.
		/// However, this fails to disable the message filter when dialogs (or the localization tool) are launched.
		/// The interception of the space key, especially, is disconcerting while some dialogs are active.
		/// So, instead, we arrange to call these methods from the OnActivated and OnDeactivate methods of the parent window.
		/// </summary>
		public void StartFilteringMessages()
		{
			Application.AddMessageFilter(this);
		}

		public void StopFilteringMessages()
		{
			Application.RemoveMessageFilter(this);
		}

		private void OnRecordingToolControl_MouseWheel(object sender, MouseEventArgs e)
		{
			//the minus here is because down (negative) on the wheel equates to addition on the horizontal slider
			_scriptSlider.Value += e.Delta / -120;
		}

		public void SetProject(Project project)
		{
			_project = project;
			_bookFlow.Controls.Clear();
			_scriptSlider.ValueChanged -= OnLineSlider_ValueChanged; // update later when we have a correct value
			foreach (BookInfo bookInfo in project.Books)
			{
				var x = new BookButton(bookInfo) {Tag = bookInfo};
				_instantToolTip.SetToolTip(x, bookInfo.LocalizedName);
				x.Click += OnBookButtonClick;
				_bookFlow.Controls.Add(x);
				if (bookInfo.BookNumber == 38)
					_bookFlow.SetFlowBreak(x, true);
				BookInfo bookInfoForInsideClosure = bookInfo;
				project.LoadBookAsync(bookInfo.BookNumber, delegate
				{
					if (x.IsHandleCreated && !x.IsDisposed)
						x.Invalidate();
					if (IsHandleCreated && !IsDisposed && project.SelectedBook == bookInfoForInsideClosure)
					{
						//_project.SelectedChapterInfo = bookInfoForInsideClosure.GetFirstChapter();
						//UpdateSelectedChapter();
						_project.GotoInitialChapter();
						UpdateSelectedBook();
					}
				});
			}
			UpdateSelectedBook();
			_scriptSlider.ValueChanged += OnLineSlider_ValueChanged;
			_scriptSlider.GetSegmentBrushesMethod = GetSegmentBrushes;
		}

		private Brush[] GetSegmentBrushes()
		{
			Guard.AgainstNull(_project, "project");

			int lineCountForChapter = _project.GetLineCountForChapter(true);
			var brushes = new Brush[_project.GetLineCountForChapter(_showSkippedBlocksButton.Checked)];
			int iBrush = 0;
			for (int i = 0; i < lineCountForChapter; i++)
			{
				if (ClipRecordingRepository.GetHaveClip(_project.Name, _project.SelectedBook.Name,
					_project.SelectedChapterInfo.ChapterNumber1Based, i))
				{
					brushes[iBrush++] = AppPallette.BlueBrush;
				}
				else if (GetScriptBlock(i).Skipped)
				{
					if (_showSkippedBlocksButton.Checked)
						brushes[iBrush++] = AppPallette.SkippedSegmentBrush;
				}
				else
					brushes[iBrush++] = Brushes.Transparent;
			}
			return brushes;
		}

		private void UpdateDisplay()
		{
			_skipButton.Enabled = HaveScript;
			_audioButtonsControl.HaveSomethingToRecord = HaveScript;
			_audioButtonsControl.UpdateDisplay();
			_lineCountLabel.Visible = HaveScript;
			//_upButton.Enabled = _project.SelectedScriptLine > 0;
			//_audioButtonsControl.CanGoNext = _project.SelectedScriptBlock < (_project.GetLineCountForChapter()-1);
			_deleteRecordingButton.Visible = HaveRecording;
		}

		private bool HaveRecording
		{
			get
			{
				return ClipRecordingRepository.GetHaveClip(_project.Name, _project.SelectedBook.Name,
					_project.SelectedChapterInfo.ChapterNumber1Based, _project.SelectedScriptBlock);
			}
		}

		private bool HaveScript
		{
			// This method is much more reliable for single line sections than comparing slider max & min
			get { return CurrentScriptLine != null && CurrentScriptLine.Text.Length > 0; }
		}


		/// <summary>
		/// Filter out all keystrokes except the few that we want to handle.
		/// We handle Space, Enter, Period, PageUp, PageDown, Delete and Arrow keys.
		/// </summary>
		/// <remarks>This is invoked because we implement IMessagFilter and call Application.AddMessageFilter(this)</remarks>
		public bool PreFilterMessage(ref Message m)
		{
			const int WM_KEYDOWN = 0x100;
			const int WM_KEYUP = 0x101;

			if (m.Msg != WM_KEYDOWN && m.Msg != WM_KEYUP)
				return false;

			if (m.Msg == WM_KEYUP && (Keys) m.WParam != Keys.Space)
				return false;

			switch ((Keys) m.WParam)
			{
				case Keys.OemPeriod:
				case Keys.Decimal:
				case Keys.Enter:
					_audioButtonsControl.OnPlay(this, null);
					break;

				case Keys.Right:
				case Keys.PageDown:
				case Keys.Down:
					OnNextButton(this, null);
					break;

				case Keys.Left:
				case Keys.PageUp:
				case Keys.Up:
					GoBack();
					break;

				case Keys.Space:
					if (m.Msg == WM_KEYDOWN)
						_audioButtonsControl.SpaceGoingDown();
					if (m.Msg == WM_KEYUP)
						_audioButtonsControl.SpaceGoingUp();
					break;

				case Keys.Delete:
					OnDeleteRecording();
					break;

				case Keys.Tab:

					// Eat these.
					break;

				default:
					return false;
			}

			return true;
		}

		public void Shutdown()
		{
			if (_alreadyShutdown)
				return;
			_alreadyShutdown = true;
		}

		/// ------------------------------------------------------------------------------------
		protected override void OnHandleDestroyed(EventArgs e)
		{
			Shutdown();
			base.OnHandleDestroyed(e);
		}

		private void OnBookButtonClick(object sender, EventArgs e)
		{
			_project.SelectedBook = (BookInfo) ((BookButton) sender).Tag;
			UpdateSelectedBook();
		}

		private void UpdateSelectedBook()
		{
			_bookLabel.Text = _project.SelectedBook.LocalizedName;

			foreach (BookButton button in _bookFlow.Controls)
				button.Selected = false;

			BookButton selected = (from BookButton control in _bookFlow.Controls
				where control.Tag == _project.SelectedBook
				select control).Single();

			selected.Selected = true;

			_chapterFlow.SuspendLayout();
			_chapterFlow.Controls.Clear();

			var buttons = new List<ChapterButton>();

			//note: we're using chapter 0 to mean the material at the start of the book
			for (int i = 0; i <= _project.SelectedBook.ChapterCount; i++)
			{
				var chapterInfo = _project.SelectedBook.GetChapter(i);
				if (i == 0 && chapterInfo.IsEmpty)
					continue;

				var button = new ChapterButton(chapterInfo);
				button.Width = 15;
				button.Click += OnChapterClick;
				buttons.Add(button);
				_instantToolTip.SetToolTip(button, i == 0 ? GetIntroductionString() : string.Format(GetChapterNumberString(), i));
			}
			_chapterFlow.Controls.AddRange(buttons.ToArray());
			_chapterFlow.ResumeLayout(true);
			UpdateSelectedChapter();
		}

		private static string GetIntroductionString()
		{
			return LocalizationManager.GetString("RecordingControl.Introduction", "Introduction");
		}

		private static string GetChapterNumberString()
		{
			return LocalizationManager.GetString("RecordingControl.Chapter", "Chapter {0}");
		}

		private void OnChapterClick(object sender, EventArgs e)
		{
			_project.SelectedChapterInfo = ((ChapterButton) sender).ChapterInfo;
			UpdateSelectedChapter();
		}

		private void UpdateSelectedChapter()
		{
			foreach (ChapterButton chapterButton in _chapterFlow.Controls)
			{
				chapterButton.Selected = false;
			}
			if (_project.SelectedChapterInfo.ChapterNumber1Based > 0)
				_chapterLabel.Text = string.Format(GetChapterNumberString(), _project.SelectedChapterInfo.ChapterNumber1Based);
			else
			{
				_chapterLabel.Text = string.Format(GetIntroductionString());
			}

			ChapterButton button = (from ChapterButton control in _chapterFlow.Controls
				where control.ChapterInfo.ChapterNumber1Based == _project.SelectedChapterInfo.ChapterNumber1Based
				select control).Single();

			button.Selected = true;
			ResetSegmentCount();
			_changingChapter = true;
			if (HidingSkippedBlocks)
				_project.SelectedScriptBlock = GetScriptBlockIndexFromSliderValueByAccountingForPrecedingHiddenBlocks(0);
			if (_scriptSlider.Value == 0)
				UpdateSelectedScriptLine();
			else
				_scriptSlider.Value = 0;
			_changingChapter = false;
			UpdateScriptAndMessageControls();
		}

		private void ResetSegmentCount()
		{
			if (_project == null)
				return;
			var lineCount = _project.GetLineCountForChapter(!HidingSkippedBlocks);
			_scriptSlider.SegmentCount = lineCount;
			if (_scriptSlider.SegmentCount == 0 && lineCount == 0) // Fixes case where lineCount = 0 (Introduction)
			{
				_audioButtonsControl.Enabled = false;
				_scriptSlider.Enabled = false;
				//_maxScriptLineLabel.Text = "";
			}
			else
			{
				_audioButtonsControl.Enabled = true;
				_scriptSlider.Enabled = true;
				//_maxScriptLineLabel.Text = _scriptLineSlider.Maximum.ToString();
			}
		}

		private void OnLineSlider_ValueChanged(object sender, EventArgs e)
		{
			UpdateScriptAndMessageControls();
			if (_scriptSlider.Finished)
				_project.SelectedScriptBlock = _project.GetLineCountForChapter(true);
			else
			{
				int sliderValue = _scriptSlider.Value;
				if (HidingSkippedBlocks)
					sliderValue = GetScriptBlockIndexFromSliderValueByAccountingForPrecedingHiddenBlocks(sliderValue);
				_project.SelectedScriptBlock = sliderValue;
				UpdateSelectedScriptLine();
			}
		}

		private void UpdateSelectedScriptLine()
		{
			var currentScriptLine = CurrentScriptLine;
			string verse = currentScriptLine != null ? currentScriptLine.Verse : null;
			bool isRealVerseNumber = !string.IsNullOrEmpty(verse) && verse != "0";
			_segmentLabel.Visible = true;
			_skipButton.CheckedChanged -= OnSkipButtonCheckedChanged;
			_skipButton.Checked = currentScriptLine != null && currentScriptLine.Skipped;
			_skipButton.CheckedChanged += OnSkipButtonCheckedChanged;
			if (HaveScript)
			{
				int displayedBlockIndex = _scriptSlider.Value + 1;
				_lineCountLabel.Text = string.Format(_lineCountLabelFormat, displayedBlockIndex, _scriptSlider.SegmentCount);

				if (currentScriptLine.Heading)
					_segmentLabel.Text = LocalizationManager.GetString("RecordingControl.Heading", "Heading");
				else if (isRealVerseNumber)
					_segmentLabel.Text = String.Format(LocalizationManager.GetString("RecordingControl.Script", "Verse {0}"), verse);
				else
					_segmentLabel.Text = String.Empty;
			}
			else
			{
				if (isRealVerseNumber)
				{
					_segmentLabel.Text =
						String.Format(
							LocalizationManager.GetString("RecordingControl.VerseNotTranslated", "Verse {0} not translated yet"),
							CurrentScriptLine.Verse);
				}
				else
				{
					// Can this happen?
					_segmentLabel.Text = LocalizationManager.GetString("RecordingControl.NotTranslated", "Not translated yet");
				}
			}

			if (_scriptSlider.SegmentCount == 0)
				_project.SelectedScriptBlock = 0; // This should already be true, but just make sure;

			_scriptControl.GoToScript(GetDirection(), PreviousScriptBlock, currentScriptLine, NextScriptBlock);
			_previousLine = _project.SelectedScriptBlock;
			_audioButtonsControl.Path = _project.GetPathToRecordingForSelectedLine();

			char[] delimiters = {' ', '\r', '\n'};

			var approximateWordCount = 0;
			if (currentScriptLine != null)
				approximateWordCount = currentScriptLine.Text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;

			_audioButtonsControl.ContextForAnalytics = new Dictionary<string, string>
			{
				{"book", _project.SelectedBook.Name},
				{"chapter", _project.SelectedChapterInfo.ChapterNumber1Based.ToString()},
				{"scriptBlock", _project.SelectedScriptBlock.ToString()},
				{"wordsInLine", approximateWordCount.ToString()}
			};
			UpdateDisplay();
		}

		private bool HidingSkippedBlocks
		{
			get { return !_showSkippedBlocksButton.Checked; }
		}

		private ScriptControl.Direction GetDirection()
		{
			if (_changingChapter)
				return ScriptControl.Direction.Forwards;

			return _previousLine < _project.SelectedScriptBlock
				? ScriptControl.Direction.Forwards
				: ScriptControl.Direction.Backwards;
		}

		public ScriptLine CurrentScriptLine
		{
			get { return GetScriptBlock(_project.SelectedScriptBlock); }
		}

		public ScriptLine PreviousScriptBlock
		{
			get { return GetScriptBlock(_project.SelectedScriptBlock - 1); }
		}

		public ScriptLine NextScriptBlock
		{
			get { return GetScriptBlock(_project.SelectedScriptBlock + 1); }
		}

		public ScriptLine GetScriptBlock(int index)
		{
			if (index < 0 || index >= _project.SelectedChapterInfo.GetScriptBlockCount())
				return null;
			return _project.SelectedBook.GetBlock(_project.SelectedChapterInfo.ChapterNumber1Based, index);
		}

		private void OnNextButton(object sender, EventArgs e)
		{
			int newSliderValue = _scriptSlider.Value + 1;
			_scriptSlider.Value = newSliderValue;
			_audioButtonsControl.UpdateButtonStateOnNavigate();
		}

		private void GoBack()
		{
			int newSliderValue = _scriptSlider.Value - 1;
			_scriptSlider.Value = newSliderValue;
			_audioButtonsControl.UpdateButtonStateOnNavigate();
		}

		private void OnSaveClick(object sender, EventArgs e)
		{
			MessageBox.Show(
				LocalizationManager.GetString("RecordingControl.SaveAutomatically",
					"HearThis automatically saves your work, while you use it. This button is just here to tell you that :-)  To create sound files for playing your recordings, click on the Publish button."),
				LocalizationManager.GetString("Common.Save", "Save"));
		}

		private void _deleteRecordingButton_Click(object sender, EventArgs e)
		{
			OnDeleteRecording();
		}

		private void _deleteRecordingButton_MouseEnter(object sender, EventArgs e)
		{
			_deleteRecordingButton.Image = Resources.deleteHighlighted;
		}

		private void _deleteRecordingButton_MouseLeave(object sender, EventArgs e)
		{
			_deleteRecordingButton.Image = Resources.deleteNormal;
		}

		private void OnDeleteRecording()
		{
			if (ClipRecordingRepository.DeleteLineRecording(_project.Name, _project.SelectedBook.Name,
				_project.SelectedChapterInfo.ChapterNumber1Based, _project.SelectedScriptBlock))
			{
				OnSoundFileCreatedOrDeleted();
			}
		}

		private void OnSkipButtonCheckedChanged(object sender, EventArgs e)
		{
			if (_skipButton.Checked)
			{
				if (HaveRecording)
				{
					if (DialogResult.No ==
						MessageBox.Show(this,
							LocalizationManager.GetString("RecordingControl.ConfirmSkip",
								"There is already a recording for this line.\r\nIf you skip it, this recording will be omitted when publishing.\r\n\r\nAre you sure you want to do this?"),
							ProductName,
							MessageBoxButtons.YesNo))
						return;
					var recordingPath = _project.GetPathToRecordingForSelectedLine();
					File.Move(recordingPath, Path.ChangeExtension(recordingPath, "skip"));
				}
				CurrentScriptLine.Skipped = true;
				// This is no longer needed because the skip button is invisible if skipped blocks are being hidden.
				//if (HidingSkippedBlocks)
				//{
				//    ResetSegmentCount();
				//    if (_scriptSlider.Finished)
				//    {
				//        UpdateScriptAndMessageControls();
				//        return;
				//    }
				//}
				OnNextButton(sender, e);
			}
			else
			{
				var recordingPath = _project.GetPathToRecordingForSelectedLine();
				var skipPath = Path.ChangeExtension(recordingPath, "skip");
				if (File.Exists(skipPath))
				{
					File.Move(skipPath, recordingPath);
					OnSoundFileCreatedOrDeleted();
				}
				else
					_scriptSlider.Invalidate();
				CurrentScriptLine.Skipped = false;
				_scriptControl.Invalidate();
			}
		}

		private void OnShowSkippedBlocksButtonCheckedChanged(object sender, EventArgs e)
		{
			_scriptControl.ShowSkippedBlocks = _skipButton.Visible = !HidingSkippedBlocks;

			if (_project == null)
				return;

			int sliderValue = _scriptSlider.Value;
			bool alreadyFinished = (sliderValue == _scriptSlider.SegmentCount);
			ResetSegmentCount();

			if (alreadyFinished)
			{
				sliderValue = _scriptSlider.SegmentCount;
			}
			else if (_scriptSlider.SegmentCount == 0)
			{
				// Unusual case where all segments were skipped and are now being hidden
				sliderValue = 0;
			}
			else
			{
				if (HidingSkippedBlocks)
				{
					for (int i = 0; i < _project.SelectedScriptBlock; i++)
					{
						if (GetScriptBlock(i).Skipped)
							sliderValue--;
					}
					// We also need to subtract 1 for the selected block if it was skipped
					if (GetScriptBlock(_project.SelectedScriptBlock).Skipped)
						sliderValue--;
					if (sliderValue < 0)
					{
						// Look forward to find an unskipped block
						sliderValue = 0;
						while (sliderValue < _scriptSlider.SegmentCount && GetScriptBlock(_project.SelectedScriptBlock + sliderValue + 1).Skipped)
							sliderValue++;
					}
				}
				else
				{
					sliderValue = GetScriptBlockIndexFromSliderValueByAccountingForPrecedingHiddenBlocks(sliderValue);
				}
			}
			if (_scriptSlider.Value == sliderValue)
			{
				UpdateScriptAndMessageControls();
				if (!_scriptSlider.Finished)
					UpdateSelectedScriptLine();
			}
			else
				_scriptSlider.Value = sliderValue;

			Settings.Default.ShowSkippedBlocks = !HidingSkippedBlocks;
		}

		private void OnAboutClick(object sender, EventArgs e)
		{
			using (var dlg = new AboutDialog())
			{
				dlg.ShowDialog();
			}
		}

		private void OnPublishClick(object sender, EventArgs e)
		{
			using (var dlg = new PublishDialog(new PublishingModel(_project.Name, _project.EthnologueCode)))
			{
				dlg.ShowDialog();
			}
		}

		private void OnChangeProjectButton_Click(object sender, EventArgs e)
		{
			if (ChooseProject != null)
				ChooseProject(this, null);
		}

		private void OnSmallerClick(object sender, EventArgs e)
		{
			if (_scriptControl.ZoomFactor > 1)
				_scriptControl.ZoomFactor -= 0.2f;
		}

		private void OnLargerClick(object sender, EventArgs e)
		{
			if (_scriptControl.ZoomFactor < 2)
				_scriptControl.ZoomFactor += 0.2f;
		}

		/// <summary>
		/// Shows or hides controls as appropriate based on whether user has advanced through all blocks in this chapter:
		/// responsable for the "End of (book)" messages and "Go To Chapter x" links.
		/// </summary>
		private void UpdateScriptAndMessageControls()
		{
			if (_scriptSlider.Finished)
			{
				HideScriptLines();
				// '>' is just paranoia
				if (_project.SelectedChapterInfo.ChapterNumber1Based >= _project.SelectedBook.ChapterCount)
				{
					ShowEndOfBook();
				}
				else
				{
					ShowEndOfChapter();
				}
				_skipButton.Enabled = false;
				_audioButtonsControl.HaveSomethingToRecord = false;
				_audioButtonsControl.UpdateDisplay();
				_segmentLabel.Visible = false;
				_lineCountLabel.Visible = false;
			}
			else
				ShowScriptLines();
		}

		private void ShowEndOfChapter()
		{
			if (_project.SelectedChapterInfo.RecordingsFinished)
			{
				_endOfUnitMessage.Text = string.Format(_chapterFinished, _chapterLabel.Text);
				_endOfUnitMessage.Visible = true;
			}
			else
				_endOfUnitMessage.Visible = false;
			_nextChapterLink.Text = string.Format(_gotoLink, GetNextChapterLabel());
			_nextChapterLink.Visible = true;
			_audioButtonsControl.CanGoNext = false;
		}

		private string GetNextChapterLabel()
		{
			return string.Format(GetChapterNumberString(), _project.GetNextChapterNum());
		}

		private void ShowEndOfBook()
		{
			_endOfUnitMessage.Text = string.Format(_endOfBook, _bookLabel.Text);
			_endOfUnitMessage.Visible = true;
		}

		private void ShowScriptLines()
		{
			_endOfUnitMessage.Visible = false;
			_nextChapterLink.Visible = false;
			_scriptControl.Visible = true;
			_audioButtonsControl.CanGoNext = true;
		}

		private void HideScriptLines()
		{
			_scriptControl.Visible = false;
		}

		private void OnNextChapterLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			_project.SelectedChapterInfo = _project.GetNextChapterInfo();
			UpdateSelectedChapter();
		}

		private void SetupUILanguageMenu()
		{
			_uiLanguageMenu.DropDownItems.Clear();
			foreach (var lang in LocalizationManager.GetUILanguages(true))
			{
				var item = _uiLanguageMenu.DropDownItems.Add(lang.NativeName);
				item.Tag = lang;
				item.Click += new EventHandler((a, b) =>
				{
					LocalizationManager.SetUILanguage(((CultureInfo) item.Tag).IetfLanguageTag, true);
					Settings.Default.UserInterfaceLanguage = ((CultureInfo) item.Tag).IetfLanguageTag;
					item.Select();
					_uiLanguageMenu.Text = ((CultureInfo) item.Tag).NativeName;
				});
				if (((CultureInfo) item.Tag).IetfLanguageTag == Settings.Default.UserInterfaceLanguage)
				{
					_uiLanguageMenu.Text = ((CultureInfo) item.Tag).NativeName;
				}
			}

			_uiLanguageMenu.DropDownItems.Add(new ToolStripSeparator());
			var menu = _uiLanguageMenu.DropDownItems.Add(LocalizationManager.GetString("RecordingControl.MoreMenuItem",
				"More...", "Last item in menu of UI languages"));
			menu.Click += new EventHandler((a, b) =>
			{
				LocalizationManager.ShowLocalizationDialogBox(this);
				SetupUILanguageMenu();
			});
		}

		private void _breakLinesAtCommasButton_Click(object sender, EventArgs e)
		{
			Settings.Default.BreakLinesAtClauses = !Settings.Default.BreakLinesAtClauses;
			Settings.Default.Save();
			UpdateBreakClausesImage();
			_scriptControl.Invalidate();
		}

		private void UpdateBreakClausesImage()
		{
			_breakLinesAtCommasButton.Image =
				Settings.Default.BreakLinesAtClauses ? Resources.Icon_LineBreak_Comma_Active : Resources.Icon_LineBreak_Comma;
		}

		private void _breakLinesAtCommasButton_MouseEnter(object sender, EventArgs e)
		{
			_breakLinesAtCommasButton.BackColor = AppPallette.MouseOverButtonBackColor;
		}

		private void _breakLinesAtCommasButton_MouseLeave(object sender, EventArgs e)
		{
			_breakLinesAtCommasButton.BackColor = AppPallette.Background;
		}

		public class NoBorderToolStripRenderer : ToolStripProfessionalRenderer
		{
			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
			}
		}

		private int GetScriptBlockIndexFromSliderValueByAccountingForPrecedingHiddenBlocks(int sliderValue)
		{
			for (int i = 0; i <= sliderValue; i++)
			{
				var block = GetScriptBlock(i);
				if (block == null) // passed the end of the list. All were skipped.
					return sliderValue;
				if (block.Skipped)
					sliderValue++;
			}
			return sliderValue;
		}
	}
}
