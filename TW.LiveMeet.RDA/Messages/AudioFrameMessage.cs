using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class AudioFrameMessage : RdapMessageBase, IRdapMessage
    {
        internal const int FORMAT_TYPE_LENGTH = 1;
        internal const int IMAGE_BYTE_LENGTH_LENGTH = 4;

        private int formatTypePosition;
        private int audioByteLengthPosition;
        private int audioBytePosition;

        public AudioFrameMessage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public AudioFrameMessage(AudioFormatType formatType, byte[] imageBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write((byte)formatType);
                writer.Write((uint)imageBytes.Length);
                writer.Write(imageBytes);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }

        private void InitialiseFieldPositions()
        {
            formatTypePosition = 0;
            audioByteLengthPosition = formatTypePosition + FORMAT_TYPE_LENGTH;
            audioBytePosition = audioByteLengthPosition + IMAGE_BYTE_LENGTH_LENGTH;
        }

        public AudioFormatType FormatType
        {
            get
            {
                return (AudioFormatType)messageBuffer[formatTypePosition];
            }
        }

        private uint AudioBytesLength
        {
            get
            {
                return BitConverter.ToUInt32(messageBuffer, audioByteLengthPosition);
            }
        }

        public byte[] AudioBytes
        {
            get
            {
                var data = new byte[AudioBytesLength];
                Array.Copy(messageBuffer, audioBytePosition, data, 0, data.Length);
                return data;
            }
        }
    }
}