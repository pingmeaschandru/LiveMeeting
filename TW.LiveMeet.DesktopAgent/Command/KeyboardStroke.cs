using System.Collections.Generic;
using TW.Core.Native;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class KeyboardStroke : ICommand
    {
        private readonly int key;
        private readonly bool alt;
        private readonly bool control;
        private readonly bool shift;

        public KeyboardStroke(ushort key, bool alt, bool control, bool shift)
        {
            this.key = key;
            this.alt = alt;
            this.control = control;
            this.shift = shift;
        }

        public void Execute()
        {
            var keyCodes = new List<VirtualKeyCode>(); 
            if (alt) keyCodes.Add(VirtualKeyCode.MENU);
            if (control) keyCodes.Add(VirtualKeyCode.CONTROL);
            if (shift) keyCodes.Add(VirtualKeyCode.SHIFT);

            KeyboardInputSimulator.SimulateModifiedKeyStroke(keyCodes, (VirtualKeyCode)key);
        }
    }
}