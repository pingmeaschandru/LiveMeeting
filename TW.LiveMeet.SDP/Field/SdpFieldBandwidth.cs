using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldBandwidth : ISdpFieldValue
    {
        public string BandwidthInfo { get; set; }

        public SdpFieldBandwidth(string fieldValue)
        {
            BandwidthInfo = fieldValue;
        }

        public SdpFieldBandwidth()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(BandwidthInfo);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'b'; }
        }
    }
}