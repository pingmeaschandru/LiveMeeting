using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpSessionInformationParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            message.SessionInformation = new SdpFieldSessionInformation(GetFieldValue(reader));
        }
    }
}