using System;

namespace TW.LiveMeet.RDAP
{
    public abstract class RdapMessageBase
    {
        protected byte[] messageBuffer;

        protected RdapMessageBase(byte[] messageBuffer)
        {
            this.messageBuffer = messageBuffer;
        }

        protected RdapMessageBase()
        {
        }

        public byte[] ToBytes()
        {
            var dataToReturn = new byte[messageBuffer.Length];
            Array.Copy(messageBuffer, dataToReturn, messageBuffer.Length);

            return dataToReturn;
        }
    }
}