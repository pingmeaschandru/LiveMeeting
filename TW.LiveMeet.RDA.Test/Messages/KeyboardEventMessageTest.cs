using NUnit.Framework;
using TW.LiveMeet.RDAP.Messages;

namespace TW.LiveMeet.RDAP.Test.Messages
{
    [TestFixture]
    public class KeyboardEventMessageTest
    {
        [Test]
        public void ShouldAbleToRetriveVariablesFromBytes()
        {
            var messageBuffer = new byte[] {1, 4};
            var message = new KeyboardEventMessage(messageBuffer);
            Assert.AreEqual(1, message.KeyStroke);
            Assert.AreEqual(4, message.ExtraKeyStroke);
        }

        [Test]
        public void ShouldAbleToBytesFromRetriveVariables()
        {
            var message = new KeyboardEventMessage(1, 4);
            var messageBuffer = new byte[] {1, 4};
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
            var message = new KeyboardEventMessage(1, 4);

            var messageBuffer1 = message.ToBytes();
            var messageBuffer2 = message.ToBytes();

            Assert.AreNotSame(messageBuffer1, messageBuffer2);
        }

    }
}