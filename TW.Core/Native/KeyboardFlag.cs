using System;

namespace TW.Core.Native
{
    [Flags]
    public enum KeyboardFlag : uint
    {
        EXTENDEDKEY = 0x0001,
        KEYUP = 0x0002,
        UNICODE = 0x0004,
        SCANCODE = 0x0008,
    }
}