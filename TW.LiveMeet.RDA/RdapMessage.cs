using System.IO;

namespace TW.LiveMeet.RDAP
{
    public class RdapMessage
    {
        internal const int MESSAGE_TYPE_LENGTH = 1;
        internal const int DATA_LENGTH_LENGTH = 4;

        public RdapMessage(RdapMessageType messageType, byte[] data)
        {
            MessageType = messageType;
            Data = data;
        }

        public RdapMessage()
        {
        }

        public RdapMessageType MessageType { get; set; }
        public byte[] Data { get; set; }

        public byte[] ToBytes()
        {
            byte[] messageData;
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write((byte) MessageType);
                writer.Write((uint) Data.Length);
                writer.Write(Data);
                messageData = memoryStream.ToArray();
            }
            return messageData;
        }
    }
}