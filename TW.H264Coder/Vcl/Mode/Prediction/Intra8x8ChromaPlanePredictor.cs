using System.Drawing;
using TW.Core.Helper;
using TW.H264Coder.Vcl.Algorithm;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public class Intra8X8ChromaPlanePredictor : Intra8X8ChromaAbstractPredictor
    {
        public Intra8X8ChromaPlanePredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
            : base(x, y, macroblock, algorithms)
        {
        }

        protected override bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mpCb, int[][] mpCr)
        {
            var maxW = Macroblock.MbChromaWidth;
            var maxH = Macroblock.MbChromaHeight;
            var upAvail = access.IsUpAvailable(maxW);
            var leftAvail = access.IsLeftAvailable(maxH);
            var leftUpAvail = access.IsLeftUpAvailable();
            if (!upAvail || !leftAvail || !leftUpAvail)
                return false;

            // Note: used (chroma_block_width >> 1) instead of (xCF + 4)   
            // Note: used (chroma_block_height >> 1) instead of (yCF + 4)   

            var hCb = 0;
            var vCb = 0;
            var hCr = 0;
            var vCr = 0;
            var pUp = new Point();
            var pLeft = new Point();
            var pLeftUp = new Point();

            access.GetNeighbour(0, -1, maxW, maxH, pUp);
            access.GetNeighbour(-1, 0, maxW, maxH, pLeft);
            access.GetNeighbour(-1, -1, maxW, maxH, pLeftUp);

            // H = sum(x' = 0 to (3 + xCF)){ (x'+1) * (p[4 + xCF + x', -1] - p[2 + xCF -x', -1]) }
            for (var i = 1; i <= (maxW >> 1); i++)
            {
                var x = i - 1; // x'
                hCb += i*(codedFrameBuffer.GetCb8Bit(pUp.X + (maxW >> 1) + x, pUp.Y) - codedFrameBuffer.GetCb8Bit(pUp.X + (maxW >> 1) - 2 - x, pUp.Y));
                hCr += i*(codedFrameBuffer.GetCr8Bit(pUp.X + (maxW >> 1) + x, pUp.Y) - codedFrameBuffer.GetCr8Bit(pUp.X + (maxW >> 1) - 2 - x, pUp.Y));
            }
            // V = sum(y' = 0 to (3 + yCF)){ (y'+1) * (p[-1, 4 + yCF + y'] - p[-1, 2 + yCF - y']) }
            for (var i = 1; i <= (maxH >> 1); i++)
            {
                var y = i - 1; // y'
                vCb += i*(codedFrameBuffer.GetCb8Bit(pLeft.X, pLeft.Y + (maxH >> 1) + y) - codedFrameBuffer.GetCb8Bit(pLeft.X, pLeft.Y + (maxH >> 1) - 2 - y));
                vCr += i*(codedFrameBuffer.GetCr8Bit(pLeft.X, pLeft.Y + (maxH >> 1) + y) - codedFrameBuffer.GetCr8Bit(pLeft.X, pLeft.Y + (maxH >> 1) - 2 - y));
            }
            // a = 16 * ( p[ -1, MbHeightC - 1 ] + p[ MbWidthC - 1, -1 ] )
            var aCb = 16*(codedFrameBuffer.GetCb8Bit(pLeft.X, pLeft.Y + maxH - 1) + codedFrameBuffer.GetCb8Bit(pUp.X + maxH - 1, pUp.Y));
            var aCr = 16*(codedFrameBuffer.GetCr8Bit(pLeft.X, pLeft.Y + maxH - 1) + codedFrameBuffer.GetCr8Bit(pUp.X + maxH - 1, pUp.Y));

            // b = ( ( 34 – 29 * ( ChromaArrayType == 3 ) ) * H + 32 ) >> 6
            var bCb = ((maxW == 8 ? 17 : 5)*hCb + 2*maxW) >> (maxW == 8 ? 5 : 6);
            var bCr = ((maxW == 8 ? 17 : 5)*hCr + 2*maxW) >> (maxW == 8 ? 5 : 6);

            // c = ( ( 34 – 29 * ( ChromaArrayType != 1 ) ) * V + 32 ) >> 6
            var cCb = ((maxH == 8 ? 17 : 5)*vCb + 2*maxH) >> (maxH == 8 ? 5 : 6);
            var cCr = ((maxH == 8 ? 17 : 5)*vCr + 2*maxH) >> (maxH == 8 ? 5 : 6);

            // predC[ x, y ] = Clip1C( ( a + b * ( x – 3 – xCF ) + c * ( y – 3 – yCF ) + 16 ) >> 5 ),
            // with x = 0..MbWidthC - 1 and y = 0..MbHeightC - 1
            for (var j = 0; j < maxH; j++)
            {
                for (var i = 0; i < maxW; i++)
                {
                    var predCb = (aCb + (i - (maxW >> 1) + 1)*bCb + (j - (maxH >> 1) + 1)*cCb + 16) >> 5;
                    var predCr = (aCr + (i - (maxW >> 1) + 1)*bCr + (j - (maxH >> 1) + 1)*cCr + 16) >> 5;
                    mpCb[j][i] = MathHelper.Clip(maxImagePelValue, predCb);
                    mpCr[j][i] = MathHelper.Clip(maxImagePelValue, predCr);
                }
            }
            return true;
        }
    }
}