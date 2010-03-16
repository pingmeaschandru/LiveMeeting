using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using TW.Coder;
using TW.Coder.Converter;
using TW.Core.Helper;
using TW.H264Coder.Vcl;

namespace TW.H264Coder.Test
{
    [TestFixture]
    public class VideoSequenceTest
    {
        [Test]
        public void ShouldAbleToConvertRgbToH264()
        {
            //var image = (Bitmap)Image.FromFile("Test.jpg");
            //var image1 = (Bitmap)ImageHelper.Resize(image, new Size(704, 576));
            //var finalImage = ImageHelper.Clone(image1, PixelFormat.Format24bppRgb);
            //var rgbFrame = RgbFrameFactory.CreateFrame(finalImage);
            //var converter = new RGB24ToYUV420Converter();
            //var yuvFrame = (YuvFrame)converter.Process(rgbFrame);

            //var vs = new VideoSequence(YuvFormatType.Yuv420, new Size(yuvFrame.Width, yuvFrame.Height));
            //vs.Encode(yuvFrame.Data);
        }
    }
}
