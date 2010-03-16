using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NUnit.Framework;
using TW.Coder.Converter;

namespace TW.Coder.Test.Converter
{
    [TestFixture]
    public class RGB24ToYUV420ConverterTest
    {
        [Test]
        public void ConvertFromRgb24ToYuv420()
        {
            var image = (Bitmap)Image.FromFile("Test.jpg", false);
            var converter = new RGB24ToYUV420Converter();
            var destImage = converter.Process(RgbFrameFactory.CreateFrame(image));

            var bmp = new Bitmap(destImage.Width, destImage.Height, PixelFormat.Format24bppRgb);
            var bData = bmp.LockBits(new Rectangle(0, 0, destImage.Width, destImage.Height),
                                     ImageLockMode.WriteOnly,
                                     PixelFormat.Format24bppRgb);
            Marshal.Copy(destImage.Data, 0, bData.Scan0, destImage.Data.Length);
            bmp.UnlockBits(bData);

            bmp.Save("TestFinalRgb24ToYuv420.jpg");
        }
    }
}