using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using TW.Coder;
using TW.Core.Helper;
using TW.Core.Native;
using Rectangle=System.Drawing.Rectangle;

namespace TW.LiveMeet.DesktopAgent.Command
{
    public class DesktopCapture
    {
        private readonly PixelFormat pixelFormat;
        private readonly Size imageSize;
        private readonly IntPtr desktopHandle;
        private readonly Size desktopActualSize;
        private readonly double widthAspectRatio;
        private readonly double heightAspectRatio;

        public DesktopCapture(PixelFormat pixelFormat, Size imageSize)
        {
            this.pixelFormat = pixelFormat;
            this.imageSize = imageSize;

            desktopHandle = User32.GetDesktopWindow();
            var windowRect = new Core.Native.Rectangle();
            User32.GetWindowRect(desktopHandle, ref windowRect);
            var desktopWidth = windowRect.Right - windowRect.Left;
            var desktopHeight = windowRect.Bottom - windowRect.Top;

            widthAspectRatio = (double) desktopWidth/imageSize.Width;
            heightAspectRatio = (double) desktopHeight/imageSize.Height;

            desktopActualSize = new Size(desktopWidth, desktopHeight);
        }

        public RgbFrame Execute()
        {
            try
            {
                var cursorX = 0;
                var cursorY = 0;
                var desktopBMP = CaptureDesktop();
                var cursorBMP = CaptureCursor(ref cursorX, ref cursorY);

                if (desktopBMP == null || cursorBMP == null) return null;

                var r = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                var g = Graphics.FromImage(desktopBMP);
                g.DrawImage(cursorBMP, r);
                g.Flush();

                return RgbFrameFactory.CreateFrame((Bitmap)ImageHelper.Resize(desktopBMP, imageSize, pixelFormat));
            }
            catch
            {
            }

            return null;
        }

        private Bitmap CaptureCursor(ref int x, ref int y)
        {
            var ci = new CursorInfo();
            ci.cbSize = Marshal.SizeOf(ci);
            if (!User32.GetCursorInfo(out ci))
                return null;

            if (ci.flags != User32.CURSOR_SHOWING)
                return null;

            IconInfo icInfo;
            var hicon = User32.CopyIcon(ci.hCursor);
            if (!User32.GetIconInfo(hicon, out icInfo))
                return null;

            x = ci.ptScreenPos.x - icInfo.xHotspot;
            y = ci.ptScreenPos.y - icInfo.yHotspot;

            return Icon.FromHandle(hicon).ToBitmap();
        }

        private Bitmap CaptureDesktop()
        {
            var hDC = User32.GetDC(desktopHandle);
            var hMemDC = Gdi32.CreateCompatibleDC(hDC);
            var hBitmap = Gdi32.CreateCompatibleBitmap(hDC, DesktopActualSize.Width, DesktopActualSize.Height);
            if (hBitmap == IntPtr.Zero)
                return null;

            var hOld = Gdi32.SelectObject(hMemDC, hBitmap);
            Gdi32.BitBlt(hMemDC, 0, 0, DesktopActualSize.Width, DesktopActualSize.Height, hDC, 0, 0, Gdi32.SOURCE_COPY);
            Gdi32.SelectObject(hMemDC, hOld);
            Gdi32.DeleteDC(hMemDC);
            User32.ReleaseDC(User32.GetDesktopWindow(), hDC);
            var bmp = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);
            GC.Collect();
            return bmp;
        }

        public Size DesktopActualSize
        {
            get { return desktopActualSize; }
        }

        public double WidthAspectRatio
        {
            get { return widthAspectRatio; }
        }

        public double HeightAspectRatio
        {
            get { return heightAspectRatio; }
        }
    }
}