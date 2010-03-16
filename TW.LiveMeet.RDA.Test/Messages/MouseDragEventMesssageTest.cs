using NUnit.Framework;
using TW.LiveMeet.RDAP.Messages;

namespace TW.LiveMeet.RDAP.Test.Messages
{
    [TestFixture]
    public class MouseDragEventMesssageTest
    {
        [Test]
        public void ShouldAbleToRetriveVariablesFromBytes()
        {
            var messageBuffer = new byte[] { 2, 56, 0, 0, 0, 67, 0, 0, 0, 60, 0, 0, 0, 71, 0, 0, 0 };

            var message = new MouseDragEventMesssage(messageBuffer);
            Assert.AreEqual(MouseEventType.Right, message.EventType);
            Assert.AreEqual(56, message.StartXPosition);
            Assert.AreEqual(67, message.StartYPosition);
            Assert.AreEqual(60, message.EndXPosition);
            Assert.AreEqual(71, message.EndYPosition);
        }

        [Test]
        public void ShouldAbleToBytesFromRetriveVariables()
        {
            var message = new MouseDragEventMesssage(MouseEventType.Right, 56, 67, 60, 71);
            var messageBuffer = new byte[] { 2, 56, 0, 0, 0, 67, 0, 0, 0, 60, 0, 0, 0, 71, 0, 0, 0 };
            var messageBufferToCompare = message.ToBytes();

            Assert.AreEqual(messageBuffer.Length, messageBufferToCompare.Length);

            for (var i = 0; i < messageBuffer.Length; i++)
            {
                if (messageBuffer[i] != messageBufferToCompare[i])
                    Assert.Fail("Message contents is not same");
            }
        }

        [Test]
        public void TwoConsecutiveCallsWhichReturnsMessageBuffersShouldNotBeSame()
        {
            var message = new MouseDragEventMesssage(MouseEventType.Right, 56, 67, 60, 71);
            var messageBuffer1 = message.ToBytes();
            var messageBuffer2 = message.ToBytes();

            Assert.AreNotSame(messageBuffer1, messageBuffer2);
        }
    }
}