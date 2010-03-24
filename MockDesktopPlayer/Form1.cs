using System;
using System.Windows.Forms;

namespace MockDesktopPlayer
{
    public partial class Form1 : Form
    {
        private bool alt;
        private bool shift;
        private bool control;

        public Form1()
        {
            InitializeComponent();
        }

        private void KeyPress_Load(object sender, EventArgs e)
        {
            KeyPreview = true;
        }


        //protected override void OnKeyPress(KeyPressEventArgs e)
        //{
        //    //lock (this)
        //    //{
        //        e.Handled = true;
        //        desktopPlayerControl1.SendKeyEvent(e.KeyChar, alt, shift, control);
        //   // }
        //}

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    //lock (this)
        //    //{
        //        alt = e.Alt;
        //        control = e.Control;
        //        shift = e.Shift;
        //        e.Handled = true;
        //    //}
        //}


        //private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //lock (this)
        //    //{
        //    e.Handled = true;
        //    desktopPlayerControl1.SendKeyEvent(e.KeyChar, alt, shift, control);
        //    // }
        //}

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            lock (this)
            {
            //alt = e.Alt;
            //control = e.Control;
            //shift = e.Shift;
            e.Handled = true;
            if(e.KeyCode > 0x00)
                desktopPlayerControl1.SendKeyEvent((ushort)e.KeyCode, e.Alt, e.Control, e.Shift);
            }
        }


        private void Form1_Closed(object sender, EventArgs e)
        {
            desktopPlayerControl1.Close();
        }
    }
}
