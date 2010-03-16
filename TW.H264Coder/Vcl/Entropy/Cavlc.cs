using System;
using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Block;
using TW.H264Coder.Vcl.Datatype;

namespace TW.H264Coder.Vcl.Entropy
{
    public class Cavlc : VlcTable, IEntropyOutputStream
    {
        public static readonly int CavlcLevelLimit = 2063;
        private const int RunbeforeNumM1 = 6;
        private readonly BitOutputStream bitStream;

        public Cavlc(BitOutputStream bitStream)
        {
            this.bitStream = bitStream;
        }

        public int WriteUv(int n, int value)
        {
            var se = new SyntaxElement {Bitpattern = value, Length = n, Value1 = value};
            WriteUVLC2Buffer(se, bitStream);
            return se.Length;
        }

        public int WriteU1(bool value)
        {
            var se = new SyntaxElement {Bitpattern = value ? 1 : 0, Length = 1, Value1 = value ? 1 : 0};
            WriteUVLC2Buffer(se, bitStream);
            return se.Length;
        }

        public int WriteUeV(int value)
        {
            var se = new SyntaxElement {Value1 = value, Value2 = 0};
            UeLinfo(se);
            Symbol2Uvlc(se);
            WriteUVLC2Buffer(se, bitStream);
            return se.Length;
        }

        public int WriteSeV(int value)
        {
            var se = new SyntaxElement {Value1 = value, Value2 = 0};
            SeLinfo(se);
            Symbol2Uvlc(se);
            WriteUVLC2Buffer(se, bitStream);
            return se.Length;
        }

        public int WriteMbType(int value)
        {
            return WriteUeV(value);
        }

        public int WriteIntraChromaPredMode(int value)
        {
            return WriteUeV(value);
        }

        public int WriteMbQpDelta(int value)
        {
            return WriteSeV(value);
        }

        public int WriteResidualBlock(int[] coeffLevel, int[] coeffRun, ResidualBlockInfo residualBlock)
        {
            var maxNumCoeff = residualBlock.MaxNumCoeff;
            var totalCoeff = 0; // Number of non-zero coefficients of this block
            var trailingOnes = 0; // Number of +-1s
            var totalZeros = 0; // Total number of zero coefficients
            var lastCoeff = 0; // Last non-zero coefficient
            var numBits = 0; // Amount of bits used to code this residual block

            for (var k = 0; k < maxNumCoeff; k++)
            {
                var level = coeffLevel[k];
                var run = coeffRun[k];

                // TODO System.err.printf("%3d ", level);

                if (level == 0) continue;
                totalZeros += run;

                if (Math.Abs(level) == 1)
                {
                    trailingOnes++;
                    trailingOnes = Math.Min(trailingOnes, 3); // Clip to 3
                }
                else
                    trailingOnes = 0;

                totalCoeff++;
                lastCoeff = k;
            }

            // TODO System.err.println();
            var vlcTableIdx = SelectVlcTableIndex(residualBlock);
            numBits += WriteCoeffToken(totalCoeff, trailingOnes, vlcTableIdx);

            if (totalCoeff > 0)
            {
                var trailingOnesSignFlags = 0;
                for (var k = lastCoeff; k > (lastCoeff - trailingOnes); k--)
                {
                    var level = coeffLevel[k];
                    trailingOnesSignFlags <<= 1;
                    if (level < 0)
                        trailingOnesSignFlags |= 0x1;
                }
                if (trailingOnes > 0)
                    numBits += WriteTrailingOnesSignFlag(trailingOnesSignFlags, trailingOnes);

                var suffixLength = ((totalCoeff > 10) && (trailingOnes < 3)) ? 1 : 0;
                var incVlc = new[] {0, 3, 6, 12, 24, 48, 32768};
                // TODO what is this level two or higher?
                var levelTwoOrHigher = (totalCoeff > 3 && trailingOnes == 3) ? 0 : 1;

                for (var k = (lastCoeff - trailingOnes); k >= 0; k--)
                {
                    var level = coeffLevel[k];
                    if (levelTwoOrHigher != 0)
                    {
                        levelTwoOrHigher = 0;
                        if (level > 0)
                            level--;
                        else
                            level++;
                    }

                    numBits += WriteLevel(level, suffixLength);
                    if (Math.Abs(level) > incVlc[suffixLength])
                        suffixLength++;

                    if ((k == (lastCoeff - trailingOnes)) && (Math.Abs(level) > 3))
                        suffixLength = 2;
                }

                numBits += WriteTotalZeros(totalZeros, totalCoeff, maxNumCoeff);
                var zerosLeft = totalZeros;
                var numCoeff = totalCoeff;
                for (var k = lastCoeff; k >= 0; k--)
                {
                    var runBefore = coeffRun[k];
                    if ((zerosLeft == 0) || (numCoeff <= 1))
                        break;

                    numBits += WriteRunBefore(runBefore, zerosLeft);

                    zerosLeft -= runBefore;
                    numCoeff--;
                }
            }

            return numBits;
        }

