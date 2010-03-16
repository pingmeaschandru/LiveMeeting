using NUnit.Framework;
using TW.LiveMeet.DesktopAgent.Command;

namespace TW.LiveMeet.DesktopAgent.Test.Command
{
    [TestFixture]
    public class KeyboardStrokeTest
    {
        [Test]
        public void ShouldFireLeftButtonMouseDragEvent()
        {
            var command = new KeyboardStroke('c');
            command.Execute();
        }
    }
}
