using System.Drawing;
using TW.Core.Helper;
using TW.H264Coder.IO;
using TW.H264Coder.Nal;

namespace TW.H264Coder.ParameterSet
{
    public class SequenceParameterSet : RBSP
    {
        //private readonly H264Format format;
        private readonly int profileIdc; // u(8)
        private readonly bool constrainedSet0Flag; // u(1)
        private readonly bool constrainedSet1Flag; // u(1)
        private readonly bool constrainedSet2Flag; // u(1)
        private readonly bool constrainedSet3Flag; // u(1)
        private readonly int levelIdc; // u(8)
        private readonly int seqParameterSetId; // ue(v)
        private readonly int log2MaxFrameNumMinus4; // ue(v)
        private readonly int picOrderCntType;
        private readonly int log2MaxPicOrderCntLsbMinus4; // ue(v)
        private readonly int numRefFrames; // ue(v)
        private readonly bool gapsInFrameNumValueAllowedFlag; // u(1)
        private readonly int picWidthInMbsMinus1; // ue(v)
        private readonly int picHeightInMapUnitsMinus1; // ue(v)
        private readonly bool frameMbsOnlyFlag; // u(1)
        private readonly bool direct_8X8InferenceFlag; // u(1)
        private readonly bool frameCroppingFlag; // u(1)
        private readonly bool vuiParametersPresentFlag; // u(1)

        public SequenceParameterSet(int maxFrameNum, Size frameSize)//(H264Format format, int maxFrameNum)
        {
            //this.format = format;

            profileIdc = 66;
            levelIdc = 10;
            constrainedSet0Flag = false;
            constrainedSet1Flag = false;
            constrainedSet2Flag = false;
            constrainedSet3Flag = false;
            seqParameterSetId = 0;

            var log2MaxFrameNum = MathHelper.Log2(maxFrameNum);
            log2MaxFrameNumMinus4 = MathHelper.Clip(0, 12, log2MaxFrameNum - 4);

            var maxPicOrderCntLsb = 2*maxFrameNum;
            var log2MaxPicOrderCntLsb = MathHelper.Log2(maxPicOrderCntLsb);
            log2MaxPicOrderCntLsbMinus4 = MathHelper.Clip(0, 12, log2MaxPicOrderCntLsb - 4);

            picOrderCntType = 0;
            numRefFrames = 4;
            gapsInFrameNumValueAllowedFlag = false;
            frameMbsOnlyFlag = true;
            var frameMbsOnlyFlagInt = frameMbsOnlyFlag ? 1 : 0;

            picWidthInMbsMinus1 = ((frameSize.Width) / 16) - 1;
            picHeightInMapUnitsMinus1 = (((frameSize.Height) / 16) / (2 - frameMbsOnlyFlagInt)) - 1;

            direct_8X8InferenceFlag = true;
            vuiParametersPresentFlag = false;
            if (vuiParametersPresentFlag)
            {
                // TODO Unreachable
            }

            frameCroppingFlag = false;
        }

        public override int Write(IH264EntropyOutputStream stream)
        {
            stream.WriteUv(8, profileIdc);
            stream.WriteU1(constrainedSet0Flag);
            stream.WriteU1(constrainedSet1Flag);
            stream.WriteU1(constrainedSet2Flag);
            stream.WriteU1(constrainedSet3Flag);
            stream.WriteUv(4, 0);
            stream.WriteUv(8, levelIdc);
            stream.WriteUeV(seqParameterSetId);
            stream.WriteUeV(log2MaxFrameNumMinus4);
            stream.WriteUeV(picOrderCntType);
            if (picOrderCntType == 0)
            {
                stream.WriteUeV(log2MaxPicOrderCntLsbMinus4);
            }
            else if (picOrderCntType == 1)
            {
                // TODO Unreachable
            }
            stream.WriteUeV(numRefFrames);
            stream.WriteU1(gapsInFrameNumValueAllowedFlag);
            stream.WriteUeV(picWidthInMbsMinus1);
            stream.WriteUeV(picHeightInMapUnitsMinus1);
            stream.WriteU1(frameMbsOnlyFlag);
            if (!frameMbsOnlyFlag)
            {
                // TODO Unreachable
            }
            stream.WriteU1(direct_8X8InferenceFlag);
            stream.WriteU1(frameCroppingFlag);
            if (frameCroppingFlag)
            {
                // TODO Unreachable
            }
            stream.WriteU1(vuiParametersPresentFlag);
            if (vuiParametersPresentFlag)
            {
                // TODO Unreachable
            }
            stream.Close();

            return (int)(stream.Length/8);
        }

        //public H264Format Format
        //{
        //    get { return format; }
        //}

        public int SeqParameterSetId
        {
            get { return seqParameterSetId; }
        }

        public int Log2MaxFrameNumMinus4
        {
            get { return log2MaxFrameNumMinus4; }
        }

        public int PicOrderCntType
        {
            get { return picOrderCntType; }
        }

        public int Log2MaxPicOrderCntLsbMinus4
        {
            get { return log2MaxPicOrderCntLsbMinus4; }
        }

        public int NumRefFrames
        {
            get { return numRefFrames; }
        }

        public bool FrameMbsOnlyFlag
        {
            get { return frameMbsOnlyFlag; }
        }
    }
}