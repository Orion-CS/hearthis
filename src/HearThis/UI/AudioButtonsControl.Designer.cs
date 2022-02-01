namespace HearThis.UI
{
    partial class AudioButtonsControl
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
	        if (disposing)
	        {
		        DisposePlayer();
				components?.Dispose();
				_startRecordingTimer.Elapsed -= OnStartRecordingTimer_Elapsed;
				_recordButton.ButtonStateChanged -= OnRecordButtonStateChanged;
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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.l10NSharpExtender1 = new L10NSharp.UI.L10NSharpExtender(this.components);
			this._nextButton = new HearThis.UI.ArrowButton();
			this._playButton = new HearThis.UI.PlayButton();
			this._recordButton = new HearThis.UI.RecordButton();
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).BeginInit();
			this.SuspendLayout();
			// 
			// l10NSharpExtender1
			// 
			this.l10NSharpExtender1.LocalizationManagerId = "HearThis";
			this.l10NSharpExtender1.PrefixForNewItems = "AudioButtonsControl";
			// 
			// _nextButton
			// 
			this._nextButton.BackColor = System.Drawing.Color.Transparent;
			this._nextButton.CancellableMouseDownCall = null;
			this._nextButton.Enabled = false;
			this._nextButton.IsDefault = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._nextButton, "Next script block (PAGE DOWN or RIGHT ARROW key)");
			this.l10NSharpExtender1.SetLocalizationComment(this._nextButton, "Localize the tooltip, not the button name");
			this.l10NSharpExtender1.SetLocalizationPriority(this._nextButton, L10NSharp.LocalizationPriority.Low);
			this.l10NSharpExtender1.SetLocalizingId(this._nextButton, "AudioButtonsControl.NextButton");
			this._nextButton.Location = new System.Drawing.Point(84, 4);
			this._nextButton.Name = "_nextButton";
			this._nextButton.Size = new System.Drawing.Size(32, 33);
			this._nextButton.State = HearThis.UI.BtnState.Normal;
			this._nextButton.TabIndex = 28;
			this._nextButton.Text = "_nextButton";
			this._nextButton.Click += new System.EventHandler(this.OnNextClick);
			// 
			// _playButton
			// 
			this._playButton.BackColor = System.Drawing.Color.Transparent;
			this._playButton.CancellableMouseDownCall = null;
			this._playButton.Enabled = false;
			this._playButton.IsDefault = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._playButton, "Play the clip for this block (TAB key)");
			this.l10NSharpExtender1.SetLocalizationComment(this._playButton, "Localize the tooltip, not the button name");
			this.l10NSharpExtender1.SetLocalizationPriority(this._playButton, L10NSharp.LocalizationPriority.Low);
			this.l10NSharpExtender1.SetLocalizingId(this._playButton, "AudioButtonsControl.PlayButton");
			this._playButton.Location = new System.Drawing.Point(4, 5);
			this._playButton.Name = "_playButton";
			this._playButton.Playing = false;
			this._playButton.Size = new System.Drawing.Size(29, 31);
			this._playButton.State = HearThis.UI.BtnState.Normal;
			this._playButton.TabIndex = 24;
			this._playButton.Text = "playButton1";
			this._playButton.Click += new System.EventHandler(this.OnPlay);
			// 
			// _recordButton
			// 
			this._recordButton.BackColor = System.Drawing.Color.Transparent;
			this._recordButton.CancellableMouseDownCall = null;
			this._recordButton.Enabled = false;
			this._recordButton.IsDefault = false;
			this.l10NSharpExtender1.SetLocalizableToolTip(this._recordButton, "Record this block. (Press and hold the mouse or the SPACEBAR.)");
			this.l10NSharpExtender1.SetLocalizationComment(this._recordButton, "Localize the tooltip, not the button name");
			this.l10NSharpExtender1.SetLocalizationPriority(this._recordButton, L10NSharp.LocalizationPriority.Low);
			this.l10NSharpExtender1.SetLocalizingId(this._recordButton, "AudioButtonsControl.RecordButton");
			this._recordButton.Location = new System.Drawing.Point(40, 5);
			this._recordButton.Name = "_recordButton";
			this._recordButton.Size = new System.Drawing.Size(39, 31);
			this._recordButton.State = HearThis.UI.BtnState.Normal;
			this._recordButton.TabIndex = 23;
			this._recordButton.Text = "recordButton1";
			this._recordButton.Waiting = false;
			this._recordButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnRecordUp);
			// 
			// AudioButtonsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this._nextButton);
			this.Controls.Add(this._playButton);
			this.Controls.Add(this._recordButton);
			this.l10NSharpExtender1.SetLocalizableToolTip(this, null);
			this.l10NSharpExtender1.SetLocalizationComment(this, null);
			this.l10NSharpExtender1.SetLocalizingId(this, "AudioButtonsControl.AudioButtonsControl.AudioButtonsControl");
			this.Name = "AudioButtonsControl";
			this.Size = new System.Drawing.Size(145, 40);
			this.Load += new System.EventHandler(this.RecordAndPlayControl_Load);
			((System.ComponentModel.ISupportInitialize)(this.l10NSharpExtender1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private RecordButton _recordButton;
        private PlayButton _playButton;
		private ArrowButton _nextButton;
		private L10NSharp.UI.L10NSharpExtender l10NSharpExtender1;
    }
}
