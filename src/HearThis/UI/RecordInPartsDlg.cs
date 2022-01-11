// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2022, SIL International. All Rights Reserved.
// <copyright from='2016' to='2022' company='SIL International'>
//		Copyright (c) 2022, SIL International. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HearThis.Properties;
using HearThis.Publishing;
using L10NSharp;
using SIL.IO;
using SIL.Media.Naudio;
using SIL.Media.Naudio.UI;
using SIL.Progress;
using SIL.Reporting;
using SIL.Windows.Forms.PortableSettingsProvider;

namespace HearThis.UI
{
	public partial class RecordInPartsDlg : Form, IMessageFilter
	{
		private TempFile _tempFile1 = TempFile.WithExtension("wav");
		TempFile _tempFile2 = TempFile.WithExtension("wav");
		private TempFile _tempFileJoined = TempFile.WithExtension("wav");
		private Color _scriptSecondHalfColor = AppPalette.SecondPartTextColor;
		private AudioButtonsControl _audioButtonCurrent;
		private RecordingDeviceIndicator _recordingDeviceIndicator;
		private Color _defaultForegroundColorForInstructions;

		public RecordInPartsDlg()
		{
			// TempFile creates empty files, but we don't want them to exist until there is a real
			// recording to play, because it undesirably enables the play buttons.
			RobustFile.Delete(_tempFile1.Path);
			RobustFile.Delete(_tempFile2.Path);
			RobustFile.Delete(_tempFileJoined.Path);

			InitializeComponent();
			_defaultForegroundColorForInstructions = _instructionsLabel.ForeColor;
			if (Settings.Default.RecordInPartsFormSettings == null)
				Settings.Default.RecordInPartsFormSettings = FormSettings.Create(this);
			_audioButtonCurrent = _audioButtonsFirst;

			_audioButtonsFirst.HaveSomethingToRecord = _audioButtonsSecond.HaveSomethingToRecord = true;
			_audioButtonsFirst.Path = _tempFile1.Path;
			_audioButtonsSecond.Path = _tempFile2.Path;
			_audioButtonsBoth.Path = _tempFileJoined.Path;
			_audioButtonsFirst.SoundFileRecordingComplete += AudioButtonsOnSoundFileCreated;
			_audioButtonsSecond.SoundFileRecordingComplete += AudioButtonsOnSoundFileCreated;
			UpdateDisplay();
			_recordTextBox.ForeColor = AppPalette.ScriptFocusTextColor;
			BackColor = AppPalette.Background;
			_recordTextBox.BackColor = AppPalette.Background;
			_recordTextBox.SelectionChanged += RecordTextBoxOnSelectionChanged;
			_recordTextBox.ReadOnly = true;
			Application.AddMessageFilter(this);
			Closing += (sender, args) => Application.RemoveMessageFilter(this);
			_audioButtonsFirst.RecordingStarting += RecordingStarting;
			_audioButtonsSecond.RecordingStarting += RecordingStarting;
		}

		private void RecordingStarting(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Although the instructions are not actually script context, their proximity to the
			// text to be recorded could be confusing, so we'll mute them during the recording.
			_instructionsLabel.ForeColor = AppPalette.ScriptContextTextColorDuringRecording;
		}

		private static bool RecordingExists(string path)
		{
			return File.Exists(path) && new FileInfo(path).Length > 0;
		}

		private bool _handlingSelChanged = false;

		private void RecordTextBoxOnSelectionChanged(object sender, EventArgs eventArgs)
		{
			// Checking selectionStart stops it from firing when the dialog first comes up.
			// Incidentally prevents it ALL going red.
			if (_handlingSelChanged || _recordTextBox.SelectionLength > 0 || _recordTextBox.SelectionStart == 0)
				return;
			_handlingSelChanged = true;
			int originalStart = _recordTextBox.SelectionStart;
			_recordTextBox.SelectionStart = 0;
			_recordTextBox.SelectionLength = originalStart;
			_recordTextBox.SelectionColor = AppPalette.ScriptFocusTextColor;

			_recordTextBox.SelectionStart = originalStart;
			_recordTextBox.SelectionLength = _recordTextBox.TextLength - originalStart;
			_recordTextBox.SelectionColor = _scriptSecondHalfColor;
			_recordTextBox.SelectionLength = 0;
			_handlingSelChanged = false;
			_labelBothOne.ForeColor = _labelOne.ForeColor = AppPalette.ScriptFocusTextColor;
			_labelBothTwo.ForeColor = _labelTwo.ForeColor = _scriptSecondHalfColor;
		}

