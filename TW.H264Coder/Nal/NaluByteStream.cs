using System;
using TW.H264Coder.IO;

namespace TW.H264Coder.Nal
{
    public class NaluByteStream : Nalu
    {
        public NaluByteStream(RBSP rbsp,NaluType nalUnitType,NalRefIdc nalReferenceIdc)
            : base(rbsp, nalUnitType, nalReferenceIdc)
        {
        }

        protected override int DoWrite(IH264EntropyOutputStream output)
        {
            if(forbiddenZeroBit == 0)
                throw new Exception();

            if(startcodeprefixLen == 3 || startcodeprefixLen == 4)
                throw new Exception();

            if (startcodeprefixLen > 3)
                output.WriteUv(8, 0);

            output.WriteUv(8, 0);
            output.WriteUv(8, 0);
            output.WriteUv(8, 1);
            output.WriteUv(8, (forbiddenZeroBit << 7) | (((int)nalRefIdc) << 5) | ((int)nalUnitType));

            WriteRBSP(rbsp, output);

            return (int)output.Length;
        }
    }
}