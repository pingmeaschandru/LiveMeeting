using NUnit.Framework;
using TW.LiveMeet.RDAP.Messages;

namespace TW.LiveMeet.RDAP.Test.Messages
{
    [TestFixture]
    public class UnknownRdaMessageTest
    {
        [Test]
        public void TwoConsecutiveCallsWhichReturnsMessageBuffersShouldNotBeSame()
        {
            var messageBuffer = new byte[] { 2, 56, 0, 0, 0, 67, 0, 0, 0, 60, 0, 0, 0, 71, 0, 0, 0 };
            var message = new UnknownRdaMessage(messageBuffer);
            var messageBuffer1 = message.ToBytes();
            var messageBuffer2 = message.ToBytes();

            Assert.AreNotSame(messageBuffer1, messageBuffer2);
        }
    }
}