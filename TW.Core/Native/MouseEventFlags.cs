using System;

namespace TW.Core.Native
{
    [Flags]
    public enum MouseEventFlags
    {
        Leftdown = 0x00000002,
        Leftup = 0x00000004,
        Middledown = 0x00000020,
        Middleup = 0x00000040,
        Move = 0x00000001,
        Absolute = 0x00008000,
        Rightdown = 0x00000008,
        Rightup = 0x00000010
    }
}