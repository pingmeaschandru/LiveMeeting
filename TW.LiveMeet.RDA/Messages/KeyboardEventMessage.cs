using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class KeyboardEventMessage : RdapMessageBase, IRdapMessage
    {
        internal const int KEY_STROKE_LENGTH = 1;
        internal const int EXTRA_KEYS_LENGTH = 1;

        private int keyStrokePosition;
        private int extraKeyStrokePosition;

        public KeyboardEventMessage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public KeyboardEventMessage(byte keyStroke, byte extraKeyStroke)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write(keyStroke);
                writer.Write(extraKeyStroke);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }


        private void InitialiseFieldPositions()
        {
            keyStrokePosition = 0;
            extraKeyStrokePosition = keyStrokePosition + KEY_STROKE_LENGTH;
        }    
        
        public byte KeyStroke
        {
            get { return messageBuffer[keyStrokePosition]; }
        }

        public byte ExtraKeyStroke
        {
            get { return messageBuffer[extraKeyStrokePosition]; }
        }


        //public byte VirtualKeyCode { get; set; }
        //public byte ScanCode { get; set; }
        //public bool KeyDown { get; set; }
        //public bool ExtendedKey { get; set; }
    }
}