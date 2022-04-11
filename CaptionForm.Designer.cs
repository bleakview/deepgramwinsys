using deepgramwinsys.Component;

namespace deepgramwinsys
{
    partial class CaptionForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.captionLabel = new deepgramwinsys.Component.CustomLabel();
            this.SuspendLayout();
            // 
            // captionLabel
            // 
            this.captionLabel.AutoSize = true;
            this.captionLabel.Font = new System.Drawing.Font("Times New Roman", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.captionLabel.ForeColor = System.Drawing.Color.SpringGreen;
            this.captionLabel.Location = new System.Drawing.Point(12, 9);
            this.captionLabel.Name = "captionLabel";
            this.captionLabel.OutlineForeColor = System.Drawing.Color.Black;
            this.captionLabel.OutlineWidth = 4F;
            this.captionLabel.Size = new System.Drawing.Size(213, 40);
            this.captionLabel.TabIndex = 0;
            this.captionLabel.Text = "captionLabel";
            // 
            // CaptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1336, 93);
            this.ControlBox = false;
            this.Controls.Add(this.captionLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Captions";
            this.TopMost = true;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CaptionForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CaptionForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CaptionForm_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public CustomLabel captionLabel;
    }
}