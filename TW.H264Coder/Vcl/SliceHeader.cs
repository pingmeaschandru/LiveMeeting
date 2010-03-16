using System;
using TW.H264Coder.IO;
using TW.H264Coder.Nal;
using TW.H264Coder.ParameterSet;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.Entropy;

namespace TW.H264Coder.Vcl
{
    public class SliceHeader
    {
        private readonly Slice slice;
        private readonly int picParameterSetId;
        private readonly int log2MaxFrameNumMinus4;
        private readonly bool frameMbsOnlyFlag;
        private readonly int picOrderCntType;
        private readonly int log2MaxPicOrderCntLsbMinus4;
        private readonly int picInitQpMinus26;
        private readonly NalRefIdc nalRefIdc;
        private readonly NaluType nalUnitType;
        private readonly int firstMbInSlice;
        private readonly SliceType sliceType;
        private int picOrderCntLsb;
        private readonly bool noOutputOfPriorPicsFlag;
        private readonly bool longTermReferenceFlag;
        private readonly int sliceQpDelta;

        public SliceHeader(Slice slice, Nalu nalu, SequenceParameterSet sps, PictureParameterSet pps)
        {
            this.slice = slice;

            // BEGIN: parameters from Nalu, Picture Par Set and Sequence Par Set
            picParameterSetId = pps.PicParameterSetId;
            log2MaxFrameNumMinus4 = sps.Log2MaxFrameNumMinus4;
            frameMbsOnlyFlag = sps.FrameMbsOnlyFlag;
            picOrderCntType = sps.PicOrderCntType;
            log2MaxPicOrderCntLsbMinus4 = sps.Log2MaxPicOrderCntLsbMinus4;
            picInitQpMinus26 = pps.PicInitQpMinus26;
            nalRefIdc = nalu.NalRefIdc;
            nalUnitType = nalu.NalUnitType;
            // END: parameters from Nalu, Picture Par Set and Sequence Par Set

            sliceType = slice.Picture.TypeOfSlice;
            firstMbInSlice = slice.Picture.CurrentMbNr;
            noOutputOfPriorPicsFlag = false;
            longTermReferenceFlag = false;
            sliceQpDelta = (slice.Qp - 26 - picInitQpMinus26);
        }


        public int Write(IH264EntropyOutputStream stream)
        {
            var len = 0;

            len += stream.WriteUeV(firstMbInSlice);
            len += stream.WriteUeV((int)sliceType);
            len += stream.WriteUeV(picParameterSetId);
            len += stream.WriteUv(
                log2MaxFrameNumMinus4 + 4, slice.Picture.FrameNum);

            if (nalUnitType.Equals(NaluType.NaluTypeIdr))
            {
                // idr_pic_id
                len += stream.WriteUeV(slice.Picture.Number%2);
            }

            if (picOrderCntType == 0)
            {
                if (frameMbsOnlyFlag)
                {
                    const int frameSkip = 0;
                    var baseMul = slice.Picture.FrameNum;
                    var toppoc = baseMul*(2*(frameSkip + 1));
                    var maxLength = ~((-1) << (log2MaxPicOrderCntLsbMinus4 + 4));
                    picOrderCntLsb = toppoc & maxLength;
                }
                else
                {
                    // TODO Unreachable
                }
                len += stream.WriteUv(
                    log2MaxPicOrderCntLsbMinus4 + 4, picOrderCntLsb);
            }

            len += RefPicListReordering(stream);

            if (((int)nalRefIdc) != 0)
                len += DecRefPicMarking(stream);

            len += stream.WriteSeV(sliceQpDelta);

            return len;
        }

        private static int RefPicListReordering(IH264EntropyOutputStream stream)
        {
            return 0; // TODO Unreachable
        }

        private int DecRefPicMarking(IEntropyOutputStream stream)
        {
            var len = 0;

            if (slice.Picture.IsIDR)
            {
                len += stream.WriteU1(noOutputOfPriorPicsFlag);
                len += stream.WriteU1(longTermReferenceFlag);
            }
            else
            {
                const bool adaptiveRefPicBufferingFlag = false;
                len += stream.WriteU1(adaptiveRefPicBufferingFlag);

                if (adaptiveRefPicBufferingFlag)
                {
                    // TODO Unreachable
                }
            }
            return len;
        }
    }
}