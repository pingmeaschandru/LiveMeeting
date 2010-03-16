using System;
using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;

namespace TW.H264Coder.Vcl.Mode
{
    public abstract class AbstractEncodingMode : IEncodingMode
    {
        protected YuvFrameBuffer inputFrameBuffer; // raw YUV read frame
        protected YuvFrameBuffer outputFrameBuffer; // encoded frame
        protected Macroblock macroblock;
        protected MacroblockType mbType;

        protected AbstractEncodingMode(Macroblock macroblock, MacroblockType mbType)
        {
            this.macroblock = macroblock;
            this.mbType = mbType;
        }

        public void Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            inputFrameBuffer = inFrameBuffer;
            outputFrameBuffer = outFrameBuffer;
            DoEncode(inFrameBuffer, outFrameBuffer);
        }

        public int GetDistortion()
        {
            var distortion = 0;
            var x = macroblock.PixelX;
            var y = macroblock.PixelY;
            var cx = macroblock.PixelChromaX;
            var cy = macroblock.PixelChromaY;

            // LUMA
            for (var j = y; j < y + Macroblock.MbHeight; j++)
                for (var i = x; i < x + Macroblock.MbWidth; i++)
                    distortion += Convert.ToInt32(Math.Pow(inputFrameBuffer.GetY8Bit(i, j) - outputFrameBuffer.GetY8Bit(i, j), 2));

            // CHROMA
            for (var j = cy; j < cy + Macroblock.MbChromaHeight; j++)
                for (var i = cx; i < cx + Macroblock.MbChromaWidth; i++)
                {
                    distortion += Convert.ToInt32(Math.Pow(inputFrameBuffer.GetCb8Bit(i, j) - outputFrameBuffer.GetCb8Bit(i, j), 2));
                    distortion += Convert.ToInt32(Math.Pow(inputFrameBuffer.GetCr8Bit(i, j) - outputFrameBuffer.GetCr8Bit(i, j), 2));
                }

            return distortion;
        }

        public void Write(IH264EntropyOutputStream outStream)
        {
            DoWrite(outStream);
        }

        public abstract void Reconstruct(YuvFrameBuffer outFrameBuffer);
        protected abstract void DoEncode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer codedFrameBuffer);
        protected abstract void DoWrite(IH264EntropyOutputStream outStream);

        public abstract MacroblockInfo LumaMacroblockInfo { get; }

        public abstract MacroblockInfo ChromaMacroblockInfo { get; }

        public MacroblockType MbType
        {
            get { return mbType; }
        }

        public Macroblock GetMacroblock()
        {
            return macroblock;
        }
    }
}