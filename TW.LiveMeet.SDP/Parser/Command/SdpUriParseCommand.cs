using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpUriParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            message.Uri = new SdpFieldUri(GetFieldValue(reader));
        }
    }
}