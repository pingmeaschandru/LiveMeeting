using TW.Core.IO;

namespace TW.LiveMeet.SDP.Parser
{
    internal interface ISdpParseCommand
    {
        void Execute(StringTokenizer reader,SdpMessage message);
    }
}