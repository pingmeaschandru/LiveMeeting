//package br.ufsc.inf.guiga.media.codec.video.h264.vcl.entropy;

namespace TW.H264Coder.Vcl.Entropy
{
    public class VlcTable
    {
        // [Tables 1, 2, 3, 5 and 6][TrailingOnes][TotalCoeff]
        protected static readonly int[][][] CoeffTokenCodeLength =
            new[]
                {
                    new[]
                        {
                            new[] {1, 6, 8, 9, 10, 11, 13, 13, 13, 14, 14, 15, 15, 16, 16, 16, 16},
                            new[] {0, 2, 6, 8, 9, 10, 11, 13, 13, 14, 14, 15, 15, 15, 16, 16, 16},
                            new[] {0, 0, 3, 7, 8, 9, 10, 11, 13, 13, 14, 14, 15, 15, 16, 16, 16},
                            new[] {0, 0, 0, 5, 6, 7, 8, 9, 10, 11, 13, 14, 14, 15, 15, 16, 16},
                        },
                    new[]
                        {
                            new[] {2, 6, 6, 7, 8, 8, 9, 11, 11, 12, 12, 12, 13, 13, 13, 14, 14},
                            new[] {0, 2, 5, 6, 6, 7, 8, 9, 11, 11, 12, 12, 13, 13, 14, 14, 14},
                            new[] {0, 0, 3, 6, 6, 7, 8, 9, 11, 11, 12, 12, 13, 13, 13, 14, 14},
                            new[] {0, 0, 0, 4, 4, 5, 6, 6, 7, 9, 11, 11, 12, 13, 13, 13, 14},
                        },
                    new[]
                        {
                            new[] {4, 6, 6, 6, 7, 7, 7, 7, 8, 8, 9, 9, 9, 10, 10, 10, 10},
                            new[] {0, 4, 5, 5, 5, 5, 6, 6, 7, 8, 8, 9, 9, 9, 10, 10, 10},
                            new[] {0, 0, 4, 5, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 10},
                            new[] {0, 0, 0, 4, 4, 4, 4, 4, 5, 6, 7, 8, 8, 9, 10, 10, 10},
                        },
                    new[]
                        {
                            //Table 4: Fixed Length 
                            new int[] {},
                        },
                    new[]
                        {
                            //Table 5: YUV 4:2:0
                            new[] {2, 6, 6, 6, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 1, 6, 7, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 3, 7, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 0, 6, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                        },
                    new[]
                        {
                            //Table 6: YUV 4:2:2
                            new[] {1, 7, 7, 9, 9, 10, 11, 12, 13, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 2, 7, 7, 9, 10, 11, 12, 12, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 3, 7, 7, 9, 10, 11, 12, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 0, 5, 6, 7, 7, 10, 11, 0, 0, 0, 0, 0, 0, 0, 0}
                        }
                };

        protected static readonly int[][][] CoeffTokenCodeValue =
            new[]
                {
                    new[]
                        {
                            new[] {1, 5, 7, 7, 7, 7, 15, 11, 8, 15, 11, 15, 11, 15, 11, 7, 4},
                            new[] {0, 1, 4, 6, 6, 6, 6, 14, 10, 14, 10, 14, 10, 1, 14, 10, 6},
                            new[] {0, 0, 1, 5, 5, 5, 5, 5, 13, 9, 13, 9, 13, 9, 13, 9, 5},
                            new[] {0, 0, 0, 3, 3, 4, 4, 4, 4, 4, 12, 12, 8, 12, 8, 12, 8},
                        },
                    new[]
                        {
                            new[] {3, 11, 7, 7, 7, 4, 7, 15, 11, 15, 11, 8, 15, 11, 7, 9, 7},
                            new[] {0, 2, 7, 10, 6, 6, 6, 6, 14, 10, 14, 10, 14, 10, 11, 8, 6},
                            new[] {0, 0, 3, 9, 5, 5, 5, 5, 13, 9, 13, 9, 13, 9, 6, 10, 5},
                            new[] {0, 0, 0, 5, 4, 6, 8, 4, 4, 4, 12, 8, 12, 12, 8, 1, 4},
                        },
                    new[]
                        {
                            new[] {15, 15, 11, 8, 15, 11, 9, 8, 15, 11, 15, 11, 8, 13, 9, 5, 1},
                            new[] {0, 14, 15, 12, 10, 8, 14, 10, 14, 14, 10, 14, 10, 7, 12, 8, 4},
                            new[] {0, 0, 13, 14, 11, 9, 13, 9, 13, 10, 13, 9, 13, 9, 11, 7, 3},
                            new[] {0, 0, 0, 12, 11, 10, 9, 8, 13, 12, 12, 12, 8, 12, 10, 6, 2},
                        },
                    new[]
                        {
                            //Table 4: Fixed Length 
                            new int[] {},
                        },
                    new[]
                        {
                            //Table 5: YUV 4:2:0
                            new[] {1, 7, 4, 3, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 1, 6, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
                        },
                    new[]
                        {
                            //Table 6: YUV 4:2:2
                            new[] {1, 15, 14, 7, 6, 7, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 1, 13, 12, 5, 6, 6, 6, 5, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 1, 11, 10, 4, 5, 5, 4, 0, 0, 0, 0, 0, 0, 0, 0},
                            new[] {0, 0, 0, 1, 1, 9, 8, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0}
                        }
                };

        // [tzVlcIndex (VLC Table)][total_zeros]
        protected static readonly int[][] TotalZeros4X4CodeLength =
            new[]
                {
                    new[] {1, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 9},
                    new[] {3, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 6, 6, 6, 6},
                    new[] {4, 3, 3, 3, 4, 4, 3, 3, 4, 5, 5, 6, 5, 6},
                    new[] {5, 3, 4, 4, 3, 3, 3, 4, 3, 4, 5, 5, 5},
                    new[] {4, 4, 4, 3, 3, 3, 3, 3, 4, 5, 4, 5},
                    new[] {6, 5, 3, 3, 3, 3, 3, 3, 4, 3, 6},
                    new[] {6, 5, 3, 3, 3, 2, 3, 4, 3, 6},
                    new[] {6, 4, 5, 3, 2, 2, 3, 3, 6},
                    new[] {6, 6, 4, 2, 2, 3, 2, 5},
                    new[] {5, 5, 3, 2, 2, 2, 4},
                    new[] {4, 4, 3, 3, 1, 3},
                    new[] {4, 4, 2, 1, 3},
                    new[] {3, 3, 1, 2},
                    new[] {2, 2, 1},
                    new[] {1, 1}
                };

        protected static readonly int[][] TotalZeros4X4CodeValue =
            new[]
                {
                    new[] {1, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 1},
                    new[] {7, 6, 5, 4, 3, 5, 4, 3, 2, 3, 2, 3, 2, 1, 0},
                    new[] {5, 7, 6, 5, 4, 3, 4, 3, 2, 3, 2, 1, 1, 0},
                    new[] {3, 7, 5, 4, 6, 5, 4, 3, 3, 2, 2, 1, 0},
                    new[] {5, 4, 3, 7, 6, 5, 4, 3, 2, 1, 1, 0},
                    new[] {1, 1, 7, 6, 5, 4, 3, 2, 1, 1, 0},
                    new[] {1, 1, 5, 4, 3, 3, 2, 1, 1, 0},
                    new[] {1, 1, 1, 3, 3, 2, 2, 1, 0},
                    new[] {1, 0, 1, 3, 2, 1, 1, 1,},
                    new[] {1, 0, 1, 3, 2, 1, 1,},
                    new[] {0, 1, 1, 2, 1, 3},
                    new[] {0, 1, 1, 1, 1},
                    new[] {0, 1, 1, 1},
                    new[] {0, 1, 1},
                    new[] {0, 1}
                };

        // [tzVlcIndex (VLC Table)][total_zeros]
        protected static readonly int[][] TotalZeros2X2CodeLength =
            new[]
                {
                    new[] {1, 2, 3, 3},
                    new[] {1, 2, 2},
                    new[] {1, 1}
                };

        protected static readonly int[][] TotalZeros2X2CodeValue =
            new[]
                {
                    new[] {1, 1, 1, 0},
                    new[] {1, 1, 0},
                    new[] {1, 0}
                };

        // [zerosLeft][run_before]
        protected static readonly int[][] RunBeforeCodeLength =
            new[]
                {
                    new[] {1, 1},
                    new[] {1, 2, 2},
                    new[] {2, 2, 2, 2},
                    new[] {2, 2, 2, 3, 3},
                    new[] {2, 2, 3, 3, 3, 3},
                    new[] {2, 3, 3, 3, 3, 3, 3},
                    new[] {3, 3, 3, 3, 3, 3, 3, 4, 5, 6, 7, 8, 9, 10, 11},
                };

        protected static readonly int[][] RunBeforeCodeValue =
            new[]
                {
                    new[] {1, 0},
                    new[] {1, 1, 0},
                    new[] {3, 2, 1, 0},
                    new[] {3, 2, 1, 1, 0},
                    new[] {3, 2, 3, 2, 1, 0},
                    new[] {3, 0, 1, 3, 2, 5, 4},
                    new[] {7, 6, 5, 4, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                };
    }
}