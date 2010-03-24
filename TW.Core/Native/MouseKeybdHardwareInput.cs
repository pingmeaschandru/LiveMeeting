using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MouseKeybdHardwareInput
    {
        [FieldOffset(0)]
        public MouseInput Mouse;
        [FieldOffset(0)]
        public KeybdInput Keyboard;
        [FieldOffset(0)]
        public HardwareInput Hardware;
    }
}