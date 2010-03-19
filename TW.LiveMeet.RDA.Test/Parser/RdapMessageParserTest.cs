using NUnit.Framework;
using TW.Core.IO;
using TW.LiveMeet.RDAP.Parser;

namespace TW.LiveMeet.RDAP.Test.Parser
{
    [TestFixture]
    public class RdapMessageParserTest
    {
        [Test]
        public void ShouldAbleToReadRDAMessageFromStream()
        {
            var fifoStream = new FifoStream();

            var desktopMessage = new RdapMessage(
                RdapMessageType.DesktopWindowImageFrameMessage,
                new byte[]
                    {
                        1, 4, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 0, 0, 0
                        , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 10, 0, 0, 0,
                        10, 0, 0,
                        0, 40, 0, 0,
                        0, 50, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 40, 0, 0, 0,
                        50, 0, 0,
                        0
                    }
                );

            var mouseClickEventMessage = new RdapMessage(
                RdapMessageType.MouseClickEventMessage,
                new byte[] { 2, 56, 0, 0, 0, 67, 0, 0, 0 }
                );

            var bytesToWrite1 = desktopMessage.ToBytes();
            var bytesToWrite2 = mouseClickEventMessage.ToBytes();

            fifoStream.Write(bytesToWrite1, 0, bytesToWrite1.Length);
            fifoStream.Write(bytesToWrite2, 0, bytesToWrite2.Length);

            var parser = new RdapMessageParser(fifoStream);

            RdapMessage desktopMessageOutput;
            parser.TryParseMessage(out desktopMessageOutput);
            Assert.AreEqual(RdapMessageType.DesktopWindowImageFrameMessage, desktopMessageOutput.MessageType);
            Assert.AreEqual(97, desktopMessageOutput.Data.Length);

            RdapMessage mouseClickEventMessageOutput;
            parser.TryParseMessage(out mouseClickEventMessageOutput);
            Assert.AreEqual(RdapMessageType.MouseClickEventMessage, mouseClickEventMessageOutput.MessageType);
            Assert.AreEqual(9, mouseClickEventMessageOutput.Data.Length);
        }

        [Test]
        public void RDAMessageParserShouldReturnFalseIfNoMessageToBuild()
        {
            var fifoStream = new FifoStream();

            var desktopMessage = new RdapMessage(
                RdapMessageType.DesktopWindowImageFrameMessage,
                new byte[]
                    {
                        1, 4, 0, 0, 0, 4, 0, 0, 0, 48, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 0, 0, 0
                        , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0,
                        0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 10, 0, 0, 0,
                        10, 0, 0,
                        0, 40, 0, 0,
                        0, 50, 0, 0, 0, 10, 0, 0, 0, 10, 0, 0, 0, 40, 0, 0, 0,
                        50, 0, 0,
                        0
                    }
                );

            var mouseClickEventMessage = new RdapMessage(
                RdapMessageType.MouseClickEventMessage,
                new byte[] { 2, 56, 0, 0, 0, 67, 0, 0, 0 }
                );

            var bytesToWrite1 = desktopMessage.ToBytes();
            var bytesToWrite2 = mouseClickEventMessage.ToBytes();

            var parser = new RdapMessageParser(fifoStream);

            fifoStream.Write(bytesToWrite1, 0, bytesToWrite1.Length - 10);

            RdapMessage desktopMessageOutput;
            var retVal = parser.TryParseMessage(out desktopMessageOutput);
            Assert.AreEqual(false, retVal);
            Assert.AreEqual(null, desktopMessageOutput);

            fifoStream.Write(bytesToWrite1, bytesToWrite1.Length - 10, 10);
            fifoStream.Write(bytesToWrite2, 0, bytesToWrite2.Length);

            parser.TryParseMessage(out desktopMessageOutput);
            Assert.AreEqual(RdapMessageType.DesktopWindowImageFrameMessage, desktopMessageOutput.MessageType);
            Assert.AreEqual(97, desktopMessageOutput.Data.Length);

            RdapMessage mouseClickEventMessageOutput;
            parser.TryParseMessage(out mouseClickEventMessageOutput);
            Assert.AreEqual(RdapMessageType.MouseClickEventMessage, mouseClickEventMessageOutput.MessageType);
            Assert.AreEqual(9, mouseClickEventMessageOutput.Data.Length);
        }
    }
}