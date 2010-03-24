using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public Int32 x;
        public Int32 y;
    }
}