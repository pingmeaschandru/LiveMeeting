using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    public class Gdi32
    {
        public const int SOURCE_COPY = 0x00CC0020;

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject,
                                         int nXDest,
                                         int nYDest,
                                         int nWidth,
                                         int nHeight,
                                         IntPtr hObjectSource,
                                         int nXSrc,
                                         int nYSrc,
                                         int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDc);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);
    }
}