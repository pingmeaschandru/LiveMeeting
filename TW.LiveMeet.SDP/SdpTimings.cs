using System.Collections.Generic;
using System.Text;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP
{
    public class SdpTimings
    {
        public SdpTimings()
        {
            RepeatTimes = new List<SdpFieldRepeatTime>();
        }

        public void AddFieldValue(string field)
        {
            var fieldValue = SdpConstants.GetValue(field);

            if (field.StartsWith("t")) Timing = new SdpFieldTime(fieldValue);
            else if (field.StartsWith("r")) RepeatTimes.Add(new SdpFieldRepeatTime(fieldValue));
            else if (field.StartsWith("z")) ZoneAdjustment = new SdpFieldZoneAdjustments(fieldValue);
            else throw new InvalidSdpStringFormatException();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var time in RepeatTimes)
            {
                stringBuilder.Append(time.ToString());
                stringBuilder.Append("\r\n");
            }

            return stringBuilder.ToString(0, stringBuilder.Length - 2);
        }

        public SdpFieldTime Timing { get; private set; }
        public List<SdpFieldRepeatTime> RepeatTimes { get; private set; }
        public SdpFieldZoneAdjustments ZoneAdjustment { get; private set; }
    }
}