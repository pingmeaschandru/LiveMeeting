using System;
using TW.Core.Helper;
using TW.H264Coder.Vcl.Entropy;

namespace TW.H264Coder.Vcl.Algorithm.Impl
{
    public class IntegerQuantizer : Quantizer
    {
        private const int MaxQp = 51;
        private const int QBits = 15;

        private static readonly int[][][] QuantCoeff = new[] {
                    new[] {
                            new[] {13107, 8066, 13107, 8066}, new[] {8066, 5243, 8066, 5243},
                            new[] {13107, 8066, 13107, 8066}, new[] {8066, 5243, 8066, 5243}
                        },
                    new[] {
                            new[] {11916, 7490, 11916, 7490}, new[] {7490, 4660, 7490, 4660},
                            new[] {11916, 7490, 11916, 7490}, new[] {7490, 4660, 7490, 4660}
                        },
                    new[] {
                            new[] {10082, 6554, 10082, 6554}, new[] {6554, 4194, 6554, 4194},
                            new[] {10082, 6554, 10082, 6554}, new[] {6554, 4194, 6554, 4194}
                        },
                    new[] {
                            new[] {9362, 5825, 9362, 5825}, new[] {5825, 3647, 5825, 3647},
                            new[] {9362, 5825, 9362, 5825}, new[] {5825, 3647, 5825, 3647}
                        },
                    new[] {
                            new[] {8192, 5243, 8192, 5243}, new[] {5243, 3355, 5243, 3355},
                            new[] {8192, 5243, 8192, 5243}, new[] {5243, 3355, 5243, 3355}
                        },
                    new[] {
                            new[] {7282, 4559, 7282, 4559}, new[] {4559, 2893, 4559, 2893},
                            new[] {7282, 4559, 7282, 4559}, new[] {4559, 2893, 4559, 2893}
                        }
                };

        // Scaling factor, dequantizer coefficients [QP][j][i] <-> [6][4][4]
        private static readonly int[][][] DequantCoeff = new[] {
                    new[] {
                            new[] {10, 13, 10, 13}, new[] {13, 16, 13, 16},
                            new[] {10, 13, 10, 13}, new[] {13, 16, 13, 16}
                        },
                    new[] {
                            new[] {11, 14, 11, 14}, new[] {14, 18, 14, 18},
                            new[] {11, 14, 11, 14}, new[] {14, 18, 14, 18}
                        },
                    new[] {
                            new[] {13, 16, 13, 16}, new[] {16, 20, 16, 20},
                            new[] {13, 16, 13, 16}, new[] {16, 20, 16, 20}
                        },
                    new[] {
                            new[] {14, 18, 14, 18}, new[] {18, 23, 18, 23},
                            new[] {14, 18, 14, 18}, new[] {18, 23, 18, 23}
                        },
                    new[] {
                            new[] {16, 20, 16, 20}, new[] {20, 25, 20, 25},
                            new[] {16, 20, 16, 20}, new[] {20, 25, 20, 25}
                        },
                    new[] {
                            new[] {18, 23, 18, 23}, new[] {23, 29, 23, 29},
                            new[] {18, 23, 18, 23}, new[] {23, 29, 23, 29}
                        }
                };

        private static readonly short[][] Offset4X4Intra = new[] {
                    new short[] {682, 682, 682, 682}, new short[] {682, 682, 682, 682},
                    new short[] {682, 682, 682, 682}, new short[] {682, 682, 682, 682}
                };

        private int[] qpPerMatrix; // Pre-calculated floor(QP/6) for qbits
        private int[] qpRemMatrix; // Multiplication Factor indexer
        private int[][][] levelOffset4X4Intra; // Pre-calculated offset for each QP
        private int[][] invLevelScale4X4;

        public IntegerQuantizer(int qp) : base(qp)
        {
            InitQMatrix();
            CalculateQuantParam();
            CalculateOffsetParam();
        }

        private void InitQMatrix()
        {
            qpPerMatrix = new int[MaxQp + 1];
            qpRemMatrix = new int[MaxQp + 1];

            for (var i = 0; i < MaxQp + 1; i++)
            {
                qpPerMatrix[i] = i/6;
                qpRemMatrix[i] = i%6;
            }
        }

        private void CalculateQuantParam()
        {
            invLevelScale4X4 = new int[Partition.Block.Block_4X4Size][];
            for (var i = 0; i < invLevelScale4X4.Length; i++)
                invLevelScale4X4[i] = new int[Partition.Block.Block_4X4Size];

            var qpRem = qpRemMatrix[qp];
            for (var j = 0; j < Partition.Block.Block_4X4Size; j++)
                for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
                    invLevelScale4X4[j][i] = DequantCoeff[qpRem][j][i] << 4;
        }

        private void CalculateOffsetParam()
        {
            levelOffset4X4Intra = new int[MaxQp + 1][][];
            for (var i = 0; i < levelOffset4X4Intra.Length; i++)
            {
                levelOffset4X4Intra[i] = new int[Partition.Block.Block_4X4Size][];
                for (var j = 0; j < levelOffset4X4Intra[i].Length; j++)
                    levelOffset4X4Intra[i][j] = new int[Partition.Block.Block_4X4Size];
            }

            const int offsetBits = 11;

            var k = qpPerMatrix[qp];
            var qpPer = QBits + k - offsetBits;

            for (var qpTmp = 0; qpTmp < MaxQp + 1; qpTmp++)
                for (var j = 0; j < Partition.Block.Block_4X4Size; j++)
                    for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
                        levelOffset4X4Intra[qpTmp][j][i] = Offset4X4Intra[j][i] << qpPer;
        }

