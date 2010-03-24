using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WaveHdr
    {
        public IntPtr lpData; 
        public int dwBufferLength; 
        public int dwBytesRecorded; 
        public IntPtr dwUser; 
        public int dwFlags; 
        public int dwLoops; 
        public IntPtr lpNext;
        public int reserved;
    }
}