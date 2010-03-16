using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldVersion : ISdpFieldValue
    {
        public string Version { get; set; }

        public SdpFieldVersion(string fieldValue)
        {
            Version = fieldValue;
        }

        public SdpFieldVersion()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(Version);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'v'; }
        }
    }
}