        public override int Quantization4X4DC(int[][] src, int[][] dst)
        {
            var nonZeroCoeff = 0;
            var qpPer = qpPerMatrix[qp];
            var qpRem = qpRemMatrix[qp];
            var qbits = QBits + qpPer; // qbits = 15 + floor(QP/6)
            var mf = QuantCoeff[qpRem]; // MF = (2^qbits * PF) / Qstep
            var f = levelOffset4X4Intra[qp]; // f = 2^qbits/3 for Intra

            for (var j = 0; j < Partition.Block.Block_4X4Size; j++)
            {
                for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
                {
                    // |Z(i,j)| = (|Y(i,j)|*MF(0,0) + 2*f) >> (qbits + 1)
                    var level = (Math.Abs(src[j][i])*mf[0][0] + (f[0][0] << 1)) >> (qbits + 1);

                    if (level != 0)
                    {
                        nonZeroCoeff++;

                        if (qp < 10)
                            level = Math.Min(level, Cavlc.CavlcLevelLimit);

                        // sign(Z(i,j)) = sign(Y(i,j))
                        dst[j][i] = level*(Math.Sign(src[j][i]));
                    }
                    else
                        dst[j][i] = 0;
                }
            }

            return nonZeroCoeff;
        }

        public override void Iquantization4X4DC(int[][] src, int[][] dst)
        {
            var qpPer = qpPerMatrix[qp];
            var v = invLevelScale4X4; // V = Qstep * PF * 64

            for (var j = 0; j < Partition.Block.Block_4X4Size; j++)
                for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
                    dst[j][i] = MathHelper.RshiftRound(((src[j][i]*v[0][0]) << qpPer), 6);
                        // W'(i,j) = Z(i,j) * V(i,j) * 2^floor(QP/6)
        }

        public override int Quantization2X2DC(int[][] src, int[][] dst)
        {
            var nonZeroCoeff = 0;
            var qpPer = qpPerMatrix[qp];
            var qpRem = qpRemMatrix[qp];
            var qbits = QBits + qpPer; // qbits = 15 + floor(QP/6)
            var mf = QuantCoeff[qpRem]; // MF = (2^qbits * PF) / Qstep
            var f = levelOffset4X4Intra[qp]; // f = 2^qbits/3 for Intra

            for (var j = 0; j < Partition.Block.Block_2X2Size; j++)
            {
                for (var i = 0; i < Partition.Block.Block_2X2Size; i++)
                {
                    // |Z(i,j)| = (|Y(i,j)|*MF(0,0) + 2*f) >> (qbits + 1)
                    var level = (Math.Abs(src[j][i])*mf[0][0] + (f[0][0] << 1)) >> (qbits + 1);

                    if (level != 0)
                    {
                        nonZeroCoeff++;

                        if (qp < 4)
                            level = Math.Min(level, Cavlc.CavlcLevelLimit);

                        // sign(Z(i,j)) = sign(Y(i,j))
                        dst[j][i] = level*Math.Sign(src[j][i]);
                    }
                    else
                        dst[j][i] = 0;
                }
            }

            return nonZeroCoeff;
        }

        public override void Iquantization2X2DC(int[][] src, int[][] dst)
        {
            var qpPer = qpPerMatrix[qp];
            var v = invLevelScale4X4; // V = Qstep * PF * 64

            for (var j = 0; j < Partition.Block.Block_2X2Size; j++)
                for (var i = 0; i < Partition.Block.Block_2X2Size; i++)
                    dst[j][i] = ((src[j][i]*v[0][0]) << qpPer) >> 5; // W'(i,j) = Z(i,j) * V(i,j) * 2^floor(QP/6)
        }

        public override int Quantization4X4AC(int[][] src, int[][] dst, int posY, int posX)
        {
            var nonZeroCoeff = 0;
            var qpPer = qpPerMatrix[qp];
            var qpRem = qpRemMatrix[qp];
            var qbits = QBits + qpPer; // qbits = 15 + floor(QP/6)
            var mf = QuantCoeff[qpRem]; // MF = (2^qbits * PF) / Qstep
            var f = levelOffset4X4Intra[qp]; // f = 2^qbits/3 for Intra

            for (var coeff = 1; coeff < 16; coeff++)
            {
                var j = coeff/Partition.Block.Block_4X4Size;
                var i = coeff%Partition.Block.Block_4X4Size;
                var jj = posY + j;
                var ii = posX + i;
                // |Z(i,j)| = (|Y(i,j)|*MF(0,0) + 2*f) >> (qbits + 1)
                var level = (Math.Abs(src[jj][ii])*mf[j][i] + f[j][i]) >> qbits;

                if (level != 0)
                {
                    nonZeroCoeff++;

                    if (qp < 10)
                        level = Math.Min(level, Cavlc.CavlcLevelLimit);

                    // sign(Z(i,j)) = sign(Y(i,j))
                    dst[jj][ii] = level*Math.Sign(src[jj][ii]);
                }
                else
                    dst[jj][ii] = 0;
            }

            return nonZeroCoeff;
        }

        public override void Iquantization4X4AC(int[][] src, int[][] dst, int posY, int posX)
        {
            var qpPer = qpPerMatrix[qp];
            var v = invLevelScale4X4; // V = Qstep * PF * 64

            for (var coeff = 1; coeff < 16; coeff++)
            {
                var j = coeff/Partition.Block.Block_4X4Size;
                var i = coeff%Partition.Block.Block_4X4Size;
                var jj = posY + j;
                var ii = posX + i;
                // inverse scale
                // W'(i,j) = Z(i,j) * V(i,j) * 2^floor(QP/4)
                dst[jj][ii] = MathHelper.RshiftRound((src[jj][ii]*v[j][i]) << qpPer, 4);
            }
        }
    }
}