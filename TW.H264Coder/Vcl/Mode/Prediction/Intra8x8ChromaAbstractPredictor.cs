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
    public abstract class Intra8X8ChromaAbstractPredictor : IIntraPredictor
    {
        protected YuvFrameBuffer inputFrameBuffer; // raw YUV frame
        protected YuvFrameBuffer outputFrameBuffer; // encoded frame
        protected int x; // macroblock left up corner x0
        protected int y; // macroblock left up corner y0
        protected int[][] mOrigCb;
        protected int[][] mOrigCr;
        protected int[][] mResdCb;
        protected int[][] mResdCr;
        protected int[][] mPredCb;
        protected int[][] mPredCr;
        protected int[][] mDcCb;
        protected int[][] mDcCr;
        protected MacroblockAccess access;
        protected MacroblockInfo info;
        protected ITransform transform;
        protected Quantizer quantizer;
        protected IDistortionMetric distortion;
        protected IScanner scanner;
        protected int mbWidthC = 8;
        protected int mbHeightC = 8;
        protected int dcWidthC = 2;
        protected int dcHeightC = 2;
        protected int maxImagePelValue = 255; // TODO centralize this value
        protected int bitDepthC = 8; // TODO 8-bit fixed centralize this value

        protected Intra8X8ChromaAbstractPredictor(int x, int y, Macroblock macroblock, IAlgorithmFactory algorithms)
        {
            this.x = x;
            this.y = y;
            access = macroblock.MacroblockAccess;
            info = new MacroblockInfo(macroblock);
            transform = algorithms.CreateTransform();
            quantizer = algorithms.CreateQuantizer();
            distortion = algorithms.CreateDistortionMetric();
            scanner = algorithms.CreateScanner();
            mOrigCb = new int[mbHeightC][];
            for (var i = 0; i < mOrigCb.Length; i++)
                mOrigCb[i] = new int[mbWidthC];

            mOrigCr = new int[mbHeightC][];
            for (var i = 0; i < mOrigCr.Length; i++)
                mOrigCr[i] = new int[mbWidthC];

            mResdCb = new int[mbHeightC][];
            for (var i = 0; i < mResdCb.Length; i++)
                mResdCb[i] = new int[mbWidthC];

            mResdCr = new int[mbHeightC][];
            for (var i = 0; i < mResdCr.Length; i++)
                mResdCr[i] = new int[mbWidthC];

            mPredCb = new int[mbHeightC][];
            for (var i = 0; i < mPredCb.Length; i++)
                mPredCb[i] = new int[mbWidthC];

            mPredCr = new int[mbHeightC][];
            for (var i = 0; i < mPredCr.Length; i++)
                mPredCr[i] = new int[mbWidthC];

            mDcCb = new int[dcHeightC][];
            for (var i = 0; i < mDcCb.Length; i++)
                mDcCb[i] = new int[mbWidthC];

            mDcCr = new int[dcHeightC][];
            for (var i = 0; i < mDcCr.Length; i++)
                mDcCr[i] = new int[mbWidthC];
        }

        public bool Predict(YuvFrameBuffer origiFrameBuffer, YuvFrameBuffer codedFrameBuffer)
        {
            FillOriginalMatrix(origiFrameBuffer);
            return DoIntraPrediction(codedFrameBuffer, mPredCb, mPredCr);
        }

        public int Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            FillResidualMatrix();
            var nonZeroCoeffCb = ForwardTransform(mResdCb, mDcCb, mResdCb);
            var nonZeroCoeffCr = ForwardTransform(mResdCr, mDcCr, mResdCr);
            return (Math.Max(nonZeroCoeffCb, nonZeroCoeffCr) << 4);
        }

        public void Reconstruct(YuvFrameBuffer outFrameBuffer)
        {
            const int dqBits = 6;
            var mrInvCb = new int[mbHeightC][];
            for (var i = 0; i < mrInvCb.Length; i++)
                mrInvCb[i] = new int[mbWidthC];

            var mrInvCr = new int[mbHeightC][];
            for (var i = 0; i < mrInvCr.Length; i++)
                mrInvCr[i] = new int[mbWidthC];

            InverseTransform(mDcCb, mResdCb, mrInvCb);
            InverseTransform(mDcCr, mResdCr, mrInvCr);

            for (var j = 0; j < mbHeightC; j++)
            {
                var jj = y + j;

                for (var i = 0; i < mbWidthC; i++)
                {
                    var ii = x + i;

                    var predictedCb = mPredCb[j][i];
                    var residualReconsCb = MathHelper.RshiftRound(mrInvCb[j][i], dqBits);
                    var originalReconsCb = residualReconsCb + predictedCb;
                    originalReconsCb = MathHelper.Clip(maxImagePelValue, originalReconsCb);
                    outFrameBuffer.SetCb8Bit(ii, jj, originalReconsCb);

                    var predictedCr = mPredCr[j][i];
                    var residualReconsCr = MathHelper.RshiftRound(mrInvCr[j][i], dqBits);
                    var originalReconsCr = residualReconsCr + predictedCr;
                    originalReconsCr = MathHelper.Clip(maxImagePelValue, originalReconsCr);
                    outFrameBuffer.SetCr8Bit(ii, jj, originalReconsCr);
                }
            }
        }

        public void Write(IH264EntropyOutputStream outStream, int codedBlockPattern)
        {
            const int maxNumCoeff = 4;
            var coeffLevelCb = new int[maxNumCoeff];
            var coeffLevelCr = new int[maxNumCoeff];
            var coeffRunCb = new int[maxNumCoeff];
            var coeffRunCr = new int[maxNumCoeff];

            // TODO DC Info is the same as AC(0,0)?
            var blockInfo = info.ChromaDcBlockInfo;

            if (codedBlockPattern > 15)
            {
                scanner.Reorder2X2(mDcCb, coeffLevelCb, coeffRunCb, 0, maxNumCoeff, 0, 0);
                scanner.Reorder2X2(mDcCr, coeffLevelCr, coeffRunCr, 0, maxNumCoeff, 0, 0);
                outStream.WriteResidualBlock(coeffLevelCb, coeffRunCb, blockInfo);
                outStream.WriteResidualBlock(coeffLevelCr, coeffRunCr, blockInfo);
            }

            if (codedBlockPattern >> 4 != 2) return;
            for (var blockY = 0; blockY < mbHeightC; blockY += 4)
            {
                for (var blockX = 0; blockX < mbWidthC; blockX += 4)
                {
                    scanner.Reorder2X2(mResdCb, coeffLevelCb, coeffRunCb, 1, maxNumCoeff, blockY, blockX);
                    scanner.Reorder2X2(mResdCr, coeffLevelCr, coeffRunCr, 1, maxNumCoeff, blockY, blockX);
                    blockInfo = info.GetChromaAcBlockInfo(blockX >> 2, blockY >> 2);
                    outStream.WriteResidualBlock(coeffLevelCb, coeffRunCb, blockInfo);
                    outStream.WriteResidualBlock(coeffLevelCr, coeffRunCr, blockInfo);
                }
            }
        }

        public int GetDistortion()
        {
            var distortionCb = 0;
            var distortionCr = 0;

            for (var blockY = 0; blockY < mbHeightC; blockY += 4)
            {
                for (var blockX = 0; blockX < mbWidthC; blockX += 4)
                {
                    distortionCb += distortion.GetDistortion4X4(mOrigCb, mPredCb, blockY, blockX);
                    distortionCr += distortion.GetDistortion4X4(mOrigCr, mPredCr, blockY, blockX);
                }
            }

            // TODO why cost += (int) (enc_mb.lambda_me[Q_PEL] * mvbits[ mode ])?
            return distortionCb + distortionCr;
        }

        public MacroblockInfo GetMacroblockInfo()
        {
            return info;
        }

        protected abstract bool DoIntraPrediction(YuvFrameBuffer codedFrameBuffer, int[][] mpCb, int[][] mpCr);

        private void FillOriginalMatrix(YuvFrameBuffer origiFrameBuffer)
        {
            for (var j = 0; j < mbHeightC; j++)
            {
                var jj = y + j;
                for (var i = 0; i < mbWidthC; i++)
                {
                    var ii = x + i;
                    mOrigCb[j][i] = origiFrameBuffer.GetCb8Bit(ii, jj);
                    mOrigCr[j][i] = origiFrameBuffer.GetCr8Bit(ii, jj);
                }
            }
        }

        private void FillResidualMatrix()
        {
            for (var j = 0; j < mbHeightC; j++)
            {
                for (var i = 0; i < mbWidthC; i++)
                {
                    mResdCb[j][i] = mOrigCb[j][i] - mPredCb[j][i];
                    mResdCr[j][i] = mOrigCr[j][i] - mPredCr[j][i];
                }
            }
        }

        private int ForwardTransform(int[][] mrSrc, int[][] m2Dst, int[][] mrDst)
        {
            const int maxNumCoeff = 4;
            var acCoeff = 0;
            var dcCoeff = 0;

            for (var blockY = 0; blockY < mbHeightC; blockY += 4)
                for (var blockX = 0; blockX < mbWidthC; blockX += 4)
                    transform.Forward4X4(mrSrc, mrDst, blockY, blockX);

            for (var j = 0; j < dcHeightC; j++)
                for (var i = 0; i < dcWidthC; i++)
                    m2Dst[j][i] = mrDst[j << 2][i << 2];

            transform.Hadamard2X2(m2Dst, m2Dst);

            var nonZeroCoeff = quantizer.Quantization2X2DC(m2Dst, m2Dst);
            if (nonZeroCoeff > 0)
                dcCoeff = 1;

            // TODO Fulfill the Info of this residual block
            var blockInfo = new ResidualBlockInfo(ResidualBlockType.ChromaDCLevel, maxNumCoeff, nonZeroCoeff);
            info.ChromaDcBlockInfo = blockInfo;

            for (var blockY = 0; blockY < mbHeightC; blockY += 4)
            {
                for (var blockX = 0; blockX < mbWidthC; blockX += 4)
                {
                    nonZeroCoeff = quantizer.Quantization4X4AC(mrDst, mrDst, blockY, blockX);
                    blockInfo = new ResidualBlockInfo(ResidualBlockType.ChromaACLevel, maxNumCoeff, nonZeroCoeff);
                    info.SetChromaAcBlockInfo(blockX >> 2, blockY >> 2, blockInfo);
                    if ((nonZeroCoeff <= 0)) continue;
                    for (var jj = blockY; jj < (blockY + 4); jj++)
                        for (var ii = blockX; ii < (blockX + 4); ii++)
                            mrDst[jj][ii] = 0;
                    acCoeff = 0;
                }
            }

            return (dcCoeff + acCoeff);
        }

        private void InverseTransform(int[][] m2Src, int[][] mrSrc, int[][] mrDst)
        {
            var m2Inv = new int[dcHeightC][];
            for (var i = 0; i < m2Inv.Length; i++)
                m2Inv[i] = new int[dcWidthC];

            for (var j = 0; j < mbHeightC; j++)
                for (var i = 0; i < mbWidthC; i++)
                    mrDst[j][i] = mrSrc[j][i];

            transform.Ihadamard2X2(m2Src, m2Inv);
            quantizer.Iquantization2X2DC(m2Inv, m2Inv);

            for (var j = 0; j < dcHeightC; j++)
                for (var i = 0; i < dcWidthC; i++)
                    mrDst[j << 2][i << 2] = m2Inv[j][i];

            for (var blockY = 0; blockY < mbHeightC; blockY += 4)
            {
                for (var blockX = 0; blockX < mbWidthC; blockX += 4)
                {
                    quantizer.Iquantization4X4AC(mrDst, mrDst, blockY, blockX);
                    transform.Inverse4X4(mrDst, mrDst, blockY, blockX);
                }
            }
        }
    }
}