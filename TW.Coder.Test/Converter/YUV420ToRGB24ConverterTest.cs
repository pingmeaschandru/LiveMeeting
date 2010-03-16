using System.Drawing;
using NUnit.Framework;
using TW.Coder.Converter;

namespace TW.Coder.Test.Converter
{
    [TestFixture]
    public class YUV420ToRGB24ConverterTest
    {
        [Test]
        public void ConvertFromYuv420ToRgb24()
        {
            var image = (Bitmap)Image.FromFile("Test.jpg", false);
            ConverterBase converter = new RGB24ToYUV420Converter();
            var destImage = converter.Process(RgbFrameFactory.CreateFrame(image));

            ConverterBase converter1 = new YUV420ToRGB24Converter();
            var destImage1 = converter1.Process(destImage);

            var bmp = RgbFrameFactory.CreateBitmap((RgbFrame) destImage1);

            bmp.Save("TestFinalYuv420ToRgb24.jpg");
        }
    }
}