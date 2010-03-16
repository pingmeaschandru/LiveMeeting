using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldAttribute : ISdpFieldValue
    {
        public SdpFieldAttribute(string fieldValue)
        {
            Parse(fieldValue);
        }

        public SdpFieldAttribute()
        {
        }

        private void Parse(string fieldValue)
        {
            var originValues = fieldValue.Split(new[] { ':' }, 2);
            if (originValues.Length == 0) throw new InvalidSdpStringFormatException();

            Name = originValues[0];

            if (originValues.Length > 1)
                Value = originValues[1];
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(Name);
            stringBuilder.Append(':');
            stringBuilder.Append(Value);

            return stringBuilder.ToString();
        }

        public string Name { get; set; }
        public string Value { get; set; }

        public char FieldName
        {
            get { return 'a'; }
        }
    }
}