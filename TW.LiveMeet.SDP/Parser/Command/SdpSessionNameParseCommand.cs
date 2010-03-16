using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpSessionNameParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            message.SessionName = new SdpFieldSessionName(GetFieldValue(reader));
        }
    }
}