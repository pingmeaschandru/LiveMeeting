using System.Drawing;
using NUnit.Framework;
using TW.Core.Native;
using TW.LiveMeet.DesktopAgent.Command;

namespace TW.LiveMeet.DesktopAgent.Test.Command
{
    [TestFixture]
    public class MouseRightTest
    {
        [Test]
        public void ShouldFireMouseRightEvent()
        {
            var command = new MouseRight(6, 100);
            command.Execute();
            Point point;
            Assert.IsTrue(User32.GetCursorPos(out point));
            Assert.AreEqual(new Point(6, 100), point);
        }
    }
}
