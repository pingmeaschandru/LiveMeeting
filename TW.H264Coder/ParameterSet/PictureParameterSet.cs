using TW.H264Coder.IO;
using TW.H264Coder.Nal;

namespace TW.H264Coder.ParameterSet
{
    public class PictureParameterSet : RBSP 
    {
        private readonly int picParameterSetId; // ue(v)
        private readonly int seqParameterSetId; // ue(v)
        private readonly bool entropyCodingModeFlag; // u(1)
        private readonly bool picOrderPresentFlag; // u(1)
        private readonly int numSliceGroupsMinus1; // ue(v)
        private readonly int numRefIdxL0ActiveMinus1; // ue(v)
        private readonly int numRefIdxL1ActiveMinus1; // ue(v)
        private readonly bool weightedPredFlag; // u(1)
        private readonly int weightedBipredIdc; // u(2)
        private readonly int picInitQpMinus26; // se(v)
        private readonly int picInitQsMinus26; // se(v)
        private readonly int chromaQpIndexOffset; // se(v)
        private readonly bool deblockingFilterControlPresentFlag; // u(1)
        private readonly bool constrainedIntraPredFlag; // u(1)
        private readonly bool redundantPicCntPresentFlag; // u(1)

        public PictureParameterSet(SequenceParameterSet sps) 
        {
            seqParameterSetId = sps.SeqParameterSetId;
            picParameterSetId = 0;          // TODO hard coded to zero
            entropyCodingModeFlag = false;  // TODO hard coded to Cavlc
            picOrderPresentFlag = false;    // TODO hard coded to false
            numSliceGroupsMinus1 = 0;       // TODO hard coded to zero

            // Following set the parameter for different slice group types
            if (numSliceGroupsMinus1 > 0) 
            {
            }
            // End FMO stuff

            if (sps.FrameMbsOnlyFlag) 
            {
                numRefIdxL0ActiveMinus1 = sps.NumRefFrames - 1;
                numRefIdxL1ActiveMinus1 = sps.NumRefFrames - 1;
            } 
            else 
            {
                numRefIdxL0ActiveMinus1 = 2 * sps.NumRefFrames - 1;
                numRefIdxL1ActiveMinus1 = 2 * sps.NumRefFrames - 1;
            }

            weightedPredFlag = false;       // TODO hard coded to false
            weightedBipredIdc = 0;          // TODO hard coded to zero

            // hard coded to zero, QP lives in the slice header
            picInitQpMinus26 = 0;
            picInitQsMinus26 = 0;
            chromaQpIndexOffset = 0;                    // TODO hard coded to zero
            deblockingFilterControlPresentFlag = false; // TODO hrd cd to false
            constrainedIntraPredFlag = false;           // TODO hard coded to false
            redundantPicCntPresentFlag = false;         // TODO hard coded to false
        }

        public override int Write(IH264EntropyOutputStream  stream)
        {
            stream.WriteUeV(picParameterSetId);
            stream.WriteUeV(seqParameterSetId);
            stream.WriteU1(entropyCodingModeFlag);
            stream.WriteU1(picOrderPresentFlag);
            stream.WriteUeV(numSliceGroupsMinus1);
            // FMO stuff
            if (numSliceGroupsMinus1 > 0) {
                // TODO Unreachable
            }
            // End of FMO stuff
            stream.WriteUeV(numRefIdxL0ActiveMinus1);
            stream.WriteUeV(numRefIdxL1ActiveMinus1);
            stream.WriteU1(weightedPredFlag);
            stream.WriteUv(2, weightedBipredIdc);
            stream.WriteSeV(picInitQpMinus26);
            stream.WriteSeV(picInitQsMinus26);
            stream.WriteSeV(chromaQpIndexOffset);
            stream.WriteU1(deblockingFilterControlPresentFlag);
            stream.WriteU1(constrainedIntraPredFlag);
            stream.WriteU1(redundantPicCntPresentFlag);
            // copies the last couple of bits into the byte buffer
            stream.Close();

            return (int)(stream.Length / 8);
        }

        public int PicParameterSetId
        {
            get { return picParameterSetId; }
        }

        public bool EntropyCodingModeFlag
        {
            get { return entropyCodingModeFlag; }
        }

        public int PicInitQpMinus26
        {
            get { return picInitQpMinus26; }
        }
    }
}