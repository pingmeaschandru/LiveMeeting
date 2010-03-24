using TW.Core.Native;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class MouseRight : ICommand
    {
        private readonly int x;
        private readonly int y;

        public MouseRight(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Execute()
        {
            User32.SetCursorPos(x, y);
            User32.mouse_event((uint)MouseEventFlags.Rightdown, 0, 0, 0, 0);
            User32.mouse_event((uint)MouseEventFlags.Rightup, 0, 0, 0, 0);
        }
    }
}