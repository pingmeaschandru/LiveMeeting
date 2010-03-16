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
            User32.mouse_event((uint)User32.Flags.Leftdown, 0, 0, 0, 0);
            User32.mouse_event((uint)User32.Flags.Leftup, 0, 0, 0, 0);
        }
    }
}
