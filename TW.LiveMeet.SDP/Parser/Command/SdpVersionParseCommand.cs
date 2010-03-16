using System;
using TW.Core.IO;
using TW.LiveMeet.SDP.Field;

namespace TW.LiveMeet.SDP.Parser.Command
{
    internal class SdpVersionParseCommand : SdpParseCommandBase, ISdpParseCommand
    {
        public void Execute(StringTokenizer reader, SdpMessage message)
        {
            message.Version = new SdpFieldVersion(GetFieldValue(reader));
        }
    }
}