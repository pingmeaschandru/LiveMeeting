using NUnit.Framework;

namespace TW.LiveMeet.RDAP.Test
{
    [TestFixture]
    public class RdapMessageTest
    {
        [Test]
        public void ShouldAbleToRetriveVariables()
        {
            var messageBuffer = new byte[]
                                    {
                                        1, 4, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                        , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 40, 0, 0,
                                        0, 50, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 40, 0, 0, 0, 50, 0, 0, 0
                                    };

            var message = new RdapMessage(RdapMessageType.DesktopWindowInfoMessage, messageBuffer);

            Assert.AreEqual(RdapMessageType.DesktopWindowInfoMessage, message.MessageType);
            Assert.AreEqual(messageBuffer.Length, message.Data.Length);

            for (var i = 0; i < messageBuffer.Length; i++)
            {
                if (messageBuffer[i] != message.Data[i])
                    Assert.Fail("Message contents is not same");
            }
        }
    }
}