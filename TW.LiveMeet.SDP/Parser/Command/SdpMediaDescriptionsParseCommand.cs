using TW.Core.IO;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpMediaDescriptionsParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            var media = new SdpMediaDescriptions();
            media.AddFieldValue(GetFieldValuePair(reader));
            while (reader.Length > 0)
            {
                var type = reader.ReadPeekChar();
                if (type == 'i') media.AddFieldValue(GetFieldValuePair(reader));
                else if (type == 'c') media.AddFieldValue(GetFieldValuePair(reader));
                else if (type == 'b') media.AddFieldValue(GetFieldValuePair(reader));
                else if (type == 'k') media.AddFieldValue(GetFieldValuePair(reader));
                else if (type == 'a') media.AddFieldValue(GetFieldValuePair(reader));
                else break;
            }
            message.Medias.Add(media);
        }
    }
}