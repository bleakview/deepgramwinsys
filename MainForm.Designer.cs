namespace deepgramwinsys
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStartCapture = new System.Windows.Forms.Button();
            this.buttonEndCapture = new System.Windows.Forms.Button();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.comboBoxModel = new System.Windows.Forms.ComboBox();
            this.checkBoxSaveMP3 = new System.Windows.Forms.CheckBox();
            this.checkBoxSaveAsCSV = new System.Windows.Forms.CheckBox();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.checkBoxProfinityAllowed = new System.Windows.Forms.CheckBox();
            this.buttonOpenRecordingFolder = new System.Windows.Forms.Button();
            this.checkBoxHideCaptionTitle = new System.Windows.Forms.CheckBox();
            this.checkBoxTransparentBackground = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonStartCapture
            // 
            this.buttonStartCapture.Location = new System.Drawing.Point(8, 225);
            this.buttonStartCapture.Name = "buttonStartCapture";
            this.buttonStartCapture.Size = new System.Drawing.Size(138, 23);
            this.buttonStartCapture.TabIndex = 2;
            this.buttonStartCapture.Text = "Start Capture";
            this.buttonStartCapture.UseVisualStyleBackColor = true;
            this.buttonStartCapture.Click += new System.EventHandler(this.buttonStartCapture_Click);
            // 
            // buttonEndCapture
            // 
            this.buttonEndCapture.Enabled = false;
            this.buttonEndCapture.Location = new System.Drawing.Point(152, 225);
            this.buttonEndCapture.Name = "buttonEndCapture";
            this.buttonEndCapture.Size = new System.Drawing.Size(146, 23);
            this.buttonEndCapture.TabIndex = 3;
            this.buttonEndCapture.Text = "End Capture";
            this.buttonEndCapture.UseVisualStyleBackColor = true;
            this.buttonEndCapture.Click += new System.EventHandler(this.buttonEndCapture_Click);
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(8, 38);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(208, 23);
            this.comboBoxLanguage.TabIndex = 4;
            this.comboBoxLanguage.SelectedValueChanged += new System.EventHandler(this.comboBoxLanguage_SelectedValueChanged);
            // 
            // comboBoxModel
            // 
            this.comboBoxModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxModel.FormattingEnabled = true;
            this.comboBoxModel.Location = new System.Drawing.Point(8, 67);
            this.comboBoxModel.Name = "comboBoxModel";
            this.comboBoxModel.Size = new System.Drawing.Size(206, 23);
            this.comboBoxModel.TabIndex = 5;
            // 
            // checkBoxSaveMP3
            // 
            this.checkBoxSaveMP3.AutoSize = true;
            this.checkBoxSaveMP3.Checked = true;
            this.checkBoxSaveMP3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSaveMP3.Location = new System.Drawing.Point(8, 100);
            this.checkBoxSaveMP3.Name = "checkBoxSaveMP3";
            this.checkBoxSaveMP3.Size = new System.Drawing.Size(94, 19);
            this.checkBoxSaveMP3.TabIndex = 6;
            this.checkBoxSaveMP3.Text = "Save as MP3 ";
            this.checkBoxSaveMP3.UseVisualStyleBackColor = true;
            // 
            // checkBoxSaveAsCSV
            // 
            this.checkBoxSaveAsCSV.AutoSize = true;
            this.checkBoxSaveAsCSV.Checked = true;
            this.checkBoxSaveAsCSV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSaveAsCSV.Location = new System.Drawing.Point(8, 125);
            this.checkBoxSaveAsCSV.Name = "checkBoxSaveAsCSV";
            this.checkBoxSaveAsCSV.Size = new System.Drawing.Size(88, 19);
            this.checkBoxSaveAsCSV.TabIndex = 7;
            this.checkBoxSaveAsCSV.Text = "Save as CSV";
            this.checkBoxSaveAsCSV.UseVisualStyleBackColor = true;
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Location = new System.Drawing.Point(63, 9);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(155, 23);
            this.textBoxApiKey.TabIndex = 8;
            // 
            // labelApiKey
            // 
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(8, 12);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(47, 15);
            this.labelApiKey.TabIndex = 9;
            this.labelApiKey.Text = "Api Key";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(8, 254);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(138, 23);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // checkBoxProfinityAllowed
            // 
            this.checkBoxProfinityAllowed.AutoSize = true;
            this.checkBoxProfinityAllowed.Checked = true;
            this.checkBoxProfinityAllowed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxProfinityAllowed.Location = new System.Drawing.Point(8, 150);
            this.checkBoxProfinityAllowed.Name = "checkBoxProfinityAllowed";
            this.checkBoxProfinityAllowed.Size = new System.Drawing.Size(117, 19);
            this.checkBoxProfinityAllowed.TabIndex = 11;
            this.checkBoxProfinityAllowed.Text = "Profinity Allowed";
            this.checkBoxProfinityAllowed.UseVisualStyleBackColor = true;
            // 
            // buttonOpenRecordingFolder
            // 
            this.buttonOpenRecordingFolder.Location = new System.Drawing.Point(152, 254);
            this.buttonOpenRecordingFolder.Name = "buttonOpenRecordingFolder";
            this.buttonOpenRecordingFolder.Size = new System.Drawing.Size(146, 23);
            this.buttonOpenRecordingFolder.TabIndex = 12;
            this.buttonOpenRecordingFolder.Text = "Open Recording Folder";
            this.buttonOpenRecordingFolder.UseVisualStyleBackColor = true;
            this.buttonOpenRecordingFolder.Click += new System.EventHandler(this.buttonOpenRecordingFolder_Click);
            // 
            // checkBoxHideCaptionTitle
            // 
            this.checkBoxHideCaptionTitle.AutoSize = true;
            this.checkBoxHideCaptionTitle.Location = new System.Drawing.Point(8, 175);
            this.checkBoxHideCaptionTitle.Name = "checkBoxHideCaptionTitle";
            this.checkBoxHideCaptionTitle.Size = new System.Drawing.Size(121, 19);
            this.checkBoxHideCaptionTitle.TabIndex = 13;
            this.checkBoxHideCaptionTitle.Text = "Hide Caption Title";
            this.checkBoxHideCaptionTitle.UseVisualStyleBackColor = true;
            this.checkBoxHideCaptionTitle.CheckedChanged += new System.EventHandler(this.checkBoxHideCaptionTitle_CheckedChanged);
            // 
            // checkBoxTransparentBackground
            // 
            this.checkBoxTransparentBackground.AutoSize = true;
            this.checkBoxTransparentBackground.Location = new System.Drawing.Point(8, 200);
            this.checkBoxTransparentBackground.Name = "checkBoxTransparentBackground";
            this.checkBoxTransparentBackground.Size = new System.Drawing.Size(154, 19);
            this.checkBoxTransparentBackground.TabIndex = 14;
            this.checkBoxTransparentBackground.Text = "Transparent Background";
            this.checkBoxTransparentBackground.UseVisualStyleBackColor = true;
            this.checkBoxTransparentBackground.CheckedChanged += new System.EventHandler(this.checkBoxTransparentBackground_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 321);
            this.Controls.Add(this.checkBoxTransparentBackground);
            this.Controls.Add(this.checkBoxHideCaptionTitle);
            this.Controls.Add(this.buttonOpenRecordingFolder);
            this.Controls.Add(this.checkBoxProfinityAllowed);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelApiKey);
            this.Controls.Add(this.textBoxApiKey);
            this.Controls.Add(this.checkBoxSaveAsCSV);
            this.Controls.Add(this.checkBoxSaveMP3);
            this.Controls.Add(this.comboBoxModel);
            this.Controls.Add(this.comboBoxLanguage);
            this.Controls.Add(this.buttonEndCapture);
            this.Controls.Add(this.buttonStartCapture);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Caption";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button buttonStartCapture;
        private Button buttonEndCapture;
        private ComboBox comboBoxLanguage;
        private ComboBox comboBoxModel;
        private CheckBox checkBoxSaveMP3;
        private CheckBox checkBoxSaveAsCSV;
        private TextBox textBoxApiKey;
        private Label labelApiKey;
        private Button buttonClose;
        private CheckBox checkBoxProfinityAllowed;
        private Button buttonOpenRecordingFolder;
        private CheckBox checkBoxHideCaptionTitle;
        private CheckBox checkBoxTransparentBackground;
    }
}