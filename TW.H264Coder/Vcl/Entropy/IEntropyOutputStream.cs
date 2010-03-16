using TW.H264Coder.Vcl.Block;

namespace TW.H264Coder.Vcl.Entropy
{
    public interface IEntropyOutputStream
    {
        int WriteUv(int n, int value);
        int WriteU1(bool value);
        int WriteUeV(int value);
        int WriteSeV(int value);
        int WriteMbType(int value);
        int WriteMbQpDelta(int value);
        int WriteIntraChromaPredMode(int value);
        int WriteResidualBlock(int[] coeffLevel, int[] coeffRun, ResidualBlockInfo residualBlock);
    }
}