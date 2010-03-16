using System.Drawing;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra16X16LumaVerticalPredictor : Intra16X16LumaAbstractPredictor
    {
        public Intra16X16LumaVerticalPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mp)
        {
            var maxW = Macroblock.MbWidth;
            var maxH = Macroblock.MbHeight;

            if (!access.IsUpAvailable(maxW)) return false;
            var p = new Point();

            // predL[ x, y ] = p[ x, -1 ], with x, y = 0..15
            for (var index0 = 0; index0 < maxW; index0++)
            {
                access.GetNeighbour(index0, -1, maxW, maxH, p);
                for (var index01 = 0; index01 < maxH; index01++)
                    mp[index01][index0] = codedFrameBuffer.GetY8Bit(p.X, p.Y);
            }

            return true;
        }
    }
}