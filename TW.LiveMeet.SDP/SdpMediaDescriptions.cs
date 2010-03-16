using System.Collections.Generic;
using System.Text;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP
{
    public class SdpMediaDescriptions
    {
        public SdpMediaDescriptions()
        {
            Attributes = new List<SdpFieldAttribute>();
        }

        public void AddFieldValue(string field)
        {
            var fieldValue = SdpConstants.GetValue(field);

            if (field.StartsWith("m")) MediaDescription = new SdpFieldMediaDescription(fieldValue);
            else if (field.StartsWith("i")) SessInformation = new SdpFieldSessionInformation(fieldValue);
            else if (field.StartsWith("c")) ConnectionData = new SdpFieldConnection(fieldValue);
            else if (field.StartsWith("b")) BandWidth = new SdpFieldBandwidth(fieldValue);
            else if (field.StartsWith("k")) EncryptionKey = new SdpFieldEncryptionKey(fieldValue);
            else if (field.StartsWith("a")) Attributes.Add(new SdpFieldAttribute(fieldValue));
            else throw new InvalidSdpStringFormatException();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var attribute in Attributes)
            {
                stringBuilder.Append(attribute.ToString());
                stringBuilder.Append("\r\n");
            }

            return stringBuilder.ToString(0, stringBuilder.Length - 2);
        }

        public SdpFieldMediaDescription MediaDescription { get; private set; }
        public SdpFieldSessionInformation SessInformation { get; private set; }
        public SdpFieldConnection ConnectionData { get; private set; }
        public SdpFieldBandwidth BandWidth { get; private set; }
        public SdpFieldEncryptionKey EncryptionKey { get; private set; }
        public List<SdpFieldAttribute> Attributes { get; private set; }
    }
}