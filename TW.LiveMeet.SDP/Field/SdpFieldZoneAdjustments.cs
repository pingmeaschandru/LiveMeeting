using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldZoneAdjustments : ISdpFieldValue
    {
        public string ZoneAdjustments { get; set; }

        public SdpFieldZoneAdjustments(string fieldValue)
        {
            ZoneAdjustments = fieldValue;
        }

        public SdpFieldZoneAdjustments()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(ZoneAdjustments);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'z'; }
        }
    }
}