using System;
using System.Windows.Forms;

namespace MockDesktopPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void KeyPress_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            desktopPlayerControl1.SendKeyEvent(e.KeyChar);
        }
    }
}
