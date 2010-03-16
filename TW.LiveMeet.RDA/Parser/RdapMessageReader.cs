using System.IO;

namespace TW.LiveMeet.RDAP.Parser
{
    internal class RdapMessageReader
    {
        private readonly BinaryReader byteReader;

        public RdapMessageReader(Stream messageStream)
        {
            byteReader = new BinaryReader(messageStream);
        }

        public RdapMessageType ReadMessageType()
        {
            if (byteReader.BaseStream.Length < RdapMessage.MESSAGE_TYPE_LENGTH)
                throw new InsufficientRdapMessageException();

            return (RdapMessageType)byteReader.ReadByte();
        }

        public uint ReadDataLength()
        {
            if (byteReader.BaseStream.Length < RdapMessage.DATA_LENGTH_LENGTH)
                throw new InsufficientRdapMessageException();

            return byteReader.ReadUInt32();
        }

        public byte[] ReadData(int count)
        {
            if (byteReader.BaseStream.Length < count)
                throw new InsufficientRdapMessageException();

            return byteReader.ReadBytes(count);
        }

        public Stream BaseStream
        {
            get { return byteReader.BaseStream; }
        }
    }
}