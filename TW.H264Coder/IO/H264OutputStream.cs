using System;
using TW.H264Coder.Nal;
using TW.H264Coder.ParameterSet;
using TW.H264Coder.Vcl.Block;
using TW.H264Coder.Vcl.Entropy;

namespace TW.H264Coder.IO
{
    public class H264OutputStream : BitOutputStream, IH264EntropyOutputStream
    {
        private readonly SequenceParameterSet sps;
        private readonly PictureParameterSet pps;
        private readonly IEntropyOutputStream entropyOutputStream;

        public H264OutputStream(Nalu nalu, SequenceParameterSet sps,PictureParameterSet pps)
        {
            this.Nalu = nalu;
            this.sps = sps;
            this.pps = pps;

            if (pps.EntropyCodingModeFlag)
            {
                //TODO : CABAC
                // entropyOutputStream = new CABAC();
            }
            else
            {
                entropyOutputStream = new Cavlc(this);
            }
        }

        public int WriteUv(int n, int value)
        {
            return entropyOutputStream.WriteUv(n, value);
        }

        public int WriteU1(bool value)
        {
            return entropyOutputStream.WriteU1(value);
        }

        public int WriteUeV(int value)
        {
            return entropyOutputStream.WriteUeV(value);
        }

        public int WriteSeV(int value)
        {
            return entropyOutputStream.WriteSeV(value);
        }

        public int WriteMbType(int value)
        {
            return entropyOutputStream.WriteMbType(value);
        }

        public int WriteIntraChromaPredMode(int value)
        {
            return entropyOutputStream.WriteIntraChromaPredMode(value);
        }

        public int WriteMbQpDelta(int value)
        {
            return entropyOutputStream.WriteMbQpDelta(value);
        }

        public int WriteResidualBlock(int[] coeffLevel, int[] coeffRun, ResidualBlockInfo residualBlock)
        {
            return entropyOutputStream.WriteResidualBlock(coeffLevel, coeffRun, residualBlock);
        }

        public Nalu Nalu { get; set; }

        public SequenceParameterSet Sps
        {
            get { return sps; }
        }

        public PictureParameterSet Pps
        {
            get { return pps; }
        }
    }
}