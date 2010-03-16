using System.Drawing;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra8X8ChromaVerticalPredictor : Intra8X8ChromaAbstractPredictor
    {
        public Intra8X8ChromaVerticalPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mpCb, int[][] mpCr)
        {
            var maxW = Macroblock.MbChromaWidth;
            var maxH = Macroblock.MbChromaHeight;

            if (!access.IsUpAvailable(maxW)) return false;
            var p = new Point();

            // predC[ x, y ] = p[ x, -1 ],
            // with x = 0..MbWidthC - 1 and y = 0..MbHeightC - 1
            for (var i = 0; i < maxW; i++)
            {
                access.GetNeighbour(i, -1, maxW, maxH, p);
                for (var j = 0; j < maxH; j++)
                {
                    mpCb[j][i] = codedFrameBuffer.GetCb8Bit(p.X, p.Y);
                    mpCr[j][i] = codedFrameBuffer.GetCr8Bit(p.X, p.Y);
                }
            }

            return true;
        }

    }
}