using NUnit.Framework;

namespace TW.Coder.Test
{
    [TestFixture]
    public class YuvFrameTest
    {
        [Test]
        public void ShouldAbleToCreateAndDestroyYuv410Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV410);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);

            const int xChromaShift = 2;
            const int yChromaShift = 2;
            var width2 = (frame.Width + (1 << xChromaShift) - 1) >> xChromaShift;
            var height2 = (frame.Height + (1 << yChromaShift) - 1) >> yChromaShift;
            var dataLength = (frame.Width * frame.Height) + (2 * (width2 * height2));

            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, dataLength);
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyYuv411Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV411);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);

            const int xChromaShift = 2;
            const int yChromaShift = 0;
            var width2 = (frame.Width + (1 << xChromaShift) - 1) >> xChromaShift;
            var height2 = (frame.Height + (1 << yChromaShift) - 1) >> yChromaShift;
            var dataLength = (frame.Width * frame.Height) + (2 * (width2 * height2));

            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, dataLength);
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyYuv420Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV420);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);

            const int xChromaShift = 1;
            const int yChromaShift = 1;
            var width2 = (frame.Width + (1 << xChromaShift) - 1) >> xChromaShift;
            var height2 = (frame.Height + (1 << yChromaShift) - 1) >> yChromaShift;
            var dataLength = (frame.Width * frame.Height) + (2 * (width2 * height2));

            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, dataLength);
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyYuv422Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV422);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);

            const int xChromaShift = 1;
            const int yChromaShift = 0;
            var width2 = (frame.Width + (1 << xChromaShift) - 1) >> xChromaShift;
            var height2 = (frame.Height + (1 << yChromaShift) - 1) >> yChromaShift;
            var dataLength = (frame.Width * frame.Height) + (2 * (width2 * height2));

            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, dataLength);
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyYuv440Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV440);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);

            const int xChromaShift = 0;
            const int yChromaShift = 1;
            var width2 = (frame.Width + (1 << xChromaShift) - 1) >> xChromaShift;
            var height2 = (frame.Height + (1 << yChromaShift) - 1) >> yChromaShift;
            var dataLength = (frame.Width * frame.Height) + (2 * (width2 * height2));

            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, dataLength);
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyYuv444Frame()
        {
            var frame = new YuvFrame(50, 50, PixelFormatType.PIX_FMT_YUV444);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);
            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, (50 * 50 * 3));
        }
    }
}
