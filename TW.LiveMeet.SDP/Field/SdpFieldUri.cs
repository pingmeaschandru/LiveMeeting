using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldUri : ISdpFieldValue
    {
        public string URI { get; set; }

        public SdpFieldUri(string fieldValue)
        {
            URI = fieldValue;
        }

        public SdpFieldUri()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(URI);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'u'; }
        }
    }
}