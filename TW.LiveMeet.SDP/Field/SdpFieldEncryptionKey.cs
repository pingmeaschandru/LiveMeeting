using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldEncryptionKey : ISdpFieldValue
    {
        public SdpFieldEncryptionKey(string fieldValue)
        {
            EncryptionKey = fieldValue;
        }

        public SdpFieldEncryptionKey()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(EncryptionKey);

            return stringBuilder.ToString();
        }

        public string EncryptionKey { get; set; }

        public char FieldName
        {
            get { return 'k'; }
        }
    }
}