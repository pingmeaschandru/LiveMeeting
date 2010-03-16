using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class WindowInfo : RdapMessageBase
    {
        internal const int LEFT_LENGTH = 4;
        internal const int RIGHT_LENGTH = 4;
        internal const int TOP_LENGTH = 4;
        internal const int BOTTOM_LENGTH = 4;

        private int leftPosition;
        private int rightPosition;
        private int topPosition;
        private int bottomPosition;

        public WindowInfo(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public WindowInfo(uint left, uint right, uint top, uint bottom)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write(left);
                writer.Write(right);
                writer.Write(top);
                writer.Write(bottom);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }

        private void InitialiseFieldPositions()
        {
            leftPosition = 0;
            rightPosition = leftPosition + LEFT_LENGTH;
            topPosition = rightPosition + RIGHT_LENGTH;
            bottomPosition = topPosition + TOP_LENGTH;
        }

        public uint Left
        {
            get { return BitConverter.ToUInt32(messageBuffer, leftPosition); }
        }
        
        public uint Right
        {
            get { return BitConverter.ToUInt32(messageBuffer, rightPosition); }
        }

        public uint Top
        {
            get { return BitConverter.ToUInt32(messageBuffer, topPosition); }
        }

        public uint Bottom
        {
            get { return BitConverter.ToUInt32(messageBuffer, bottomPosition); }
        }
    }
}