namespace TW.H264Coder.Vcl.Algorithm.Impl
{
    public class ZigZagFrameScanner : IScanner
    {
        private readonly int[][] scanOrder = new[] {
                    // [one out of 16 coefficients][x or y]
                    new[] {0, 0}, new[] {1, 0}, new[] {0, 1}, new[] {0, 2},
                    new[] {1, 1}, new[] {2, 0}, new[] {3, 0}, new[] {2, 1},
                    new[] {1, 2}, new[] {0, 3}, new[] {1, 3}, new[] {2, 2},
                    new[] {3, 1}, new[] {3, 2}, new[] {2, 3}, new[] {3, 3}
                };

        public void Reorder4X4(int[][] srcCoeff, int[] dstLevel, int[] dstRun, int start, int length, int yOffset, int xOffset)
        {
            var run = -1;
            var scanPos = 0;

            for (var coeff = start; coeff < length; coeff++)
            {
                var i = scanOrder[coeff][0] + xOffset;
                var j = scanOrder[coeff][1] + yOffset;

                run++;

                if (srcCoeff[j][i] == 0) continue;

                dstLevel[scanPos] = srcCoeff[j][i];
                dstRun[scanPos] = run;
                scanPos++;
                run = -1;
            }
        }

        public void Reorder2X2(int[][] srcCoeff, int[] dstLevel, int[] dstRun, int start, int length, int yOffset, int xOffset)
        {
            var run = -1;
            var scanPos = 0;

            for (var coeff = start; coeff < length; coeff++)
            {
                var i = (coeff%2) + xOffset;
                var j = (coeff/2) + yOffset;

                run++;
                var level = srcCoeff[j][i];

                if (level == 0) continue;

                dstLevel[scanPos] = level;
                dstRun[scanPos] = run;
                scanPos++;
                run = -1;
            }
        }
    }
}