using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct IconInfo
    {
        public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
        public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
        public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
        public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
        public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
    }
}