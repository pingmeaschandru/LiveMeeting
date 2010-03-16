using System.Threading;
using TW.Core.Native;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class MouseDrag : ICommand
    {
        private readonly int startx;
        private readonly int endx;
        private readonly int starty;
        private readonly int endy;
        private readonly int button;

        public MouseDrag(int startx, int endx, int starty, int endy, int button)
        {
            this.startx = startx;
            this.endx = endx;
            this.starty = starty;
            this.endy = endy;
            this.button = button;
        }

        public void Execute()
        {
            var rightdown = (uint)User32.Flags.Rightdown;
            var rightup = (uint)User32.Flags.Rightup;

            if (button == 1)
            {
                rightdown = (uint)User32.Flags.Leftdown;
                rightup = (uint)User32.Flags.Leftup;
            }

            User32.SetCursorPos(startx, starty);
            Thread.Sleep(10);

            User32.mouse_event(rightdown, 0, 0, 0, 0);
            Thread.Sleep(10);

            User32.SetCursorPos(endx, endy);
            Thread.Sleep(10);

            User32.mouse_event(rightup, 0, 0, 0, 0);
        }
    }
}