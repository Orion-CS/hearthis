using System.Windows.Forms;
using SIL.Media.Naudio.UI;

namespace HearThis.UI
{
    partial class RecordingToolControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        
		#region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this._bookFlow = new System.Windows.Forms.FlowLayoutPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._chapterFlow = new System.Windows.Forms.FlowLayoutPanel();
			this._bookLabel = new System.Windows.Forms.Label();
			this._chapterLabel = new System.Windows.Forms.Label();
			this._segmentLabel = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this._instantToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.recordingDeviceButton1 = new SIL.Media.Naudio.UI.RecordingDeviceIndicator();
			this._peakMeter = new SIL.Media.Naudio.UI.PeakMeterCtrl();
			this._lineCountLabel = new System.Windows.Forms.Label();
			this._endOfUnitMessage = new System.Windows.Forms.Label();
			this._nextChapterLink = new System.Windows.Forms.LinkLabel();
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._labelClipOutOfSynchWithBlock = new System.Windows.Forms.Label();
			this._linkLabelClickToClearWarning = new System.Windows.Forms.LinkLabel();
			this._warningIcon = new System.Windows.Forms.PictureBox();
			this._smallerButton = new HearThis.UI.HearThisToolbarButton();
			this._largerButton = new HearThis.UI.HearThisToolbarButton();
			this._skipButton = new HearThis.UI.HearThisToolbarButton();
			this._recordInPartsButton = new HearThis.UI.HearThisToolbarButton();
			this._deleteRecordingButton = new HearThis.UI.HearThisToolbarButton();
			this._breakLinesAtCommasButton = new HearThis.UI.HearThisToolbarButton();
			this._tableLayoutPanelNavigationState = new System.Windows.Forms.TableLayoutPanel();
			this._tableLayoutPanelOutOfSynchWarning = new System.Windows.Forms.TableLayoutPanel();
			this._skipButton = new HearThis.UI.SkipButton();
			this._audioButtonsControl = new HearThis.UI.AudioButtonsControl();
			this._scriptControl = new HearThis.UI.ScriptControl();
			this._scriptSlider = new HearThis.UI.DiscontiguousProgressTrackBar();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._warningIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._smallerButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._largerButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._skipButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._recordInPartsButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._deleteRecordingButton)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._breakLinesAtCommasButton)).BeginInit();
			this._tableLayoutPanelNavigationState.SuspendLayout();
			this._tableLayoutPanelOutOfSynchWarning.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bookFlow
			// 
			this._bookFlow.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bookFlow.Location = new System.Drawing.Point(3, 31);
			this._bookFlow.Margin = new System.Windows.Forms.Padding(3, 0, 3, 13);
			this._bookFlow.Name = "_bookFlow";
			this._bookFlow.Size = new System.Drawing.Size(664, 47);
			this._bookFlow.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this._bookFlow, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._chapterFlow, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this._bookLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._chapterLabel, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 15);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 198);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// _chapterFlow
			// 
			this._chapterFlow.Dock = System.Windows.Forms.DockStyle.Fill;
			this._chapterFlow.Location = new System.Drawing.Point(6, 122);
			this._chapterFlow.Margin = new System.Windows.Forms.Padding(6, 0, 3, 3);
			this._chapterFlow.Name = "_chapterFlow";
			this._chapterFlow.Size = new System.Drawing.Size(661, 89);
			this._chapterFlow.TabIndex = 5;
			// 
			// _bookLabel
			// 
			this._bookLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._bookLabel.AutoSize = true;
			this._bookLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._bookLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._bookLabel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._bookLabel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this._bookLabel, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this._bookLabel, "RecordingControl.BookLabel");
			this._bookLabel.Location = new System.Drawing.Point(0, 0);
			this._bookLabel.Margin = new System.Windows.Forms.Padding(0);
			this._bookLabel.Name = "_bookLabel";
			this._bookLabel.Size = new System.Drawing.Size(79, 31);
			this._bookLabel.TabIndex = 3;
			this._bookLabel.Text = "label1";
			// 
			// _chapterLabel
			// 
			this._chapterLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._chapterLabel.AutoSize = true;
			this._chapterLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._chapterLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._chapterLabel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._chapterLabel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this._chapterLabel, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this._chapterLabel, "RecordingControl.ChapterLabel");
			this._chapterLabel.Location = new System.Drawing.Point(0, 91);
			this._chapterLabel.Margin = new System.Windows.Forms.Padding(0);
			this._chapterLabel.Name = "_chapterLabel";
			this._chapterLabel.Size = new System.Drawing.Size(79, 31);
			this._chapterLabel.TabIndex = 4;
			this._chapterLabel.Text = "label1";
			// 
			// _segmentLabel
			// 
			this._segmentLabel.AutoSize = true;
			this._segmentLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
			this._segmentLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._segmentLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._segmentLabel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._segmentLabel, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this._segmentLabel, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this._segmentLabel, "RecordingControl.SegmentLabel");
			this._segmentLabel.Location = new System.Drawing.Point(0, 0);
			this._segmentLabel.Margin = new System.Windows.Forms.Padding(0);
			this._segmentLabel.Name = "_segmentLabel";
			this._segmentLabel.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this._segmentLabel.Size = new System.Drawing.Size(117, 32);
			this._segmentLabel.TabIndex = 12;
			this._segmentLabel.Text = "Verse 20";
			// 
			// toolTip1
			// 
			this.toolTip1.AutoPopDelay = 6500;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			// 
			// _instantToolTip
			// 
			this._instantToolTip.AutomaticDelay = 0;
			this._instantToolTip.UseAnimation = false;
			this._instantToolTip.UseFading = false;
			// 
			// recordingDeviceButton1
			// 
			this.recordingDeviceButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.recordingDeviceButton1.BackColor = System.Drawing.Color.Transparent;
			this.l10NSharpExtender1.SetLocalizableToolTip(this.recordingDeviceButton1, null);
			this.l10NSharpExtender1.SetLocalizationComment(this.recordingDeviceButton1, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this.recordingDeviceButton1, L10NSharp.LocalizationPriority.Low);
			this.l10NSharpExtender1.SetLocalizingId(this.recordingDeviceButton1, "RecordingControl.RecordingDeviceButton");
			this.recordingDeviceButton1.Location = new System.Drawing.Point(659, 494);
			this.recordingDeviceButton1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.recordingDeviceButton1.Name = "recordingDeviceButton1";
			this.recordingDeviceButton1.Recorder = null;
			this.recordingDeviceButton1.Size = new System.Drawing.Size(22, 25);
			this.recordingDeviceButton1.TabIndex = 23;
			// 
			// _peakMeter
			// 
			this._peakMeter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._peakMeter.BandsCount = 1;
			this._peakMeter.ColorHigh = System.Drawing.Color.Red;
			this._peakMeter.ColorHighBack = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
			this._peakMeter.ColorMedium = System.Drawing.Color.Yellow;
			this._peakMeter.ColorMediumBack = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
			this._peakMeter.ColorNormal = System.Drawing.Color.Green;
			this._peakMeter.ColorNormalBack = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(255)))), ((int)(((byte)(150)))));
			this._peakMeter.FalloffColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
			this._peakMeter.FalloffEffect = false;
			this._peakMeter.GridColor = System.Drawing.Color.Gainsboro;
			this._peakMeter.LEDCount = 15;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._peakMeter, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._peakMeter, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this._peakMeter, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this._peakMeter, "RecordingControl.PeakMeter");
			this._peakMeter.Location = new System.Drawing.Point(658, 375);
			this._peakMeter.Name = "_peakMeter";
			this._peakMeter.ShowGrid = false;
			this._peakMeter.Size = new System.Drawing.Size(20, 109);
			this._peakMeter.TabIndex = 22;
			this._peakMeter.Text = "peakMeterCtrl1";
			// 
			// _lineCountLabel
			// 
			this._lineCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this._lineCountLabel.AutoSize = true;
			this._lineCountLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
			this._lineCountLabel.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._lineCountLabel.ForeColor = System.Drawing.Color.DarkGray;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._lineCountLabel, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._lineCountLabel, null);
			this.l10NSharpExtender1.SetLocalizingId(this._lineCountLabel, "RecordingControl.LineCountLabel");
			this._lineCountLabel.Location = new System.Drawing.Point(430, 220);
			this._lineCountLabel.Name = "_lineCountLabel";
			this._lineCountLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this._lineCountLabel.Size = new System.Drawing.Size(116, 25);
			this._lineCountLabel.TabIndex = 25;
			this._lineCountLabel.Text = "Block {0}/{1}";
			this._lineCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// _endOfUnitMessage
			// 
			this._endOfUnitMessage.BackColor = System.Drawing.Color.Transparent;
			this._endOfUnitMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._endOfUnitMessage.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._endOfUnitMessage, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._endOfUnitMessage, null);
			this.l10NSharpExtender1.SetLocalizingId(this._endOfUnitMessage, "RecordingControl.RecordingToolControl._endOfUnitMessage");
			this._endOfUnitMessage.Location = new System.Drawing.Point(19, 312);
			this._endOfUnitMessage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this._endOfUnitMessage.Name = "_endOfUnitMessage";
			this._endOfUnitMessage.Size = new System.Drawing.Size(356, 50);
			this._endOfUnitMessage.TabIndex = 35;
			this._endOfUnitMessage.Text = "End of Chapter/Book";
			this._endOfUnitMessage.Visible = false;
			// 
			// _nextChapterLink
			// 
			this._nextChapterLink.BackColor = System.Drawing.Color.Transparent;
			this._nextChapterLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._nextChapterLink.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._nextChapterLink, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._nextChapterLink, null);
			this.l10NSharpExtender1.SetLocalizingId(this._nextChapterLink, "RecordingControl.RecordingToolControl._nextChapterLink");
			this._nextChapterLink.Location = new System.Drawing.Point(19, 365);
			this._nextChapterLink.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this._nextChapterLink.Name = "_nextChapterLink";
			this._nextChapterLink.Size = new System.Drawing.Size(356, 50);
			this._nextChapterLink.TabIndex = 36;
			this._nextChapterLink.TabStop = true;
			this._nextChapterLink.Text = "Go To Chapter x";
			this._nextChapterLink.Visible = false;
			this._nextChapterLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnNextChapterLink_LinkClicked);
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "HearThis";
			this.l10NSharpExtender1.PrefixForNewItems = "RecordingControl";
			// 
			// _labelClipOutOfSynchWithBlock
			// 
			this._labelClipOutOfSynchWithBlock.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._labelClipOutOfSynchWithBlock.AutoSize = true;
			this._labelClipOutOfSynchWithBlock.Font = new System.Drawing.Font("Segoe UI", 10F);
			this._labelClipOutOfSynchWithBlock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
			this.l10NSharpExtender1.SetLocalizableToolTip(this._labelClipOutOfSynchWithBlock, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._labelClipOutOfSynchWithBlock, null);
			this.l10NSharpExtender1.SetLocalizingId(this._labelClipOutOfSynchWithBlock, "RecordingControl.ClipOutOfSynchWithBlockLabel");
			this._labelClipOutOfSynchWithBlock.Location = new System.Drawing.Point(36, 0);
			this._labelClipOutOfSynchWithBlock.Name = "_labelClipOutOfSynchWithBlock";
			this._labelClipOutOfSynchWithBlock.Size = new System.Drawing.Size(379, 19);
			this._labelClipOutOfSynchWithBlock.TabIndex = 26;
			this._labelClipOutOfSynchWithBlock.Text = "The text for this block is different from when it was recorded.";
			this._labelClipOutOfSynchWithBlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _linkLabelClickToClearWarning
			// 
			this._linkLabelClickToClearWarning.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(74)))), ((int)(((byte)(135)))));
			this._linkLabelClickToClearWarning.AutoSize = true;
			this._linkLabelClickToClearWarning.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this._linkLabelClickToClearWarning.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
			this.l10NSharpExtender1.SetLocalizableToolTip(this._linkLabelClickToClearWarning, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._linkLabelClickToClearWarning, null);
			this.l10NSharpExtender1.SetLocalizingId(this._linkLabelClickToClearWarning, "RecordingControl.ClickToClearWarningLinkLabel");
			this._linkLabelClickToClearWarning.Location = new System.Drawing.Point(36, 19);
			this._linkLabelClickToClearWarning.Name = "_linkLabelClickToClearWarning";
			this._linkLabelClickToClearWarning.Size = new System.Drawing.Size(140, 13);
			this._linkLabelClickToClearWarning.TabIndex = 27;
			this._linkLabelClickToClearWarning.TabStop = true;
			this._linkLabelClickToClearWarning.Text = "Click to clear this warning";
			this._linkLabelClickToClearWarning.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this._btnAcceptRecording_Click);
			// 
			// _warningIcon
			// 
			this._warningIcon.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this._warningIcon.Image = global::HearThis.Properties.Resources.GreenExclamation;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._warningIcon, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._warningIcon, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this._warningIcon, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this._warningIcon, "RecordingControl.pictureBox1");
			this._warningIcon.Location = new System.Drawing.Point(0, 3);
			this._warningIcon.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this._warningIcon.Name = "_warningIcon";
			this._tableLayoutPanelOutOfSynchWarning.SetRowSpan(this._warningIcon, 2);
			this._warningIcon.Size = new System.Drawing.Size(30, 27);
			this._warningIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._warningIcon.TabIndex = 28;
			this._warningIcon.TabStop = false;
			// 
			// _smallerButton
			//
			this._smallerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._smallerButton.CheckBox = false;
			this._smallerButton.Checked = false;
			this._smallerButton.Image = global::HearThis.Properties.Resources.BottomToolbar_Smaller;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._smallerButton, "Smaller Text");
			this.l10NSharpExtender1.SetLocalizationComment(this._smallerButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._smallerButton, "RecordingControl.SmallerButton");
			this._smallerButton.Location = new System.Drawing.Point(14, 490);
			this._smallerButton.Name = "_smallerButton";
			this._smallerButton.Size = new System.Drawing.Size(18, 24);
			this._smallerButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._smallerButton.TabIndex = 46;
			this._smallerButton.TabStop = false;
			this._smallerButton.Click += new System.EventHandler(this.OnSmallerClick);
			// 
			// _largerButton
			// 
			this._largerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._largerButton.CheckBox = false;
			this._largerButton.Checked = false;
			this._largerButton.Image = global::HearThis.Properties.Resources.BottomToolbar_Larger;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._largerButton, "Larger Text");
			this.l10NSharpExtender1.SetLocalizationComment(this._largerButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._largerButton, "RecordingControl.LargerButton");
			this._largerButton.Location = new System.Drawing.Point(36, 490);
			this._largerButton.Name = "_largerButton";
			this._largerButton.Size = new System.Drawing.Size(23, 24);
			this._largerButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._largerButton.TabIndex = 45;
			this._largerButton.TabStop = false;
			this._largerButton.Click += new System.EventHandler(this.OnLargerClick);
			// 
			// _skipButton
			// 
			this._skipButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._skipButton.CheckBox = true;
			this._skipButton.Checked = false;
			this._skipButton.Image = global::HearThis.Properties.Resources.BottomToolbar_Skip;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._skipButton, "Skip this block - it does not need to be recorded.");
			this.l10NSharpExtender1.SetLocalizationComment(this._skipButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._skipButton, "RecordingControl.skipButton1");
			this._skipButton.Location = new System.Drawing.Point(374, 490);
			this._skipButton.Margin = new System.Windows.Forms.Padding(0);
			this._skipButton.Name = "_skipButton";
			this._skipButton.Size = new System.Drawing.Size(22, 24);
			this._skipButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._skipButton.TabIndex = 44;
			this._skipButton.TabStop = false;
			this._skipButton.CheckedChanged += new System.EventHandler(this.OnSkipButtonCheckedChanged);
			// 
			// _longLineButton
			// 
			this._longLineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._longLineButton.Image = global::HearThis.Properties.Resources.recordLongLineInParts;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._longLineButton, "Record long lines in parts. (p key)");
			this.l10NSharpExtender1.SetLocalizationComment(this._longLineButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._longLineButton, "RecordingControl.RecordLongLinesInParts");
			this._longLineButton.Location = new System.Drawing.Point(132, 494);
			this._longLineButton.Margin = new System.Windows.Forms.Padding(0);
			this._longLineButton.Name = "_longLineButton";
			this._longLineButton.Size = new System.Drawing.Size(54, 30);
			this._longLineButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this._longLineButton.TabIndex = 43;
			this._longLineButton.TabStop = false;
			this._longLineButton.Click += new System.EventHandler(this.longLineButton_Click);
			this._longLineButton.MouseEnter += new System.EventHandler(this._longLineButton_MouseEnter);
			this._longLineButton.MouseLeave += new System.EventHandler(this._longLineButton_MouseLeave);
			// 
			// _recordInPartsButton
			// 
			this._recordInPartsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._recordInPartsButton.CheckBox = false;
			this._recordInPartsButton.Checked = false;
			this._recordInPartsButton.Image = global::HearThis.Properties.Resources.BottomToolbar_RecordInParts;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._recordInPartsButton, "Record long lines in parts. (p key)");
			this.l10NSharpExtender1.SetLocalizationComment(this._recordInPartsButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._recordInPartsButton, "RecordingControl.RecordLongLinesInParts");
			this._recordInPartsButton.Location = new System.Drawing.Point(314, 490);
			this._recordInPartsButton.Margin = new System.Windows.Forms.Padding(0);
			this._recordInPartsButton.Name = "_recordInPartsButton";
			this._recordInPartsButton.Size = new System.Drawing.Size(40, 24);
			this._recordInPartsButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._recordInPartsButton.TabIndex = 43;
			this._recordInPartsButton.TabStop = false;
			this._recordInPartsButton.Click += new System.EventHandler(this.longLineButton_Click);
			// 
			// _tableLayoutPanelNavigationState
			// 
			this._tableLayoutPanelNavigationState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._tableLayoutPanelNavigationState.ColumnCount = 3;
			//
			// _deleteRecordingButton
			// 
			this._deleteRecordingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._deleteRecordingButton.CheckBox = false;
			this._deleteRecordingButton.Checked = false;
			this._deleteRecordingButton.Image = global::HearThis.Properties.Resources.BottomToolbar_Delete;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._deleteRecordingButton, "Remove this recorded clip (Delete Key)");
			this.l10NSharpExtender1.SetLocalizationComment(this._deleteRecordingButton, "Shows as an \'X\' when on a script line that has been recorded.");
			this.l10NSharpExtender1.SetLocalizingId(this._deleteRecordingButton, "RecordingControl.RemoveThisRecording");
			this._deleteRecordingButton.Location = new System.Drawing.Point(619, 490);
			this._deleteRecordingButton.Name = "_deleteRecordingButton";
			this._deleteRecordingButton.Size = new System.Drawing.Size(21, 24);
			this._deleteRecordingButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._deleteRecordingButton.TabIndex = 39;
			this._deleteRecordingButton.TabStop = false;
			this._deleteRecordingButton.Click += new System.EventHandler(this._deleteRecordingButton_Click);
			this._deleteRecordingButton.MouseEnter += new System.EventHandler(this._deleteRecordingButton_MouseEnter);
			this._deleteRecordingButton.MouseLeave += new System.EventHandler(this._deleteRecordingButton_MouseLeave);
			// 
			// _breakLinesAtCommasButton
			// 
			this._breakLinesAtCommasButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._breakLinesAtCommasButton.CheckBox = true;
			this._breakLinesAtCommasButton.Checked = false;
			this._breakLinesAtCommasButton.Image = global::HearThis.Properties.Resources.BottomToolbar_BreakOnCommas;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._breakLinesAtCommasButton, "Start new line at pause punctuation");
			this.l10NSharpExtender1.SetLocalizationComment(this._breakLinesAtCommasButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._breakLinesAtCommasButton, "RecordingControl.BreakLinesAtClauses");
			this._breakLinesAtCommasButton.Location = new System.Drawing.Point(100, 490);
			this._breakLinesAtCommasButton.Name = "_breakLinesAtCommasButton";
			this._breakLinesAtCommasButton.Size = new System.Drawing.Size(28, 24);
			this._breakLinesAtCommasButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._breakLinesAtCommasButton.TabIndex = 38;
			this._breakLinesAtCommasButton.TabStop = false;
			this._breakLinesAtCommasButton.Click += new System.EventHandler(this._breakLinesAtCommasButton_Click);
			// 
			this._tableLayoutPanelNavigationState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanelNavigationState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanelNavigationState.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanelNavigationState.Controls.Add(this._tableLayoutPanelOutOfSynchWarning, 1, 0);
			this._tableLayoutPanelNavigationState.Controls.Add(this._segmentLabel, 0, 0);
			this._tableLayoutPanelNavigationState.Controls.Add(this._lineCountLabel, 2, 0);
			this._tableLayoutPanelNavigationState.Location = new System.Drawing.Point(13, 233);
			this._tableLayoutPanelNavigationState.Name = "_tableLayoutPanelNavigationState";
			this._tableLayoutPanelNavigationState.RowCount = 1;
			this._tableLayoutPanelNavigationState.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelNavigationState.Size = new System.Drawing.Size(667, 40);
			this._tableLayoutPanelNavigationState.TabIndex = 44;
			// 
			// _tableLayoutPanelOutOfSynchWarning
			// 
			this._tableLayoutPanelOutOfSynchWarning.Anchor = System.Windows.Forms.AnchorStyles.None;
			this._tableLayoutPanelOutOfSynchWarning.AutoSize = true;
			this._tableLayoutPanelOutOfSynchWarning.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this._tableLayoutPanelOutOfSynchWarning.ColumnCount = 2;
			this._tableLayoutPanelOutOfSynchWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanelOutOfSynchWarning.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this._tableLayoutPanelOutOfSynchWarning.Controls.Add(this._labelClipOutOfSynchWithBlock, 1, 0);
			this._tableLayoutPanelOutOfSynchWarning.Controls.Add(this._linkLabelClickToClearWarning, 1, 1);
			this._tableLayoutPanelOutOfSynchWarning.Controls.Add(this._warningIcon, 0, 0);
			this._tableLayoutPanelOutOfSynchWarning.Location = new System.Drawing.Point(122, 3);
			this._tableLayoutPanelOutOfSynchWarning.Margin = new System.Windows.Forms.Padding(0);
			this._tableLayoutPanelOutOfSynchWarning.Name = "_tableLayoutPanelOutOfSynchWarning";
			this._tableLayoutPanelOutOfSynchWarning.RowCount = 2;
			this._tableLayoutPanelOutOfSynchWarning.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this._tableLayoutPanelOutOfSynchWarning.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this._tableLayoutPanelOutOfSynchWarning.Size = new System.Drawing.Size(418, 33);
			this._tableLayoutPanelOutOfSynchWarning.TabIndex = 46;
			this._tableLayoutPanelOutOfSynchWarning.Visible = false;
			// 
			// _skipButton
			// 
			this._skipButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._skipButton.Appearance = System.Windows.Forms.Appearance.Button;
			this._skipButton.BackColor = System.Drawing.Color.Transparent;
			this._skipButton.Image = ((System.Drawing.Image)(resources.GetObject("_skipButton.Image")));
			this.l10NSharpExtender1.SetLocalizableToolTip(this._skipButton, "Skip this block - it does not need to be recorded.");
			this.l10NSharpExtender1.SetLocalizationComment(this._skipButton, null);
			this.l10NSharpExtender1.SetLocalizingId(this._skipButton, "RecordingControl.skipButton1");
			this._skipButton.Location = new System.Drawing.Point(569, 481);
			this._skipButton.Name = "_skipButton";
			this._skipButton.Size = new System.Drawing.Size(20, 36);
			this._skipButton.TabIndex = 42;
			this._skipButton.UseVisualStyleBackColor = false;
			this._skipButton.CheckedChanged += new System.EventHandler(this.OnSkipButtonCheckedChanged);
			// 
			// _audioButtonsControl
			// 
			this._audioButtonsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._audioButtonsControl.BackColor = System.Drawing.Color.Transparent;
			this._audioButtonsControl.ButtonHighlightMode = HearThis.UI.AudioButtonsControl.ButtonHighlightModes.Default;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._audioButtonsControl, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._audioButtonsControl, null);
			this.l10NSharpExtender1.SetLocalizingId(this._audioButtonsControl, "RecordingControl.AudioButtonsControl");
			this._audioButtonsControl.Location = new System.Drawing.Point(565, 278);
			this._audioButtonsControl.Name = "_audioButtonsControl";
			this._audioButtonsControl.RecordingDevice = null;
			this._audioButtonsControl.Size = new System.Drawing.Size(123, 43);
			this._audioButtonsControl.TabIndex = 20;
			this._audioButtonsControl.NextClick += new System.EventHandler(this.OnNextButton);
			// 
			// _scriptControl
			// 
			this._scriptControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._scriptControl.BackColor = System.Drawing.Color.Transparent;
			this._scriptControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._scriptControl.ForeColor = System.Drawing.Color.White;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._scriptControl, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._scriptControl, null);
			this.l10NSharpExtender1.SetLocalizingId(this._scriptControl, "RecordingControl.ScriptControl");
			this._scriptControl.Location = new System.Drawing.Point(13, 281);
			this._scriptControl.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this._scriptControl.Name = "_scriptControl";
			this._scriptControl.ShowSkippedBlocks = false;
			this._scriptControl.Size = new System.Drawing.Size(539, 202);
			this._scriptControl.TabIndex = 15;
			this._scriptControl.ZoomFactor = 1F;
			// 
			// _scriptSlider
			// 
			this._scriptSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._scriptSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
			this.l10NSharpExtender1.SetLocalizableToolTip(this._scriptSlider, null);
			this.l10NSharpExtender1.SetLocalizationComment(this._scriptSlider, null);
			this.l10NSharpExtender1.SetLocalizingId(this._scriptSlider, "RecordingControl.ScriptLineSlider");
			this._scriptSlider.Location = new System.Drawing.Point(19, 250);
			this._scriptSlider.Name = "_scriptSlider";
			this._scriptSlider.SegmentCount = 50;
			this._scriptSlider.Size = new System.Drawing.Size(669, 25);
			this._scriptSlider.TabIndex = 11;
			this._scriptSlider.Value = 4;
			this._scriptSlider.ValueChanged += new System.EventHandler(this.OnLineSlider_ValueChanged);
			// 

			// RecordingToolControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(65)))), ((int)(((byte)(65)))));
			this.Controls.Add(this._tableLayoutPanelNavigationState);
			this.Controls.Add(this._smallerButton);
			this.Controls.Add(this._largerButton);
			this.Controls.Add(this._skipButton);
			this.Controls.Add(this._recordInPartsButton);
			this.Controls.Add(this._deleteRecordingButton);
			this.Controls.Add(this._breakLinesAtCommasButton);
			this.Controls.Add(this.recordingDeviceButton1);
			this.Controls.Add(this._peakMeter);
			this.Controls.Add(this._audioButtonsControl);
			this.Controls.Add(this._scriptControl);
			this.Controls.Add(this._endOfUnitMessage);
			this.Controls.Add(this._scriptSlider);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this._nextChapterLink);
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizationPriority(this, L10NSharp.LocalizationPriority.NotLocalizable);
			this.l10NSharpExtender1.SetLocalizingId(this, "RecordingControl.RecordingToolControl.RecordingToolControl");
			this.Margin = new System.Windows.Forms.Padding(10);
			this.Name = "RecordingToolControl";
			this.Size = new System.Drawing.Size(706, 527);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._warningIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._smallerButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._largerButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._skipButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._recordInPartsButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._deleteRecordingButton)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._breakLinesAtCommasButton)).EndInit();
			this._tableLayoutPanelNavigationState.ResumeLayout(false);
			this._tableLayoutPanelNavigationState.PerformLayout();
			this._tableLayoutPanelOutOfSynchWarning.ResumeLayout(false);
			this._tableLayoutPanelOutOfSynchWarning.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel _bookFlow;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label _bookLabel;
        private System.Windows.Forms.Label _chapterLabel;
        private System.Windows.Forms.FlowLayoutPanel _chapterFlow;
        private DiscontiguousProgressTrackBar _scriptSlider;
        private System.Windows.Forms.Label _segmentLabel;
        private ScriptControl _scriptControl;
	    private System.Windows.Forms.Label _endOfUnitMessage;
	    private System.Windows.Forms.LinkLabel _nextChapterLink;
        private AudioButtonsControl _audioButtonsControl;
        private System.Windows.Forms.ToolTip toolTip1;
        private PeakMeterCtrl _peakMeter;
        private System.Windows.Forms.ToolTip _instantToolTip;
        private RecordingDeviceIndicator recordingDeviceButton1;
        private System.Windows.Forms.Label _lineCountLabel;
        private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
        private HearThisToolbarButton _breakLinesAtCommasButton;
		private HearThisToolbarButton _deleteRecordingButton;
		private HearThisToolbarButton _recordInPartsButton;
		private HearThisToolbarButton _skipButton;
		private TableLayoutPanel _tableLayoutPanelNavigationState;
		private Label _labelClipOutOfSynchWithBlock;
		private HearThisToolbarButton _largerButton;
		private HearThisToolbarButton _smallerButton;
		private TableLayoutPanel _tableLayoutPanelOutOfSynchWarning;
		private LinkLabel _linkLabelClickToClearWarning;
		private PictureBox _warningIcon;
	}
}
