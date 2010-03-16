using TW.H264Coder.IO;

namespace TW.H264Coder.Nal
{
    public abstract class RBSP
    {
        public static readonly int Maxrbspsize = 64000;
        public abstract int Write(IH264EntropyOutputStream bitstream);
    }
}