        private int WriteCoeffToken(int totalCoeff, int trailingOnes, int vlcTableIdx)
        {
            var se = new SyntaxElement();

            if (vlcTableIdx == 3)
            {
                // Table 4: Fixed Length

                se.Length = 6; // 4 + 2 bit FLC
                if (totalCoeff > 0)
                    se.Info = ((totalCoeff - 1) << 2) | trailingOnes;
                else
                    se.Info = 3;
            }
            else
            {
                // Tables 1, 2, 3, 5 and 6: Variable Length.
                // Table 1, 2 and 3: Luma
                // Table 5: Chroma YUV 4:2:0
                // Table 6: Chroma YUV 4:2:2
                se.Length = CoeffTokenCodeLength[vlcTableIdx][trailingOnes][totalCoeff];
                se.Info = CoeffTokenCodeValue[vlcTableIdx][trailingOnes][totalCoeff];
            }

            // TODO System.err.printf("[WriteCoeffToken] vlcTableIdx=%d Info=%d Length=%d\n", vlcTableIdx, se.Info, se.Length);

            Symbol2Vlc(se);
            WriteUVLC2Buffer(se, bitStream);

            return se.Length;
        }

        private int WriteTrailingOnesSignFlag(int trailingOnesSignFlags, int trailingOnes)
        {
            var numBits = 0;

            if (trailingOnes >= 0)
            {
                var se = new SyntaxElement {Length = trailingOnes, Info = trailingOnesSignFlags};
                Symbol2Vlc(se);
                WriteUVLC2Buffer(se, bitStream);
                numBits = se.Length;
            }

            return numBits;
        }

        private int WriteLevel(int level, int levelSuffixSize)
        {
            return levelSuffixSize == 0 ? WriteLevelVLC1(level) : WriteLevelVlcn(level, levelSuffixSize);
        }

        private int WriteLevelVLC1(int level)
        {
            var se = new SyntaxElement();
            var levabs = Math.Abs(level);
            var sign = (level < 0 ? 1 : 0);

            if (levabs < 8)
            {
                se.Length = levabs*2 + sign - 1;
                se.Info = 1;
            }
            else if (levabs < 16)
            {
                se.Length = 19;
                se.Info = (1 << 4) | ((levabs - 8) << 1) | sign;
            }
            else
            {
                int iLength = 28, numPrefix = 15;
                var levabsm16 = levabs - 16;

                if ((levabsm16) >= 2048)
                {
                    numPrefix++;
                    while ((levabsm16) >= (1 << (numPrefix - 3)) - 4096)
                        numPrefix++;
                }

                var addbit = numPrefix - 15;
                iLength += (addbit << 1);
                var iCodeword = (1 << (12 + addbit)) | ((levabsm16) << 1) | sign;

                if (numPrefix > 15)
                {
                    se.Length = 0x0000FFFF; // This can be some other big number
                    se.Info = iCodeword;
                }

                se.Length = iLength;
                se.Info = iCodeword;
            }

            Symbol2Vlc(se);
            WriteUVLC2Buffer(se, bitStream);

            return se.Length;
        }

        private int WriteLevelVlcn(int level, int vlc)
        {
            var se = new SyntaxElement();
            int iCodeword;
            int iLength;

            var sign = (level < 0 ? 1 : 0);
            var levabs = Math.Abs(level) - 1;

            var shift = vlc - 1;
            var escape = (15 << shift);

            if (levabs < escape)
            {
                var sufmask = (int) (~((0xffffffff) << shift));
                var suffix = (levabs) & sufmask;
                var numPrefix = (levabs) >> shift;

                iLength = numPrefix + vlc + 1;
                iCodeword = (1 << (shift + 1)) | (suffix << 1) | sign;
            }
            else
            {
                var levabsesc = levabs - escape;
                var numPrefix = 15;

                iLength = 28;
                if ((levabsesc) >= 2048)
                {
                    numPrefix++;
                    while ((levabsesc) >= (1 << (numPrefix - 3)) - 4096)
                        numPrefix++;
                }

                var addbit = numPrefix - 15;

                iLength += (addbit << 1);
                var offset = (2048 << addbit) - 2048;

                iCodeword = (1 << (12 + addbit)) | ((levabsesc - offset) << 1) | sign;
                if (numPrefix > 15)
                {
                    se.Length = 0x0000FFFF; // This can be some other big number
                    se.Info = iCodeword;
                }
            }
            se.Length = iLength;
            se.Info = iCodeword;

            Symbol2Vlc(se);
            WriteUVLC2Buffer(se, bitStream);

            return se.Length;
        }

