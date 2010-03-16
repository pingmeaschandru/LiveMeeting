using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class MouseDragEventMesssage : RdapMessageBase, IRdapMessage
    {
        internal const int EVENT_TYPE_LENGTH = 1;
        internal const int START_X_POSITION_LENGTH = 4;
        internal const int START_Y_POSITION_LENGTH = 4;
        internal const int END_X_POSITION_LENGTH = 4;
        internal const int END_Y_POSITION_LENGTH = 4;

        private int eventTypePosition;
        private int xStartPositionPosition;
        private int yStartPositionPosition;
        private int xEndPositionPosition;
        private int yEndPositionPosition;

        public MouseDragEventMesssage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public MouseDragEventMesssage(MouseEventType eventType, uint xStartPosition, uint yStartPosition, uint xEndPosition, uint yEndPosition)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write((byte) eventType);
                writer.Write(xStartPosition);
                writer.Write(yStartPosition);
                writer.Write(xEndPosition);
                writer.Write(yEndPosition);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }

        private void InitialiseFieldPositions()
        {
            eventTypePosition = 0;
            xStartPositionPosition = eventTypePosition + EVENT_TYPE_LENGTH;
            yStartPositionPosition = xStartPositionPosition + START_X_POSITION_LENGTH;
            xEndPositionPosition = yStartPositionPosition + START_Y_POSITION_LENGTH;
            yEndPositionPosition = xEndPositionPosition + END_X_POSITION_LENGTH;
        }

        public MouseEventType EventType
        {
            get { return (MouseEventType)messageBuffer[eventTypePosition]; }
        }

        public uint StartXPosition 
        { 
            get { return BitConverter.ToUInt32(messageBuffer, xStartPositionPosition); }
        }

        public uint StartYPosition 
        {
            get { return BitConverter.ToUInt32(messageBuffer, yStartPositionPosition); }
        }

        public uint EndXPosition 
        {
            get { return BitConverter.ToUInt32(messageBuffer, xEndPositionPosition); }
        }

        public uint EndYPosition 
        {
            get { return BitConverter.ToUInt32(messageBuffer, yEndPositionPosition); }
        }
    }
}