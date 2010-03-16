using TW.Core.IO;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpTimingsParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            var time = new SdpTimings();
            time.AddFieldValue(GetFieldValuePair(reader));
            while (reader.Length > 0)
            {
                var type = reader.ReadPeekChar();
                if (type == 't') time.AddFieldValue(GetFieldValuePair(reader));
                else if (type == 'z') time.AddFieldValue(GetFieldValuePair(reader));
                else break;
            }
            message.Times.Add(time);
        }
    }
}