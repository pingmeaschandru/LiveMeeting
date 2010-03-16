using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NUnit.Framework;
using TW.Coder.Converter;

namespace TW.Coder.Test.Converter
{
    [TestFixture]
    public class YUV444ToRGB24ConverterTest
    {
        [Test]
        public void ConvertFromYuv444ToRgb24()
        {
            var image = (Bitmap)Image.FromFile("Test.jpg", false);
            ConverterBase converter = new RGB24ToYUV444Converter();
            var destImage = converter.Process(RgbFrameFactory.CreateFrame(image));

            ConverterBase converter1 = new YUV444ToRGB24Converter();
            var destImage1 = converter1.Process(destImage);

            var bmp = RgbFrameFactory.CreateBitmap((RgbFrame)destImage1);

            bmp.Save("TestFinalYuv444ToRgb24.jpg");
        }
    }
}