using System.Drawing;
using NUnit.Framework;
using TW.Core.Native;
using TW.LiveMeet.DesktopAgent.Command;

namespace TW.LiveMeet.DesktopAgent.Test.Command
{
    [TestFixture]
    public class MouseDragTest
    {
        [Test]
        public void ShouldFireLeftButtonMouseDragEvent()
        {
            var command = new MouseDrag(60, 200, 100, 300, 1);
            command.Execute();
            Point point;
            Assert.IsTrue(User32.GetCursorPos(out point));
            Assert.AreEqual(new Point(200, 300), point);
        }

        [Test]
        public void ShouldFireRightButtonMouseDragEvent()
        {
            var command = new MouseDrag(60, 200, 100, 300, 2);
            command.Execute();
            Point point;
            Assert.IsTrue(User32.GetCursorPos(out point));
            Assert.AreEqual(new Point(200, 300), point);
        }
    }
}
