namespace TW.LiveMeet.RDAP.Messages
{
    public class UnknownRdaMessage : RdapMessageBase, IRdapMessage
    {
        public UnknownRdaMessage(byte[] messageBuffer)
            :base(messageBuffer)
        {
        }

        public UnknownRdaMessage()
        {
        }
    }
}