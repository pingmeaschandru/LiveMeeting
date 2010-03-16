using System.Drawing;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra8X8ChromaHorizontalPredictor : Intra8X8ChromaAbstractPredictor
    {
        public Intra8X8ChromaHorizontalPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mpCb, int[][] mpCr)
        {
            var maxW = Macroblock.MbChromaWidth;
            var maxH = Macroblock.MbChromaHeight;

            if (!access.IsLeftAvailable(maxH)) return false;
            var p = new Point();

            // predC[ x, y ] = p[ -1, y ],
            // with x = 0..MbWidthC - 1 and y = 0..MbHeightC - 1
            for (var i = 0; i < maxH; i++)
            {
                access.GetNeighbour(-1, i, maxW, maxH, p);
                for (var j = 0; j < maxW; j++)
                {
                    mpCb[i][j] = codedFrameBuffer.GetCb8Bit(p.X, p.Y);
                    mpCr[i][j] = codedFrameBuffer.GetCr8Bit(p.X, p.Y);
                }
            }

            return true;
        }
    }
}