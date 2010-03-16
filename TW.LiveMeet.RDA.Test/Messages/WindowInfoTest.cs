using NUnit.Framework;
using TW.LiveMeet.RDAP.Messages;

namespace TW.LiveMeet.RDAP.Test.Messages
{
    [TestFixture]
    public class WindowInfoTest 
    {
        [Test]
        public void ShouldAbleToRetriveVariablesFromBytes()
        {
            var messageBuffer = new byte[] { 56, 0, 0, 0, 67, 0, 0, 0, 60, 0, 0, 0, 71, 0, 0, 0 };

            var message = new WindowInfo(messageBuffer);
            Assert.AreEqual(56, message.Left);
            Assert.AreEqual(67, message.Right);
            Assert.AreEqual(60, message.Top);
            Assert.AreEqual(71, message.Bottom);
        }

        [Test]
        public void ShouldAbleToBytesFromRetriveVariables()
        {
            var message = new WindowInfo(56, 67, 60, 71);
            var messageBuffer = new byte[] { 56, 0, 0, 0, 67, 0, 0, 0, 60, 0, 0, 0, 71, 0, 0, 0 };
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
            var message = new WindowInfo(56, 67, 60, 71);
            var messageBuffer1 = message.ToBytes();
            var messageBuffer2 = message.ToBytes();

            Assert.AreNotSame(messageBuffer1, messageBuffer2);
        }
    }
}