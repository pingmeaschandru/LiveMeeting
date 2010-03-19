using NUnit.Framework;
using TW.LiveMeet.RDAP.Messages;

namespace TW.LiveMeet.RDAP.Test.Messages
{
    [TestFixture]
    public class DesktopWindowInfoMessageTest
    {
        [Test]
        public void ShouldAbleToRetriveVariablesFromBytes()
        {
            var messageBuffer = new byte[]
                                    {
                                        1, 4, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                        , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                    };

            var message = new DesktopWindowImageFrameMessage(messageBuffer);
            Assert.AreEqual(RdapImagePixelFormatType.PIX_FMT_YUV420, message.FormatType);
            Assert.AreEqual(4, message.Width);
            Assert.AreEqual(4, message.Height);
            Assert.AreEqual(48, message.ImageBytes.Length);
        }

        [Test]
        public void ShouldAbleToBytesFromRetriveVariables()
        {

            var message = new DesktopWindowImageFrameMessage(RdapImagePixelFormatType.PIX_FMT_YUV420, 4, 4,
                                                       new byte[4 * 4 * 3]);

            var messageBuffer = new byte[]
                                    {
                                        1, 4, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                        , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                    };

            var messageBufferToCompare = message.ToBytes();

            Assert.AreEqual(messageBuffer.Length, messageBufferToCompare.Length);

            for (var i = 0; i < messageBuffer.Length; i++)
            {
                if(messageBuffer[i] != messageBufferToCompare[i])
                    Assert.Fail("Message contents is not same");
            }
        }

        [Test]
        public void TwoConsecutiveCallsWhichReturnsMessageBuffersShouldNotBeSame()
        {

            var message = new DesktopWindowImageFrameMessage(RdapImagePixelFormatType.PIX_FMT_YUV420, 4, 4,
                                                       new byte[4 * 4 * 3]);


            var messageBuffer1 = message.ToBytes();
            var messageBuffer2 = message.ToBytes();

            Assert.AreNotSame(messageBuffer1, messageBuffer2);
        }
    }
}