using TW.H264Coder.IO;

namespace TW.H264Coder.Nal
{
    public abstract class Nalu 
    {
        protected int startcodeprefixLen;
        protected NaluType nalUnitType;     
        protected NalRefIdc nalRefIdc;      
        protected byte forbiddenZeroBit;   
        protected RBSP rbsp;               
        protected bool useAnnexbLongStartcode;

        protected Nalu(RBSP rbsp, NaluType nalUnitType, NalRefIdc nalRefIdc) 
        {
            startcodeprefixLen = useAnnexbLongStartcode ? 4 : 3;
            this.nalUnitType = nalUnitType;
            this.nalRefIdc = nalRefIdc;
            forbiddenZeroBit = 0;
            this.rbsp = rbsp;
            useAnnexbLongStartcode = true;
        }

        public int Write(IH264EntropyOutputStream output)
        {
            output.Nalu = this;
            return DoWrite(output);
        }

        protected abstract int DoWrite(IH264EntropyOutputStream output);

        protected int WriteRBSP(RBSP rbspObj, IH264EntropyOutputStream output) 
        {
            var rbspSize = rbspObj.Write(output);
            if (rbspSize < RBSP.Maxrbspsize) 
            {
                // TODO throw an exception. PayloadOverloadedException?
            }

            return 1 + PreventEmulationOfStartCode(output, 0);
        }

        protected int PreventEmulationOfStartCode(IH264EntropyOutputStream output, int beginBytePos)
        {
            var endBytePos = output.Length / 8;
            var j = beginBytePos;
            for (var i = beginBytePos; i < endBytePos; i++) 
            {
                // TODO must implement, but I didn't find any file that need this
                // emulation byte
                j++;
            }

            return j;
        }

        public NaluType NalUnitType
        {
            get { return nalUnitType; }
        }

        public NalRefIdc NalRefIdc
        {
            get { return nalRefIdc; }
        }
    }
}