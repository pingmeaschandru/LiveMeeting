using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldSessionName : ISdpFieldValue
    {
        public string SessionName { get; set; }

        public SdpFieldSessionName(string fieldValue)
        {
            SessionName = fieldValue;
        }

        public SdpFieldSessionName()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(SessionName);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 's'; }
        }
    }
}