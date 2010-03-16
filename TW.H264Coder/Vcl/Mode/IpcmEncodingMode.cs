using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;

namespace TW.H264Coder.Vcl.Mode
{
    public class IpcmEncodingMode : AbstractEncodingMode
    {
        private readonly int[][] bufferY;
        private readonly int[][] bufferU;
        private readonly int[][] bufferV;

        public IpcmEncodingMode(Macroblock macroblock)
            : base(macroblock, MacroblockType.Ipcm)
        {
            bufferY = new int[Macroblock.MbWidth][];
            for (var i = 0; i < bufferY.Length; i++)
                bufferY[i] = new int[Macroblock.MbHeight];

            bufferU = new int[Macroblock.MbChromaWidth][];
            for (var i = 0; i < bufferU.Length; i++)
                bufferU[i] = new int[Macroblock.MbChromaHeight];

            bufferV = new int[Macroblock.MbChromaWidth][];
            for (var i = 0; i < bufferV.Length; i++)
                bufferV[i] = new int[Macroblock.MbChromaHeight];
        }

        public override void Reconstruct(YuvFrameBuffer outFrameBuffer)
        {
            var x = macroblock.PixelX;
            var y = macroblock.PixelY;
            var cx = macroblock.PixelChromaX;
            var cy = macroblock.PixelChromaY;

            for (var j = 0; j < Macroblock.MbHeight; j++)
                for (var i = 0; i < Macroblock.MbWidth; i++)
                    outFrameBuffer.SetY8Bit(i + x, j + y, bufferY[i][j]);

            for (var j = 0; j < Macroblock.MbChromaHeight; j++)
                for (var i = 0; i < Macroblock.MbChromaWidth; i++)
                    outFrameBuffer.SetCb8Bit(i + cx, j + cy, bufferU[i][j]);

            for (var j = 0; j < Macroblock.MbChromaHeight; j++)
                for (var i = 0; i < Macroblock.MbChromaWidth; i++)
                    outFrameBuffer.SetCr8Bit(i + cx, j + cy, bufferV[i][j]);
        }

        protected override void DoEncode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer codedFrameBuffer)
        {
            var x = macroblock.PixelX;
            var y = macroblock.PixelY;
            var cx = macroblock.PixelChromaX;
            var cy = macroblock.PixelChromaY;

            // LUMA
            for (var j = 0; j < Macroblock.MbHeight; j++)
                for (var i = 0; i < Macroblock.MbWidth; i++)
                    bufferY[i][j] = inputFrameBuffer.GetY8Bit(i + x, j + y);

            // CHROMA
            for (var j = 0; j < Macroblock.MbChromaHeight; j++)
                for (var i = 0; i < Macroblock.MbChromaWidth; i++)
                {
                    bufferU[i][j] = inputFrameBuffer.GetCb8Bit(i + cx, j + cy);
                    bufferV[i][j] = inputFrameBuffer.GetCr8Bit(i + cx, j + cy);
                }
        }

        protected override void DoWrite(IH264EntropyOutputStream outStream)
        {
            const int bitDepth = 8;

            if (macroblock.Slice.SliceType.Equals(SliceType.Slice))
                outStream.WriteMbType(25);
            else
                outStream.WriteMbType(31);

            outStream.Flush();

            for (var j = 0; j < Macroblock.MbHeight; j++)
                for (var i = 0; i < Macroblock.MbWidth; i++)
                    outStream.WriteUv(bitDepth, bufferY[i][j]);

            for (var j = 0; j < Macroblock.MbChromaHeight; j++)
                for (var i = 0; i < Macroblock.MbChromaWidth; i++)
                    outStream.WriteUv(bitDepth, bufferU[i][j]);

            for (var j = 0; j < Macroblock.MbChromaHeight; j++)
                for (var i = 0; i < Macroblock.MbChromaWidth; i++)
                    outStream.WriteUv(bitDepth, bufferV[i][j]);
        }

        public override MacroblockInfo LumaMacroblockInfo
        {
            get { return null; }
        }

        public override MacroblockInfo ChromaMacroblockInfo
        {
            get { return null; }
        }
    }
}