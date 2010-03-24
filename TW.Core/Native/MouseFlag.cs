namespace TW.Core.Native
{
    public enum MouseFlag : uint
    {
        MOVE = 0x0001,
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MIDDLEDOWN = 0x0020,
        MIDDLEUP = 0x0040,
        XDOWN = 0x0080,
        XUP = 0x0100,
        WHEEL = 0x0800,
        VIRTUALDESK = 0x4000,
        ABSOLUTE = 0x8000,
    }
}