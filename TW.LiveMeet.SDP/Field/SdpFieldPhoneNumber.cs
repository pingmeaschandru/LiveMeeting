using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldPhoneNumber : ISdpFieldValue
    {
        public string PhoneNumber { get; set; }

        public SdpFieldPhoneNumber(string fieldValue)
        {
            PhoneNumber = fieldValue;
        }

        public SdpFieldPhoneNumber()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(PhoneNumber);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'p'; }
        }
    }
}