using System.Collections.Generic;
using TW.H264Coder.IO;
using TW.H264Coder.Nal;
using TW.H264Coder.Vcl.Datatype;

namespace TW.H264Coder.Vcl
{
    public class Slice : RBSP
    {
        private readonly Picture picture;
        private readonly SliceType sliceType;
        private SliceHeader header;
        private readonly List<Macroblock> macroblocks;
        //private int sliceNr;

        public Slice(Picture picture, SliceType sliceType)
        {
            this.picture = picture;
            macroblocks = new List<Macroblock>();
            this.sliceType = sliceType;
        }

        private void InitSlice()
        {
            // TODO initLists()
        }

        private void SetLagrangianMultipliers()
        {
        }

        public int Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            const int numberOfCodedMBs = 99;
            InitSlice();
            SetLagrangianMultipliers();
            for (var i = 0; i < numberOfCodedMBs; i++)
            {
                var mb = new Macroblock(this, i);
                mb.Encode(inFrameBuffer, outFrameBuffer);
                macroblocks.Add(mb);
            }

            return numberOfCodedMBs;
        }

        private int StartSlice(IH264EntropyOutputStream bitstream)
        {
            header = new SliceHeader(this,bitstream.Nalu,bitstream.Sps,bitstream.Pps);
            return header.Write(bitstream);
        }


        public override int Write(IH264EntropyOutputStream outStream)
        {
            var len = 0;
            len += StartSlice(outStream);
            foreach (var mb in macroblocks)
                mb.Write(outStream);

            TerminateSlice(outStream);
            return len;
        }

        private static void TerminateSlice(IH264EntropyOutputStream outStream)
        {
            outStream.Close();
        }

        public Picture Picture
        {
            get { return picture; }
        }

        public SliceType SliceType
        {
            get { return sliceType; }
        }

        //public int SliceNr
        //{
        //    get { return sliceNr; }
        //}

        public List<Macroblock> Macroblocks
        {
            get { return macroblocks; }
        }

        public int Qp
        {
            get { return picture.Qp; }
        }
    }
}