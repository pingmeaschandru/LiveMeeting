using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldEmailAddress : ISdpFieldValue
    {
        public SdpFieldEmailAddress(string fieldValue)
        {
            EmailAddress = fieldValue;
        }

        public SdpFieldEmailAddress()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(EmailAddress);

            return stringBuilder.ToString();
        }

        public string EmailAddress { get; set; }

        public char FieldName
        {
            get { return 'e'; }
        }
    }
}