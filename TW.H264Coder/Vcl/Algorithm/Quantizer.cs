namespace TW.H264Coder.Vcl.Algorithm
{
    public abstract class Quantizer
    {
        protected int qp;

        protected Quantizer(int qp)
        {
            this.qp = qp;
        }

        public abstract int Quantization4X4DC(int[][] src, int[][] dst);
        public abstract void Iquantization4X4DC(int[][] src, int[][] dst);
        public abstract int Quantization2X2DC(int[][] src, int[][] dst);
        public abstract void Iquantization2X2DC(int[][] src, int[][] dst);
        public abstract int Quantization4X4AC(int[][] src, int[][] dst, int posY, int posX);
        public abstract void Iquantization4X4AC(int[][] src, int[][] dst, int posY, int posX);
    }
}