		private void UpdateDisplay()
		{
			_audioButtonsFirst.HaveSomethingToRecord = true;
			_audioButtonsSecond.HaveSomethingToRecord = RecordingExists(_tempFile1.Path);
				// trick to disable record until 1st done
			_audioButtonsBoth.HaveSomethingToRecord = RecordingExists(_tempFile2.Path); // trick to disable play until 2nd done
			// Next buttons are hidden, so this is a way to have nothing highlighted.
			_audioButtonsFirst.ButtonHighlightMode =
				_audioButtonsSecond.ButtonHighlightMode =
					_audioButtonsBoth.ButtonHighlightMode = AudioButtonsControl.ButtonHighlightModes.Next;
			if (RecordingExists(_audioButtonCurrent.Path))
				_audioButtonCurrent.ButtonHighlightMode = AudioButtonsControl.ButtonHighlightModes.Play;
			else
				_audioButtonCurrent.ButtonHighlightMode = AudioButtonsControl.ButtonHighlightModes.Record;
			_audioButtonsFirst.UpdateDisplay();
			_audioButtonsSecond.UpdateDisplay();
			_audioButtonsBoth.UpdateDisplay();
			//the default disabled text color is not different enough from enabled, when the background color of the button is not
			//white. So instead it's always enabled but we control the text color here.
			//_useRecordingsButton.Enabled = RecordingExists(_tempFile2.Path);
			_useRecordingsButton.ForeColor = RecordingExists(_tempFile2.Path)
				? SystemColors.ControlText
				: SystemColors.ControlDark;
		}

		void AdvanceCurrent()
		{
			if (_audioButtonCurrent == _audioButtonsFirst && RecordingExists(_audioButtonsFirst.Path))
				_audioButtonCurrent = _audioButtonsSecond;
			else if (_audioButtonCurrent == _audioButtonsSecond && RecordingExists(_audioButtonsSecond.Path))
				_audioButtonCurrent = _audioButtonsBoth;
			UpdateDisplay();
		}

		void GoBack()
		{
			if (_audioButtonCurrent == _audioButtonsSecond)
				_audioButtonCurrent = _audioButtonsFirst;
			else if (_audioButtonCurrent == _audioButtonsBoth)
				_audioButtonCurrent = _audioButtonsSecond;
			UpdateDisplay();
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
					MessageBox.Show("To play the clip, press the TAB key.");
					break;

				case Keys.Tab:
					if (RecordingExists(_audioButtonCurrent.Path))
						_audioButtonCurrent.OnPlay(this, null);
					else if (RecordingExists(_audioButtonsFirst.Path))
					{
						_audioButtonsFirst.OnPlay(this, null); // Play first while second current if second not recorded.
						_audioButtonCurrent = _audioButtonsFirst;
					}
					UpdateDisplay();
					break;

				case Keys.Right:
				case Keys.PageDown:
				case Keys.Down:
					AdvanceCurrent();
					break;

				case Keys.Left:
				case Keys.PageUp:
				case Keys.Up:
					GoBack();
					break;

				case Keys.D1:
					_audioButtonCurrent = _audioButtonsFirst;
					UpdateDisplay();
					break;

				case Keys.D2:
					if (!RecordingExists(_audioButtonsFirst.Path))
						break;
					_audioButtonCurrent = _audioButtonsSecond;
					UpdateDisplay();
					break;

				case Keys.D3:
					if (!RecordingExists(_audioButtonsSecond.Path))
						break;
					_audioButtonCurrent = _audioButtonsBoth;
					UpdateDisplay();
					break;

				case Keys.Space:
					var recordButton = _audioButtonCurrent;
					// If the user is trying to record but the control with no visible record is active,
					// presume he is wanting another go at recording the second segment.
					if (_audioButtonCurrent == _audioButtonsBoth)
						recordButton = _audioButtonsSecond;
					if (m.Msg == WM_KEYDOWN)
						recordButton.SpaceGoingDown();
					if (m.Msg == WM_KEYUP)
						recordButton.SpaceGoingUp();
					break;

				// Seems this should be unnecessary, since this is the OK button,
				// but if the rich text box has focus, without this the program thinks
				// we are trying to edit.
				case Keys.Enter:
					_useRecordingsButton_Click(null, null);
					break;
				case Keys.Escape:
					DialogResult = DialogResult.Cancel;
					Close();
					break;

				default:
					return false;
			}

			return true;
		}

		private void AudioButtonsOnSoundFileCreated(object sender, ErrorEventArgs eventArgs)
		{
			if (eventArgs?.GetException() == null)
			{
				if (RecordingExists(_tempFile2.Path))
				{
					var inputFiles = new[] {_tempFile1.Path, _tempFile2.Path};
					ClipRepository.MergeAudioFiles(inputFiles, _tempFileJoined.Path, new NullProgress());
					// Don't advance current, default play is to play just this bit next.
					//_audioButtonCurrent = _audioButtonsBoth;
				}
				else if (_audioButtonCurrent == _audioButtonsSecond)
					throw new ApplicationException("AudioButtonsOnSoundFileCreated after recording clip 2, but the recording does not exist or is of length 0!");
			}

			if (!_audioButtonsFirst.Recording && !_audioButtonsSecond.Recording)
				_instructionsLabel.ForeColor = _defaultForegroundColorForInstructions;

			UpdateDisplay();
		}

