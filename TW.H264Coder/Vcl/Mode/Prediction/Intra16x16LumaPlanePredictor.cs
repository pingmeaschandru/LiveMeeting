using System.Drawing;
using TW.Core.Helper;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra16X16LumaPlanePredictor : Intra16X16LumaAbstractPredictor
    {
        public Intra16X16LumaPlanePredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mp)
        {
            var maxW = Macroblock.MbWidth;
            var maxH = Macroblock.MbHeight;
            var upAvail = access.IsUpAvailable(maxW);
            var leftAvail = access.IsLeftAvailable(maxH);
            var leftUpAvail = access.IsLeftUpAvailable();
            if (!upAvail || !leftAvail || !leftUpAvail) // edge
                return false;

            var h = 0;
            var v = 0;
            var pUp = new Point();
            var pLeft = new Point();
            var pLeftUp = new Point();

            access.GetNeighbour(0, -1, maxW, maxH, pUp);
            access.GetNeighbour(-1, 0, maxW, maxH, pLeft);
            access.GetNeighbour(-1, -1, maxW, maxH, pLeftUp);

            // H = sum(x' = 0 to 7){ (x'+1) * (p[8+x', -1] - p[6-x', -1]) }
            // V = sum(y' = 0 to 7){ (y'+1) * (p[-1, 8+y'] - p[-1, 6-y']) }
            for (var i = 1; i < 9; i++)
            {
                if (i < 8)
                {
                    h += i*(codedFrameBuffer.GetY8Bit(pUp.X + 7 + i, pUp.Y)- codedFrameBuffer.GetY8Bit(pUp.X + 7 - i, pUp.Y));
                    v += i*(codedFrameBuffer.GetY8Bit(pLeft.X, pLeft.Y + 7 + i)- codedFrameBuffer.GetY8Bit(pLeft.X, pLeft.Y + 8 - i));
                }
                else
                {
                    h += i*(codedFrameBuffer.GetY8Bit(pUp.X + 7 + i, pUp.Y)- codedFrameBuffer.GetY8Bit(pLeftUp.X, pLeftUp.Y));
                    v += i*(codedFrameBuffer.GetY8Bit(pLeft.X, pLeft.Y + 7 + i)- codedFrameBuffer.GetY8Bit(pLeftUp.X, pLeftUp.Y));
                }
            }

            var a = 16*(codedFrameBuffer.GetY8Bit(pLeft.X, pLeft.Y + 15) + codedFrameBuffer.GetY8Bit(pUp.X + 15, pUp.Y));
            var b = (5*h + 32) >> 6;
            var c = (5*v + 32) >> 6;

            // predL[x, y] = Clip1Y((a + b*(x - 7) + c*(y - 7) + 16) >> 5),
            // with x, y = 0..15
            for (var j = 0; j < Macroblock.MbHeight; j++)
            {
                for (var i = 0; i < Macroblock.MbWidth; i++)
                {
                    var value = MathHelper.RshiftRound((a + (i - 7)*b + (j - 7)*c), 5);
                    mp[j][i] = MathHelper.Clip(maxImagePelValue, value);
                }
            }

            return true;
        }
    }
}