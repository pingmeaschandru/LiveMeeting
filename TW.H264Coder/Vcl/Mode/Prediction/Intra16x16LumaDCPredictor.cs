using System;
using System.Drawing;
using TW.Core.Helper;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra16X16LumaDCPredictor : Intra16X16LumaAbstractPredictor
    {
        public Intra16X16LumaDCPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mp)
        {
            var p = new Point();
            var sumUp = 0;
            var sumLeft = 0;
            var maxW = Macroblock.MbWidth;
            var maxH = Macroblock.MbHeight;
            var upAvail = access.IsUpAvailable(maxW);
            var leftAvail = access.IsLeftAvailable(maxH);

            // sum(x' = 0 to 15) { p[x', -1] }
            if (upAvail)
            {
                for (var i = 0; i < Macroblock.MbWidth; i++)
                {
                    access.GetNeighbour(i, -1, maxW, maxH, p);
                    sumUp += codedFrameBuffer.GetY8Bit(p.X, p.Y);
                }
            }
            // sum(y' = 0 to 15) { p[-1, y'] }
            if (leftAvail)
            {
                for (var i = 0; i < Macroblock.MbHeight; i++)
                {
                    access.GetNeighbour(-1, i, maxW, maxH, p);
                    sumLeft += codedFrameBuffer.GetY8Bit(p.X, p.Y);
                }
            }
                
            // no edge
            // predL[x, y] = (sumUp + sumLeft + 16) >> 5 , with x, y = 0..15
            // upper edge
            // predL[x, y] = (sumLeft + 8) >> 4, with x, y = 0..15          
            // left edge
            // predL[x, y] = (sumUp + 8) >> 4, with x, y = 0..15
            // top left corner
            // predL[x, y] = (1 << (BitDepthY – 1)), with x, y = 0..15

            var predL = upAvail && leftAvail
                            ? MathHelper.RshiftRound((sumUp + sumLeft), 5)
                            : (!upAvail && leftAvail
                                   ? MathHelper.RshiftRound(sumLeft, 4)
                                   : (upAvail ? MathHelper.RshiftRound(sumUp, 4) : 1 << (bitDepthY - 1)));

            // store DC prediction
            for (var j = 0; j < Macroblock.MbHeight; j++)
                for (var i = 0; i < Macroblock.MbWidth; i++)
                    mp[j][i] = predL;

            return true;
        }
    }
}