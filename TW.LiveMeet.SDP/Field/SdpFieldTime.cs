using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldTime : ISdpFieldValue
    {
        public SdpFieldTime(string fieldValue)
        {
            Parse(fieldValue);
        }

        public SdpFieldTime()
        {
        }

        private void Parse(string fieldValue)
        {
            var originValues = fieldValue.Split(new[] { ' ' });
            if (originValues.Length != 2)
                throw new InvalidSdpStringFormatException();

            try
            {
                StartTime = ulong.Parse(originValues[0]);
                StopTime = ulong.Parse(originValues[1]);
            }
            catch
            {
                throw new InvalidSdpStringFormatException();
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(StartTime);
            stringBuilder.Append(' ');
            stringBuilder.Append(StopTime);

            return stringBuilder.ToString();
        }

        public ulong StartTime { get; set; }
        public ulong StopTime { get; set; }

        public char FieldName
        {
            get { return 't'; }
        }
    }
}