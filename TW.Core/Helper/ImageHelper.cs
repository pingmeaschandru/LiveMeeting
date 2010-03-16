using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace TW.Core.Helper
{
    public class ImageHelper
    {
        public static Bitmap BitmapFromBitmapData(byte[] bitmapData)
        {
            if (bitmapData == null)
                return null;

            var ms = new MemoryStream(bitmapData);
            return (new Bitmap(ms));
        }

        public static byte[] BitmapDataFromBitmap(Bitmap objBitmap)
        {
            if(objBitmap == null)
                return null;

            var ms = new MemoryStream();
            objBitmap.Save(ms, ImageFormat.Bmp);
            return (ms.GetBuffer());
        }

        public static Bitmap Clone(BitmapData sourceData)
        {
            var width = sourceData.Width;
            var height = sourceData.Height;

            var destination = new Bitmap(width, height, sourceData.PixelFormat);

            var destinationData = destination.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, destination.PixelFormat);

            UnmanagedMemoryCopyHelper.CopyUnmanagedMemory(destinationData.Scan0, sourceData.Scan0, height * sourceData.Stride);

            destination.UnlockBits(destinationData);

            return destination;
        }

        public static Bitmap Clone(Bitmap source)
        {
            var sourceData = source.LockBits(
                new Rectangle(0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, source.PixelFormat);

            var destination = Clone(sourceData);

            source.UnlockBits(sourceData);

            if (
                (source.PixelFormat == PixelFormat.Format1bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format4bppIndexed) ||
                (source.PixelFormat == PixelFormat.Format8bppIndexed) ||
                (source.PixelFormat == PixelFormat.Indexed))
            {
                var srcPalette = source.Palette;
                var dstPalette = destination.Palette;

                var n = srcPalette.Entries.Length;

                for (var i = 0; i < n; i++)
                    dstPalette.Entries[i] = srcPalette.Entries[i];

                destination.Palette = dstPalette;
            }

            return destination;
        }

        public static Bitmap Clone(Bitmap source, PixelFormat format)
        {
            if (source.PixelFormat == format)
                return Clone(source);

            var width = source.Width;
            var height = source.Height;

            var bitmap = new Bitmap(width, height, format);

            var g = Graphics.FromImage(bitmap);
            g.DrawImage(source, 0, 0, width, height);
            g.Dispose();

            return bitmap;
        }

        public static Bitmap ConvertToJpeg(Bitmap source, long quality)
        {
            var codecInfo = GetEncoder(ImageFormat.Jpeg);
            if (codecInfo == null)
                throw new Exception();

            var encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            var destBitmapStream = new MemoryStream();
            source.Save(destBitmapStream, codecInfo, encoderParameters);
            return (Bitmap) Image.FromStream(destBitmapStream);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            foreach (var codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;
            
            return null;
        }

        public static Image Resize(Image imgToResize, Size size)
        {
            return Resize(imgToResize, size, imgToResize.PixelFormat);
        }

        public static Image Resize(Image imgToResize, Size size, PixelFormat destinationPixelFormat)
        {
            var destWidth = size.Width;
            var destHeight = size.Height;

            var finalBmp = new Bitmap(destWidth, destHeight, destinationPixelFormat);
            var g = Graphics.FromImage(finalBmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return finalBmp;
        }

        public static Image Crop(Image img, Rectangle cropArea)
        {
            var bmpImage = new Bitmap(img);
            var bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return bmpCrop;
        }
    }
}