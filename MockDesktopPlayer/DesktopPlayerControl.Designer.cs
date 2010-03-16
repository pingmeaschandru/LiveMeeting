namespace MockDesktopPlayer
{
    partial class DesktopPlayerControl
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
            this.pbHeader = new System.Windows.Forms.PictureBox();
            this.pbDesktopCapturePane = new System.Windows.Forms.PictureBox();
            this.pbFooter = new System.Windows.Forms.PictureBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDesktopCapturePane)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFooter)).BeginInit();
            this.SuspendLayout();
            // 
            // pbHeader
            // 
            this.pbHeader.Location = new System.Drawing.Point(2, 3);
            this.pbHeader.Name = "pbHeader";
            this.pbHeader.Size = new System.Drawing.Size(640, 31);
            this.pbHeader.TabIndex = 0;
            this.pbHeader.TabStop = false;
            // 
            // pbDesktopCapturePane
            // 
            this.pbDesktopCapturePane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbDesktopCapturePane.Location = new System.Drawing.Point(2, 35);
            this.pbDesktopCapturePane.Name = "pbDesktopCapturePane";
            this.pbDesktopCapturePane.Size = new System.Drawing.Size(640, 400);
            this.pbDesktopCapturePane.TabIndex = 1;
            this.pbDesktopCapturePane.TabStop = false;
            this.pbDesktopCapturePane.MouseMove += new System.Windows.Forms.MouseEventHandler(this.OnMouseMove);
            this.pbDesktopCapturePane.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
            this.pbDesktopCapturePane.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);
            // 
            // pbFooter
            // 
            this.pbFooter.Location = new System.Drawing.Point(3, 439);
            this.pbFooter.Name = "pbFooter";
            this.pbFooter.Size = new System.Drawing.Size(639, 39);
            this.pbFooter.TabIndex = 2;
            this.pbFooter.TabStop = false;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(537, 445);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(85, 27);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.play);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(537, 446);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(85, 26);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.stop);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(12, 453);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 13);
            this.StatusLabel.TabIndex = 4;
            // 
            // DesktopPlayerControl
            // 
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.pbFooter);
            this.Controls.Add(this.pbDesktopCapturePane);
            this.Controls.Add(this.pbHeader);
            this.Name = "DesktopPlayerControl";
            this.Size = new System.Drawing.Size(653, 483);
            ((System.ComponentModel.ISupportInitialize)(this.pbHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDesktopCapturePane)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbFooter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.StatusLabel.Text = "X = " + e.X + " , Y = " + e.Y + " , MaxX = " + this.pbDesktopCapturePane.Size.Width +
                               " , MaxY = " + this.pbDesktopCapturePane.Size.Height+" , ActualX = "+(e.X * 2)+" , ActualY = "+(e.Y *2);
        }
        #endregion


        private System.Windows.Forms.PictureBox pbHeader;
        private System.Windows.Forms.PictureBox pbFooter;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.PictureBox pbDesktopCapturePane;
        private System.Windows.Forms.Label StatusLabel;
    }
}
