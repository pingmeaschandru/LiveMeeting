using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;

namespace TW.H264Coder.Vcl.Block
{
    public class ResidualBlockInfo
    {
        private readonly ResidualBlockType type;
        private readonly int nonZeroCoeff;
        private readonly int maxNumCoeff;

        public ResidualBlockInfo(ResidualBlockType type, int maxNumCoeff, int nonZeroCoeff)
        {
            this.type = type;
            this.maxNumCoeff = maxNumCoeff;
            this.nonZeroCoeff = nonZeroCoeff;
        }

        public MacroblockInfo MacroblockInfo { get; set; }
        public int BlockIndexX { get; set; }
        public int BlockIndexY { get; set; }

        public ResidualBlockType Type
        {
            get { return type; }
        }

        public int NonZeroCoeff
        {
            get { return nonZeroCoeff; }
        }

        public int MaxNumCoeff
        {
            get { return maxNumCoeff; }
        }
    }
}