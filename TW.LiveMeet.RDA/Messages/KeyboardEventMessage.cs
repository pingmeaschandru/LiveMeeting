using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class KeyboardEventMessage : RdapMessageBase, IRdapMessage
    {
        internal const int VIRTUAL_KEY_CODE_LENGTH = 2;
        internal const int ALT_LENGTH = 1;
        internal const int CONTROL_LENGTH = 1;
        internal const int SHIFT_LENGTH = 1;

        private int virtualKeyCodePosition;
        private int altPosition;
        private int controlPosition;
        private int shiftPosition;

        public KeyboardEventMessage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public KeyboardEventMessage(ushort virtualKeyCode, byte alt, byte control, byte shift)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write(virtualKeyCode);
                writer.Write(alt);
                writer.Write(control);
                writer.Write(shift);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }


        private void InitialiseFieldPositions()
        {
            virtualKeyCodePosition = 0;
            altPosition = virtualKeyCodePosition + VIRTUAL_KEY_CODE_LENGTH;
            controlPosition = altPosition + ALT_LENGTH;
            shiftPosition = controlPosition + CONTROL_LENGTH;
        }    
        
        public ushort VirtualKeyCode
        {
            get { return BitConverter.ToUInt16(messageBuffer, virtualKeyCodePosition); }
        }

        public byte Alt
        {
            get { return messageBuffer[altPosition]; }
        }

        public byte Control
        {
            get { return messageBuffer[controlPosition]; }
        }

        public byte Shift
        {
            get { return messageBuffer[shiftPosition]; }
        }
    }
}