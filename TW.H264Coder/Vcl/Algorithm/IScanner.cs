namespace TW.H264Coder.Vcl.Algorithm
{
    public interface IScanner
    {
        void Reorder4X4(int[][] srcCoeff, int[] dstLevel, int[] dstRun, int startPos, int totalCoeff, int yOffset, int xOffset);
        void Reorder2X2(int[][] srcCoeff, int[] dstLevel, int[] dstRun, int startPos, int totalCoeff, int yOffset, int xOffset);
    }
}