using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
        public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
        public IntPtr hCursor;      // Handle to the cursor. 
        public Point ptScreenPos;   // A Point structure that receives the screen coordinates of the cursor. 
    }
}