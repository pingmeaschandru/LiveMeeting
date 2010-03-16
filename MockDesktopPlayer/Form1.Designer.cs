namespace MockDesktopPlayer
{
    partial class Form1
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
            this.desktopPlayerControl1 = new MockDesktopPlayer.DesktopPlayerControl();
            this.SuspendLayout();
            // 
            // desktopPlayerControl1
            // 
            this.desktopPlayerControl1.Location = new System.Drawing.Point(12, 1);
            this.desktopPlayerControl1.Name = "desktopPlayerControl1";
            this.desktopPlayerControl1.Size = new System.Drawing.Size(646, 486);
            this.desktopPlayerControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(668, 495);
            this.Controls.Add(this.desktopPlayerControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.KeyPress_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion

        private DesktopPlayerControl desktopPlayerControl1;
    }
}