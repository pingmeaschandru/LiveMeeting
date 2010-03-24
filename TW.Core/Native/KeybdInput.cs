using System;

namespace TW.Core.Native
{
    public struct KeybdInput
    {
        public UInt16 Vk;
        public UInt16 Scan;
        public UInt32 Flags;
        public UInt32 Time;
        public IntPtr ExtraInfo;
    }
}