		public Font VernacularFont
		{
			get { return _recordTextBox.Font; }
			set { _recordTextBox.Font = value; }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components?.Dispose();
				_tempFile1.Dispose();
				_tempFile2.Dispose();
				_tempFileJoined.Dispose();
				_audioButtonsFirst.SoundFileRecordingComplete -= AudioButtonsOnSoundFileCreated;
				_audioButtonsSecond.SoundFileRecordingComplete -= AudioButtonsOnSoundFileCreated;
				_recordTextBox.SelectionChanged -= RecordTextBoxOnSelectionChanged;
				_audioButtonsFirst.RecordingStarting -= RecordingStarting;
				_audioButtonsSecond.RecordingStarting -= RecordingStarting;
			}
			base.Dispose(disposing);
		}

		public string TextToRecord
		{
			get { return _recordTextBox.Text; }
			set
			{
				if (Settings.Default.BreakLinesAtClauses)
				{
					var splitter = ScriptControl.ScriptBlockPainter.ClauseSplitter;
					var clauses = splitter.BreakIntoChunks(value);
					_recordTextBox.Text = string.Join("\n", clauses.Select(c => c.Text).ToArray());
					// Note: doing this means that the value we get may not match the value that was set.
					// Currently I don't think we actually use the getter, but if we ever do we might
					// want to fix this.
				}
				else
				{
					_recordTextBox.Text = value;
				}
			}
		}

		public RecordingDeviceIndicator RecordingDeviceIndicator
		{
			// Although this is an "indicator", it also has the function of changing the recording device
			// on the recorder if a new one gets plugged in. Since it is designed to work with a single recorder,
			// we'll have it change the first one, and then we'll catch that change and apply it to the second one.
			set
			{
				_recordingDeviceIndicator = value;
				if (_recordingDeviceIndicator != null)
				{
					_recordingDeviceIndicator.Recorder = _audioButtonsFirst.Recorder;
				}
			}
		}

		public RecordingDevice RecordingDevice
		{
			get { return _audioButtonsFirst.RecordingDevice; }
			set
			{
				_audioButtonsFirst.RecordingDevice = _audioButtonsSecond.RecordingDevice = value;
				_audioButtonsFirst.Recorder.SelectedDeviceChanged += audioButtonsFirstRecorder_SelectedDeviceChanged;
			}
		}

		private void audioButtonsFirstRecorder_SelectedDeviceChanged(object sender, EventArgs e)
		{
			// Keep the second one in sync with the first one.
			_audioButtonsSecond.Recorder.SelectedDevice = RecordingDevice;
		}

		public Dictionary<string, string> ContextForAnalytics
		{
			get { return _audioButtonsFirst.ContextForAnalytics; }
			set
			{
				_audioButtonsFirst.ContextForAnalytics =
					_audioButtonsSecond.ContextForAnalytics = _audioButtonsBoth.ContextForAnalytics = value;
			}
		}

		public void WriteCombinedAudio(string destPath)
		{
			if (!File.Exists(_tempFileJoined.Path))
				return;
			try
			{
				RobustFile.Copy(_tempFileJoined.Path, destPath, true);
			}
			catch (Exception err)
			{
				ErrorReport.NotifyUserOfProblem(err, String.Format(LocalizationManager.GetString("RecordInParts.ErrorMovingExistingRecording",
					"HearThis was unable to copy the combined recording to the correct destination:\r\n{0}\r\n" +
					"Please report this error. Restarting HearThis might solve this problem."), destPath));
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			Logger.WriteEvent("Recording in parts");
			Settings.Default.RecordInPartsFormSettings.InitializeForm(this);
			base.OnLoad(e);
			UpdateDisplay();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			_recordingDeviceIndicator.MicCheckingEnabled = true;
		}

		protected override void OnDeactivate(EventArgs e)
		{
			base.OnDeactivate(e);
			_recordingDeviceIndicator.MicCheckingEnabled = false;
		}

		private void OnRecordButtonStateChanged(object sender, BtnState newState)
		{
			if (_audioButtonsFirst.Recording || _audioButtonsSecond.Recording)
				return;
			_instructionsLabel.ForeColor = (newState == BtnState.MouseOver) ?
				AppPalette.ScriptContextTextColorDuringRecording :
				_defaultForegroundColorForInstructions;
		}

		private void _useRecordingsButton_Click(object sender, EventArgs e)
		{
			// Can't use these recordings until we have both
			if (RecordingExists(_audioButtonsSecond.Path))
			{
				DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				//conceivably, they just did it all in one go and are happy, and this will make them unhappy!
				//we're weighing that against someone intending to do 2 but getting confused and clicking this button prematurely.
				MessageBox.Show(
					"HearThis needs two recordings in order to finish this task. Click 'Cancel' if you don't want to make two recordings.");
			}
		}
	}
}
