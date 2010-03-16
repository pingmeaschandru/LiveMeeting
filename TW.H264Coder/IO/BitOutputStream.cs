using System;
using System.IO;
namespace TW.H264Coder.IO
{
    public class BitOutputStream
    {
        private readonly MemoryStream byteStream;
        private int byteBuf;
        private int bitsToGo;
        private bool closed;

        public BitOutputStream()
        {
            byteStream = new MemoryStream();
            bitsToGo = 8;
            byteBuf = 0;
            closed = false;
        }

        public void Flush()
        {
            if (bitsToGo >= 8) return;
            byteBuf <<= bitsToGo; // Note: removing this Line breaks the code!
            byteStream.WriteByte((byte)byteBuf);
            bitsToGo = 8;
            byteBuf = 0;
        }

        public void Close()
        {
            if (closed) return;
            byteBuf <<= 1;
            byteBuf |= 1;
            bitsToGo--;
            byteBuf <<= bitsToGo;
            byteStream.WriteByte((byte)byteBuf);
            bitsToGo = 8;
            byteBuf = 0;

            closed = true;
        }

        public void WriteByte(int b, int len)
        {
            var mask = 1 << (len - 1);
            if (len > 32)
                throw new Exception("cannot Write more than 32 bits per Write");

            for (var i = 0; i < len; i++)
            {
                byteBuf <<= 1;

                if ((b & mask) > 0)
                    byteBuf |= 1;

                bitsToGo--;
                mask >>= 1;

                if (bitsToGo != 0) continue;
                byteStream.WriteByte((byte)byteBuf);
                bitsToGo = 8;
                byteBuf = 0;
            }

            closed = false;
        }

        public long Length
        {
            get
            {
                var bitsWritten = byteStream.Length * 8;
                var bitsNotWritten = 8 - bitsToGo;

                return (bitsWritten + bitsNotWritten);
            }
        }

        public byte[] ToArray()
        {
            return byteStream.ToArray();
        }
    }
}