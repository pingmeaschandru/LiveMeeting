using System;
using TW.Core.Helper;
using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Algorithm;
using TW.H264Coder.Vcl.Block;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;
using TW.H264Coder.Vcl.Mode.Decision;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public abstract class Intra16X16LumaAbstractPredictor : IIntraPredictor
    {
        protected YuvFrameBuffer inputFrameBuffer;
        protected YuvFrameBuffer outputFrameBuffer;
        protected int x; 
        protected int y;
        protected int mbWidth = Macroblock.MbWidth;
        protected int mbHeight = Macroblock.MbHeight;
        protected int dcWidth = 4;
        protected int dcHeight = 4;
        protected int[][] mOrig;
        protected int[][] mResd; 
        protected int[][] mPred;
        protected int[][] mDc;
        protected MacroblockAccess access;
        protected MacroblockInfo info;
        protected ITransform transform;
        protected Quantizer quantizer;
        protected IDistortionMetric distortion;
        protected IScanner scanner;
        protected int maxImagePelValue = 255; // TODO centralize this value
        protected int bitDepthY = 8; // TODO 8-bit fixed centralize this value

        protected Intra16X16LumaAbstractPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
        {
            this.x = x;
            this.y = y;
            access = macroblock.MacroblockAccess;
            info = new MacroblockInfo(macroblock);
            transform = algorithms.CreateTransform();
            quantizer = algorithms.CreateQuantizer();
            distortion = algorithms.CreateDistortionMetric();
            scanner = algorithms.CreateScanner();
            mOrig = new int[mbHeight][];
            for (var i = 0; i < mOrig.Length; i++)
                mOrig[i] = new int[mbWidth];

            mResd = new int[mbHeight][];
            for (var i = 0; i < mResd.Length; i++)
                mResd[i] = new int[mbWidth];

            mPred = new int[mbHeight][];
            for (var i = 0; i < mPred.Length; i++)
                mPred[i] = new int[mbWidth];

            mDc = new int[dcHeight][];
            for (var i = 0; i < mDc.Length; i++)
                mDc[i] = new int[dcWidth];
        }

        public bool Predict(YuvFrameBuffer origiFrameBuffer, YuvFrameBuffer codedFrameBuffer)
        {
            FillOriginalMatrix(origiFrameBuffer);
            return DoIntraPrediction(codedFrameBuffer, mPred);
        }

        public int Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            inputFrameBuffer = inFrameBuffer;
            outputFrameBuffer = outFrameBuffer;
            FillResidualMatrix();
            return ForwardTransform(mResd, mDc, mResd);
        }

        public void Reconstruct(YuvFrameBuffer outFrameBuffer)
        {
            const int dqBits = 6;
            var mrInv = new int[mbHeight][];
            for (var i = 0; i < mrInv.Length; i++)
                mrInv[i] = new int[mbWidth];

            InverseTransform(mDc, mResd, mrInv);

            for (var j = 0; j < mbHeight; j++)
            {
                var jj = y + j;

                for (var i = 0; i < mbWidth; i++)
                {
                    var ii = x + i;

                    var predicted = mPred[j][i]; // Predicted sample
                    var residualRecons = MathHelper.RshiftRound(mrInv[j][i], dqBits); // Reconstructed residual sample
                    var originalRecons = residualRecons + predicted; // Reconstructed original sample
                    originalRecons = MathHelper.Clip(maxImagePelValue, originalRecons);
                    outFrameBuffer.SetY8Bit(ii, jj, originalRecons);
                }
            }
        }

        public void Write(IH264EntropyOutputStream outStream, int codedBlockPattern)
        {
            const int maxNumDcCoeff = 16;
            const int maxNumAcCoeff = 15;
            var coeffLevel = new int[maxNumDcCoeff];
            var coeffRun = new int[maxNumDcCoeff];
            var blockInfo = info.LumaDcBlockInfo;
            scanner.Reorder4X4(mDc, coeffLevel, coeffRun, 0, maxNumDcCoeff, 0, 0);
            outStream.WriteResidualBlock(coeffLevel, coeffRun, blockInfo);

            if ((codedBlockPattern & 15) == 0) return;
            for (var i8X8 = 0; i8X8 < 4; i8X8++)
            {
                for (var i4X4 = 0; i4X4 < 4; i4X4++)
                {
                    var blockY = 4*(2*(i8X8 >> 1) + (i4X4 >> 1));
                    var blockX = 4*(2*(i8X8 & 0x01) + (i4X4 & 0x01));

                    Array.Clear(coeffLevel, 0, coeffLevel.Length);
                    Array.Clear(coeffRun, 0, coeffRun.Length);
                    scanner.Reorder4X4(mResd, coeffLevel, coeffRun, 1, maxNumAcCoeff, blockY, blockX);
                    blockInfo = info.GetLumaAcBlockInfo(blockX >> 2, blockY >> 2);
                    outStream.WriteResidualBlock(coeffLevel, coeffRun, blockInfo);
                }
            }
        }

        public int GetDistortion()
        {
            return distortion.GetDistortion16X16(mOrig, mPred);
        }

        public MacroblockInfo GetMacroblockInfo()
        {
            return info;
        }

        protected abstract bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mp);

        private void FillOriginalMatrix(YuvFrameBuffer origiFrameBuffer)
        {
            for (var j = 0; j < mbHeight; j++)
            {
                var jj = y + j;
                for (var i = 0; i < mbWidth; i++)
                {
                    var ii = x + i;
                    mOrig[j][i] = origiFrameBuffer.GetY8Bit(ii, jj);
                }
            }
        }

        private void FillResidualMatrix()
        {
            for (var j = 0; j < mbHeight; j++)
                for (var i = 0; i < mbWidth; i++)
                    mResd[j][i] = mOrig[j][i] - mPred[j][i];
        }

        private int ForwardTransform(int[][] mrSrc, int[][] m4Dst, int[][] mrDst)
        {
            const int maxNumDcCoeff = 16;
            const int maxNumAcCoeff = 15;
            var acCoeff = 0;

            for (var blockY = 0; blockY < mbHeight; blockY += 4)
                for (var blockX = 0; blockX < mbWidth; blockX += 4)
                    transform.Forward4X4(mrSrc, mrDst, blockY, blockX);

            for (var j = 0; j < dcHeight; j++)
                for (var i = 0; i < dcWidth; i++)
                    m4Dst[j][i] = mrDst[j << 2][i << 2];

            transform.Hadamard4X4(m4Dst, m4Dst);
            var nonZeroCoeff = quantizer.Quantization4X4DC(m4Dst, m4Dst);
            var blockInfo = new ResidualBlockInfo(ResidualBlockType.Intra16X16LumaDCLevel, maxNumDcCoeff, nonZeroCoeff);
            info.LumaDcBlockInfo = blockInfo;

            for (var blockY = 0; blockY < mbHeight; blockY += 4)
            {
                for (var blockX = 0; blockX < mbHeight; blockX += 4)
                {
                    nonZeroCoeff = quantizer.Quantization4X4AC(mrDst, mrDst, blockY, blockX);
                    blockInfo = new ResidualBlockInfo(ResidualBlockType.Intra16X16LumaACLevel, maxNumAcCoeff,
                                                      nonZeroCoeff);
                    info.SetLumaAcBlockInfo(blockX >> 2, blockY >> 2, blockInfo);
                    if (nonZeroCoeff > 0)
                        acCoeff = 15;
                }
            }
            return acCoeff;
        }

        private void InverseTransform(int[][] m4Src, int[][] mrSrc, int[][] mrDst)
        {
            var m4Inv = new int[dcHeight][];
            for (var i = 0; i < m4Inv.Length; i++)
                m4Inv[i] = new int[dcWidth];

            for (var j = 0; j < mbHeight; j++)
                for (var i = 0; i < mbWidth; i++)
                    mrDst[j][i] = mrSrc[j][i];

            transform.Ihadamard4X4(m4Src, m4Inv);
            quantizer.Iquantization4X4DC(m4Inv, m4Inv);
            for (var j = 0; j < dcHeight; j++)
                for (var i = 0; i < dcWidth; i++)
                    mrDst[j << 2][i << 2] = m4Inv[j][i];

            for (var blockY = 0; blockY < mbHeight; blockY += 4)
                for (var blockX = 0; blockX < mbWidth; blockX += 4)
                {
                    quantizer.Iquantization4X4AC(mrDst, mrDst, blockY, blockX);
                    transform.Inverse4X4(mrDst, mrDst, blockY, blockX);
                }
        }
    }
}