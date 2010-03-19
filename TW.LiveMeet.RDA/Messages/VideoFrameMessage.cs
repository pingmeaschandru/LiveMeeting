using System;
using System.IO;

namespace TW.LiveMeet.RDAP.Messages
{
    public class VideoFrameMessage : RdapMessageBase, IRdapMessage
    {
        internal const int FORMAT_TYPE_LENGTH = 1;
        internal const int WIDTH_LENGTH = 4;
        internal const int HEIGHT_LENGTH = 4;
        internal const int IMAGE_BYTE_LENGTH_LENGTH = 4;

        private int formatTypePosition;
        private int widthPosition;
        private int heightPosition;
        private int imageByteLengthPosition;
        private int imageBytePosition;

        public VideoFrameMessage(byte[] messageBuffer)
            : base(messageBuffer)
        {
            InitialiseFieldPositions();
        }

        public VideoFrameMessage(RdapImagePixelFormatType formatType, uint width, uint height, byte[] imageBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                var writer = new BinaryWriter(memoryStream);
                writer.Write((byte)formatType);
                writer.Write(width);
                writer.Write(height);
                writer.Write((uint)imageBytes.Length);
                writer.Write(imageBytes);

                messageBuffer = memoryStream.ToArray();
            }

            InitialiseFieldPositions();
        }

        private void InitialiseFieldPositions()
        {
            formatTypePosition = 0;
            widthPosition = formatTypePosition + FORMAT_TYPE_LENGTH;
            heightPosition = widthPosition + WIDTH_LENGTH;
            imageByteLengthPosition = heightPosition + HEIGHT_LENGTH;
            imageBytePosition = imageByteLengthPosition + IMAGE_BYTE_LENGTH_LENGTH;
        }

        public RdapImagePixelFormatType FormatType
        {
            get
            {
                return (RdapImagePixelFormatType)messageBuffer[formatTypePosition];
            }
        }

        public uint Width
        {
            get
            {
                return BitConverter.ToUInt32(messageBuffer, widthPosition);
            }
        }

        public uint Height
        {
            get
            {
                return BitConverter.ToUInt32(messageBuffer, heightPosition);
            }
        }

        private uint ImageBytesLength
        {
            get
            {
                return BitConverter.ToUInt32(messageBuffer, imageByteLengthPosition);
            }
        }

        public byte[] ImageBytes
        {
            get
            {
                var data = new byte[ImageBytesLength];
                Array.Copy(messageBuffer, imageBytePosition, data, 0, data.Length);
                return data;
            }
        }
    }
}