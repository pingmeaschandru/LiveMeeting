using System.Collections.Generic;
using TW.Core.IO;
using TW.LiveMeet.SDP.Parser.Command;

namespace TW.LiveMeet.SDP.Parser
{
    public class SdpMessageParser
    {
        private readonly Dictionary<char, ISdpParseCommand> commands;

        public SdpMessageParser()
        {
            commands = new Dictionary<char, ISdpParseCommand>
                           {
                               {'a', new SdpAttributeParseCommand()},
                               {'b', new SdpBandWidthParseCommand()},
                               {'c', new SdpConnectionDataParseCommand()},
                               {'e', new SdpEmailAddressParseCommand()},
                               {'k', new SdpEncryptionKeyParseCommand()},
                               {'m', new SdpMediaDescriptionsParseCommand()},
                               {'o', new SdpOriginParseCommand()},
                               {'p', new SdpPhoneNumberParseCommand()},
                               {'i', new SdpSessionInformationParseCommand()},
                               {'s', new SdpSessionNameParseCommand()},
                               {'t', new SdpTimingsParseCommand()},
                               {'u', new SdpUriParseCommand()},
                               {'v', new SdpVersionParseCommand()}
                           };
        }

        public SdpMessage Parse(string message)
        {
            var sdpMessage = new SdpMessage();
            var stringTokenizer = new StringTokenizer(message);

            while(stringTokenizer.Length > 0)
            {
                ISdpParseCommand command;
                if (commands.TryGetValue(stringTokenizer.ReadPeekChar(), out command))
                    command.Execute(stringTokenizer, sdpMessage);
            }

            return sdpMessage;
        }
    }
}