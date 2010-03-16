using System.Drawing;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra16X16LumaHorizontalPredictor : Intra16X16LumaAbstractPredictor
    {
        public Intra16X16LumaHorizontalPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mp)
        {
            var maxW = Macroblock.MbWidth;
            var maxH = Macroblock.MbHeight;

            if (!access.IsLeftAvailable(maxH)) return false;

            var p = new Point();

            // predL[ x, y ] = p[ -1, y ], with x, y = 0..15
            for (var i = 0; i < maxH; i++)
            {
                access.GetNeighbour(-1, i, maxW, maxH, p);
                for (var j = 0; j < maxW; j++)
                    mp[i][j] = codedFrameBuffer.GetY8Bit(p.X, p.Y);
            }

            return true;
        }
    }
}