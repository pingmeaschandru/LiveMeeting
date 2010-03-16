using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;
using TW.LiveMeet.DesktopAgent.Command;

namespace TW.LiveMeet.DesktopAgent.Test.Command
{
    [TestFixture]
    public class DesktopCaptureTest
    {
        [Test]
        public void ShouldCaptureDesktopImage()
        {
            var command = new DesktopCapture(PixelFormat.Format24bppRgb, new Size(704, 576));
            var frame = command.Execute();

            Assert.IsNotNull(frame);
            Assert.AreEqual(576, frame.Height);
            Assert.AreEqual(704, frame.Width);
        }
    }
}
