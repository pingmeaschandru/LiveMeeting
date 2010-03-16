using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldSessionInformation : ISdpFieldValue
    {
        public string SessionInformation { get; set; }

        public SdpFieldSessionInformation(string fieldValue)
        {
            SessionInformation = fieldValue;
        }

        public SdpFieldSessionInformation()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(SessionInformation);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'i'; }
        }
    }
}