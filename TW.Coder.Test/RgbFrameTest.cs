using System;
using NUnit.Framework;

namespace TW.Coder.Test
{
    [TestFixture]
    public class RgbFrameTest
    {
        [Test]
        public void ShouldAbleToCreateAndDestroyRgb24Frame()
        {
            var frame = new RgbFrame(50, 50, PixelFormatType.PIX_FMT_RGB24);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);
            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, (50 * 50 * 3));
        }

        [Test]
        public void ShouldAbleToCreateAndDestroyRgb32Frame()
        {
            var frame = new RgbFrame(50, 50, PixelFormatType.PIX_FMT_RGB32);
            Assert.AreEqual(frame.Width, 50);
            Assert.AreEqual(frame.Height, 50);
            Assert.AreNotEqual(frame.Data, null);
            Assert.AreEqual(frame.Data.Length, (50 * 50 * 4));
        }
    }
}
