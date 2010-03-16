using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpConnectionDataParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            message.ConnectionData = new SdpFieldConnection(GetFieldValue(reader));
        }
    }
}