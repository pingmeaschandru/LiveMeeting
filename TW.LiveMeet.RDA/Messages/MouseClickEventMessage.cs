using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class MouseClickEventMessage : RdapMessageBase, IRdapMessage
    {
        internal const int EVENT_TYPE_LENGTH = 1;
        internal const int X_POSITION_LENGTH = 4;
        internal const int Y_POSITION_LENGTH = 4;

        private int eventTypePosition;
        private int xPositionPosition;
        private int yPositionPosition;

        public MouseClickEventMessage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public MouseClickEventMessage(MouseEventType eventType, uint xPosition, uint yPosition)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write((byte) eventType);
                writer.Write(xPosition);
                writer.Write(yPosition);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }

        private void InitialiseFieldPositions()
        {
            eventTypePosition = 0;
            xPositionPosition = eventTypePosition + EVENT_TYPE_LENGTH;
            yPositionPosition = xPositionPosition + X_POSITION_LENGTH;
        }

        public MouseEventType EventType 
        {
            get { return (MouseEventType) messageBuffer[eventTypePosition]; }
        }

        public uint XPosition
        {
            get { return BitConverter.ToUInt32(messageBuffer, xPositionPosition); }
        }

        public uint YPosition
        {
            get { return BitConverter.ToUInt32(messageBuffer, yPositionPosition); }
        }
    }
}