namespace TW.H264Coder.Vcl.Algorithm
{
    public interface ITransform
    {
        void Forward4X4(int[][] src, int[][] dst, int posY, int posX);
        void Inverse4X4(int[][] src, int[][] dst, int posY, int posX);
        void Hadamard4X4(int[][] src, int[][] dst);
        void Ihadamard4X4(int[][] src, int[][] dst);
        void Hadamard2X2(int[][] src, int[][] dst);
        void Ihadamard2X2(int[][] src, int[][] dst);
    }
}