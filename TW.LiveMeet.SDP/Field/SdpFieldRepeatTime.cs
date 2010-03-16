using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldRepeatTime : ISdpFieldValue
    {
        public string RepeatTime { get; set; }

        public SdpFieldRepeatTime(string fieldValue)
        {
            RepeatTime = fieldValue;
        }

        public SdpFieldRepeatTime()
        {
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(RepeatTime);

            return stringBuilder.ToString();
        }

        public char FieldName
        {
            get { return 'r'; }
        }
    }
}