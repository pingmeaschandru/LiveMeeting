using TW.Core.Native;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class MouseLeft : ICommand
    {
        private readonly int x;
        private readonly int y;

        public MouseLeft(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Execute()
        {
            User32.SetCursorPos(x, y);
            User32.mouse_event((uint)MouseEventFlags.Leftdown, 0, 0, 0, 0);
            User32.mouse_event((uint)MouseEventFlags.Leftup, 0, 0, 0, 0);
        }
    }
}
