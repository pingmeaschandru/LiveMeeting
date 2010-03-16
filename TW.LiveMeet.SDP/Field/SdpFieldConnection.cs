using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldConnection : ISdpFieldValue
    {
        public SdpFieldConnection(string fieldValue)
        {
            Parse(fieldValue);
        }

        public SdpFieldConnection()
        {
        }

        private void Parse(string fieldValue)
        {
            var originValues = fieldValue.Split(new[] { ' ' });
            if (originValues.Length != 3) throw new InvalidSdpStringFormatException();

            NetType = originValues[0];
            AddressType = originValues[1];
            ConnectionAddress = originValues[2];
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(FieldName);
            stringBuilder.Append('=');
            stringBuilder.Append(NetType);
            stringBuilder.Append(' ');
            stringBuilder.Append(AddressType);
            stringBuilder.Append(' ');
            stringBuilder.Append(ConnectionAddress);

            return stringBuilder.ToString();
        }

        public string NetType { get; set; }
        public string AddressType { get; set; }
        public string ConnectionAddress { get; set; }

        public char FieldName
        {
            get { return 'c'; }
        }
    }
}