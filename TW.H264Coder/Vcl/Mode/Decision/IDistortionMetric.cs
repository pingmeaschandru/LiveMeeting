namespace TW.H264Coder.Vcl.Mode.Decision
{
    public interface IDistortionMetric
    {
        int GetDistortion16X16(int[][] orig, int[][] pred);
        int GetDistortion4X4(int[][] orig, int[][] pred, int posY, int posX);
    }
}