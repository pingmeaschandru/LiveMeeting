using System.Collections.Generic;
using System.Text;

namespace TW.LiveMeet.SDP.Field
{
    public class SdpFieldMediaDescription : ISdpFieldValue
    {
        public SdpFieldMediaDescription(string fieldValue)
            :this()
        {
            Parse(fieldValue);
        }

        public SdpFieldMediaDescription()
        {
            MediaFmts = new List<uint>();
        }

        private void Parse(string fieldValue)
        {
            var originValues = fieldValue.Split(new[] { ' ' });
            if (originValues.Length < 4)
                throw new InvalidSdpStringFormatException();

            try
            {
                MediaType = originValues[0];
                Port = uint.Parse(originValues[1]);
                Protocol = originValues[2];

                for (var i = 3; i < originValues.Length; i++)
                    MediaFmts.Add(uint.Parse(originValues[i]));
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
            stringBuilder.Append(MediaType);
            stringBuilder.Append(' ');
            stringBuilder.Append(Port);
            stringBuilder.Append(' ');
            stringBuilder.Append(Protocol);
            stringBuilder.Append(' ');
            foreach (var fmt in MediaFmts)
            {
                stringBuilder.Append(fmt);
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString(0, stringBuilder.Length - 1);
        }

        public string MediaType { get; set; }
        public uint Port { get; set; }
        public string Protocol { get; set; }
        public List<uint> MediaFmts { get; private set; }

        public char FieldName
        {
            get { return 'm'; }
        }
    }
}