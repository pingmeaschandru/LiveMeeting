using System.Drawing;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra8X8ChromaDCPredictor : Intra8X8ChromaAbstractPredictor
    {
        private const int ChromaBlkWidth = 4;
        private const int ChromaBlkHeight = 4;
        private const int ChromaBlkQuantity = 2;

        public Intra8X8ChromaDCPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mpCb, int[][] mpCr)
        {
            var p = new Point();
            var maxW = Macroblock.MbChromaWidth;
            var maxH = Macroblock.MbChromaHeight;
            var upAvail = access.IsUpAvailable(maxW);
            var leftAvail = access.IsLeftAvailable(maxH);

            // For each chroma block of 4x4 samples indexed by
            // chroma4x4BlkIdx = 0..( 1 << ( ChromaArrayType + 1 ) ) – 1
            for (var chroma4X4BlkIdx = 0; chroma4X4BlkIdx < 4; chroma4X4BlkIdx++)
            {
                var predCb = 0;
                var predCr = 0;
                var sumUpCb = 0;
                var sumUpCr = 0;
                var sumLeftCb = 0;
                var sumLeftCr = 0;

                var xO = PosX(chroma4X4BlkIdx);
                var yO = PosY(chroma4X4BlkIdx);

                // sum(x' = 0 to 3) { p[x' + xO, -1] }
                if (upAvail)
                {
                    for (var i = 0; i < ChromaBlkWidth; i++)
                    {
                        access.GetNeighbour(i, -1, maxW, maxH, p);
                        sumUpCb += codedFrameBuffer.GetCb8Bit(p.X + xO, p.Y);
                        sumUpCr += codedFrameBuffer.GetCr8Bit(p.X + xO, p.Y);
                    }
                }
                // sum(y' = 0 to 3) { p[-1, y' + yO] }
                if (leftAvail)
                {
                    for (var i = 0; i < ChromaBlkHeight; i++)
                    {
                        access.GetNeighbour(-1, i, maxW, maxH, p);
                        sumLeftCb += codedFrameBuffer.GetCb8Bit(p.X, p.Y + yO);
                        sumLeftCr += codedFrameBuffer.GetCr8Bit(p.X, p.Y + yO);
                    }
                }

                // TOP-LEFT and BOTTOM-RIGHT
                if (((xO == 0) && (yO == 0)) || ((xO > 0) && (yO > 0)))
                {
                    if (upAvail && leftAvail)
                    {
                        // predC[x+xO, y+yO] = (sumUp + sumLeft + 4) >> 3
                        predCb = (sumUpCb + sumLeftCb + 4) >> 3;
                        predCr = (sumUpCr + sumLeftCr + 4) >> 3;
                    }
                    else if (leftAvail)
                    {
                        // predC[x+xO, y+yO] = (sumLeft + 2) >> 2
                        predCb = (sumLeftCb + 2) >> 2;
                        predCr = (sumLeftCr + 2) >> 2;
                    }
                    else if (upAvail)
                    {
                        // predC[x+xO, y+yO] = (sumUp + 2) >> 2
                        predCb = (sumUpCb + 2) >> 2;
                        predCr = (sumUpCr + 2) >> 2;
                    }
                    else
                    {
                        // predC[x+xO, y+yO] = (1 << ( BitDepthC – 1 ))
                        predCb = 1 << (bitDepthC - 1);
                        predCr = 1 << (bitDepthC - 1);
                    }
                } // TOP-RIGHT
                else if ((xO > 0) && (yO == 0))
                {
                    if (upAvail)
                    {
                        // predC[x+xO, y+yO] = (sumUp + 2) >> 2
                        predCb = (sumUpCb + 2) >> 2;
                        predCr = (sumUpCr + 2) >> 2;
                    }
                    else if (leftAvail)
                    {
                        // predC[x+xO, y+yO] = (sumLeft + 2) >> 2
                        predCb = (sumLeftCb + 2) >> 2;
                        predCr = (sumLeftCr + 2) >> 2;
                    }
                    else
                    {
                        // predC[x+xO, y+yO] = (1 << ( BitDepthC – 1 ))
                        predCb = 1 << (bitDepthC - 1);
                        predCr = 1 << (bitDepthC - 1);
                    }
                } // BOTTOM-LEFT
                else if ((xO == 0) && (yO > 0))
                {
                    if (leftAvail)
                    {
                        // predC[x+xO, y+yO] = (sumLeft + 2) >> 2
                        predCb = (sumLeftCb + 2) >> 2;
                        predCr = (sumLeftCr + 2) >> 2;
                    }
                    else if (upAvail)
                    {
                        // predC[x+xO, y+yO] = (sumUp + 2) >> 2
                        predCb = (sumUpCb + 2) >> 2;
                        predCr = (sumUpCr + 2) >> 2;
                    }
                    else
                    {
                        // predC[x+xO, y+yO] = (1 << ( BitDepthC – 1 ))
                        predCb = 1 << (bitDepthC - 1);
                        predCr = 1 << (bitDepthC - 1);
                    }
                }

                // store DC prediction
                for (var j = yO; j < ChromaBlkHeight + yO; j++)
                {
                    for (var i = xO; i < ChromaBlkWidth + xO; i++)
                    {
                        mpCb[j][i] = predCb;
                        mpCr[j][i] = predCr;
                    }
                }
            }

            return true;
        }

        private static int PosX(int blkIdx)
        {
            return (blkIdx % ChromaBlkQuantity) * ChromaBlkWidth;
        }

        private static int PosY(int blkIdx)
        {
            return (blkIdx / ChromaBlkQuantity) * ChromaBlkHeight;
        }
    }
}