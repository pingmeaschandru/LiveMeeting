using System.Collections.Generic;
using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;
using TW.H264Coder.Vcl.Mode;

namespace TW.H264Coder.Vcl
{
    public class Macroblock
    {
        public static readonly int MbWidth = 16;
        public static readonly int MbHeight = 16;
        public static readonly int MbChromaWidth = 8;
        public static readonly int MbChromaHeight = 8;

        protected int mbNr;
        protected Slice slice;
        protected MacroblockPosition position;
        protected MacroblockAccess access;
        protected int deltaQP;

        private readonly List<IEncodingMode> encodingModes;
        private IEncodingMode bestMode;

        public Macroblock(Slice slice, int mbNr)
        {
            this.mbNr = mbNr;
            this.slice = slice;
            position = new MacroblockPosition(this);
            access = new MacroblockAccessNonMbaff(this);
            encodingModes = new List<IEncodingMode> {new Intra16X16EncodingMode(this)};
            StartMacroblock();
        }

        private void StartMacroblock()
        {
            access.CheckAvailableNeighbours();
        }

        public void SetMacroblockParameters()
        {
        }

        public void Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            const int bestDistortion = int.MaxValue;
            foreach (var mode in encodingModes)
            {
                mode.Encode(inFrameBuffer, outFrameBuffer);
                if (mode.GetDistortion() < bestDistortion)
                    bestMode = mode;
            }
            bestMode.Reconstruct(outFrameBuffer);
        }

        public void Write(IH264EntropyOutputStream outStream)
        {
            bestMode.Write(outStream);
        }

        public Slice Slice
        {
            get { return slice; }
        }

        public MacroblockType MbType
        {
            get { return bestMode.MbType; }
        }

        public int PixelX
        {
            get { return position.PixelX; }
        }

        public int PixelY
        {
            get { return position.PixelY; }
        }

        public int PixelChromaX
        {
            get { return position.PixelChromaX; }
        }

        public int PixelChromaY
        {
            get { return position.PixelChromaY; }
        }

        public int MbNr
        {
            get { return mbNr; }
        }

        public MacroblockAccess MacroblockAccess
        {
            get { return access; }
        }

        public MacroblockInfo MacroblockInfo
        {
            get { return bestMode.LumaMacroblockInfo; }
        }
    }
}