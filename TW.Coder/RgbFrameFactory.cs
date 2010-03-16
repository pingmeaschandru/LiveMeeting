using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace TW.Coder
{
    public static class RgbFrameFactory
    {
        public static RgbFrame CreateFrame(Bitmap image)
        {
            RgbFrame destFrame;
            var sourceData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            try
            {
                destFrame = CreateFrame(sourceData);
            }
            finally
            {
                image.UnlockBits(sourceData);
            }

            return destFrame;
        }

        public static RgbFrame CreateFrame(BitmapData imageData)
        {
            var pixelFormatType = GetPixelFormatType(imageData.PixelFormat);
            var rgbFrame = new RgbFrame(imageData.Width, imageData.Height, pixelFormatType);
            Marshal.Copy(imageData.Scan0, rgbFrame.Data, 0, rgbFrame.Data.Length);

            return rgbFrame;
        }

        public static Bitmap CreateBitmap(RgbFrame frameData)
        {
            var bmp = new Bitmap(frameData.Width, frameData.Height, GetPixelFormatType(frameData.PixelFormatType));
            var bData = bmp.LockBits(new Rectangle(0, 0, frameData.Width, frameData.Height),
                                     ImageLockMode.WriteOnly,
                                     bmp.PixelFormat);
            Marshal.Copy(frameData.Data, 0, bData.Scan0, frameData.Data.Length);
            bmp.UnlockBits(bData);

            return bmp;
        }

        private static PixelFormatType GetPixelFormatType(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format24bppRgb: return PixelFormatType.PIX_FMT_RGB24;
                case PixelFormat.Format32bppRgb: return PixelFormatType.PIX_FMT_RGB32;
                default:
                    throw new Exception("Can not create image with specified pixel format.");
            }
        }

        private static PixelFormat GetPixelFormatType(PixelFormatType pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormatType.PIX_FMT_RGB24: return PixelFormat.Format24bppRgb;
                case PixelFormatType.PIX_FMT_RGB32: return PixelFormat.Format32bppRgb;
                default:
                    throw new Exception("Can not create image with specified pixel format.");
            }
        }
    }
}