        private int WriteTotalZeros(int totalZeros, int totalCoeff, int maxNumCoeff)
        {
            var numBits = 0;

            if (totalCoeff < maxNumCoeff)
            {
                var se = new SyntaxElement();

                var vlcTable = totalCoeff - 1;
                if (maxNumCoeff == 4)
                {
                    // YUV 4:2:0
                    se.Length = TotalZeros2X2CodeLength[vlcTable][totalZeros];
                    se.Info = TotalZeros2X2CodeValue[vlcTable][totalZeros];
                }
                else
                {
                    // Luma, YUV 4:4:4
                    se.Length = TotalZeros4X4CodeLength[vlcTable][totalZeros];
                    se.Info = TotalZeros4X4CodeValue[vlcTable][totalZeros];
                }

                Symbol2Vlc(se);
                WriteUVLC2Buffer(se, bitStream);

                numBits = se.Length;
            }

            return numBits;
        }

        private int WriteRunBefore(int runBefore, int zerosLeft)
        {
            const int numBits = 0;

            if (zerosLeft > 0)
            {
                var se = new SyntaxElement();
                var vlcTable = Math.Min(zerosLeft - 1, RunbeforeNumM1);

                se.Length = RunBeforeCodeLength[vlcTable][runBefore];
                se.Info = RunBeforeCodeValue[vlcTable][runBefore];

                Symbol2Vlc(se);
                WriteUVLC2Buffer(se, bitStream);
            }

            return numBits;
        }

        private static void SeLinfo(SyntaxElement se)
        {
            var sign = (se.Value1 <= 0) ? 1 : 0;
            // n+1 is the number in the code table. Based on this we find Length and
            // Info
            var n = Math.Abs(se.Value1) << 1;
            var nn = (n >> 1);
            int i;
            for (i = 0; i < 33 && nn != 0; i++)
                nn >>= 1;

            se.Length = (i << 1) + 1;
            se.Info = n - (1 << i) + sign;
        }

        private static void UeLinfo(SyntaxElement se)
        {
            var nn = (se.Value1 + 1) >> 1;
            int i;
            for (i = 0; i < 33 && nn != 0; i++)
                nn >>= 1;

            se.Length = (i << 1) + 1;
            // read_bits( leadingZeroBits ) = codeNum - 2^leadingZeroBits (page 154)
            se.Info = se.Value1 + 1 - (1 << i);
        }

        private static void Symbol2Uvlc(SyntaxElement se)
        {
            var suffixLen = se.Length >> 1;

            if (suffixLen < 32)
                throw new Exception();

            se.Bitpattern = (1 << suffixLen) | (se.Info & ((1 << suffixLen) - 1));

            return;
        }

        private static void Symbol2Vlc(SyntaxElement se)
        {
            var infoLen = se.Length;
            se.Bitpattern = 0;
            while (--infoLen >= 0)
            {
                se.Bitpattern <<= 1;
                se.Bitpattern |= (0x01 & (se.Info >> infoLen));
            }
            return;
        }

        private static int SelectVlcTableIndex(ResidualBlockInfo residualBlock)
        {
            var type = residualBlock.Type;
            var blkIdxX = residualBlock.BlockIndexX;
            var blkIdxY = residualBlock.BlockIndexY;
            var nC = residualBlock.MacroblockInfo.GetPredictedNonZeroCoeff(blkIdxX, blkIdxY);
            return type == ResidualBlockType.ChromaDCLevel ? 4 : (nC < 2 ? 0 : (nC < 4 ? 1 : (nC < 8 ? 2 : 3)));
        }

        private static void WriteUVLC2Buffer(SyntaxElement se, BitOutputStream currStream)
        {
            currStream.WriteByte(se.Bitpattern, se.Length);
        }
    }
}