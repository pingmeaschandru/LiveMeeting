using System;
using System.Collections.Generic;
using System.IO;

namespace TW.LiveMeet.RDAP.Parser
{
    public class RdapMessageParser 
    {
        private readonly RdapMessageReader messageReader;
        private readonly List<Action> commands;
        private int startCommandIndex;

        private RdapMessage currentRdapMessage;
        private int currentRdapMessageDataLength;

        public RdapMessageParser(Stream messagestream)
        {
            messageReader = new RdapMessageReader(messagestream);

            commands = new List<Action>
                           {
                               ParseMessageType,
                               ParseMessageDataLength,
                               ParseMessageData
                           };

            IntialiseStateObjects();
        }

        private void IntialiseStateObjects()
        {
            startCommandIndex = 0;
            currentRdapMessage = new RdapMessage();
            currentRdapMessageDataLength = 0;
        }

        public bool TryParseMessage(out RdapMessage message)
        {
            message = null;

            if (messageReader.BaseStream.Length <= 0)
                return false;

            for (var i = startCommandIndex; i < commands.Count; i++)
            {
                try
                {
                    commands[i]();
                }
                catch (InsufficientRdapMessageException)
                {
                    startCommandIndex = i;
                    return false;
                }
            }

            message = currentRdapMessage;
            IntialiseStateObjects();

            return true;
        }

        private void ParseMessageType()
        {
            currentRdapMessage.MessageType = messageReader.ReadMessageType();
        }

        private void ParseMessageDataLength()
        {
            currentRdapMessageDataLength = Convert.ToInt32(messageReader.ReadDataLength());
        }

        private void ParseMessageData()
        {
            currentRdapMessage.Data = messageReader.ReadData(currentRdapMessageDataLength);
        }
    }
}