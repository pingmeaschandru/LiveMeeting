using System;
using System.Text;
using System.Windows.Forms;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class KeyboardStroke : ICommand
    {
        private readonly int key;

        public KeyboardStroke(int key)
        {
            this.key = key;
        }

        public void Execute()
        {
            byte[] b = { Convert.ToByte(key) };
            var str = Encoding.ASCII.GetString(b);

            SendKeys.SendWait(str);
        }
    }
}