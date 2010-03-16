namespace TW.H264Coder.Vcl.Algorithm.Impl
{
    public class IntegerTransform : ITransform
    {
        private readonly int[] tmp;

        public IntegerTransform()
        {
            tmp = new int[64];
        }

        public void Forward4X4(int[][] src, int[][] dst, int posY, int posX)
        {
            int p0, p1, p2, p3;
            int t0, t1, t2, t3;
            var tmpIdx = 0;

            // Horizontal
            for (var i = posY; i < posY + Partition.Block.Block_4X4Size; i++)
            {
                p0 = src[i][posX + 0];
                p1 = src[i][posX + 1];
                p2 = src[i][posX + 2];
                p3 = src[i][posX + 3];

                t0 = p0 + p3;
                t1 = p1 + p2;
                t2 = p1 - p2;
                t3 = p0 - p3;

                tmp[tmpIdx++] = t0 + t1;
                tmp[tmpIdx++] = (t3 << 1) + t2;
                tmp[tmpIdx++] = t0 - t1;
                tmp[tmpIdx++] = t3 - (t2 << 1);
            }

            // Vertical
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                tmpIdx = i;
                p0 = tmp[tmpIdx];
                p1 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                p2 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                p3 = tmp[tmpIdx + Partition.Block.Block_4X4Size];

                t0 = p0 + p3;
                t1 = p1 + p2;
                t2 = p1 - p2;
                t3 = p0 - p3;

                var ii = posX + i;
                dst[posY + 0][ii] = t0 + t1;
                dst[posY + 1][ii] = t2 + (t3 << 1);
                dst[posY + 2][ii] = t0 - t1;
                dst[posY + 3][ii] = t3 - (t2 << 1);
            }
        }

        public void Inverse4X4(int[][] src, int[][] dst, int posY, int posX)
        {
            int p0, p1, p2, p3;
            int t0, t1, t2, t3;
            var tmpIdx = 0;

            // Horizontal
            for (var i = posY; i < posY + Partition.Block.Block_4X4Size; i++)
            {
                t0 = src[i][posX + 0];
                t1 = src[i][posX + 1];
                t2 = src[i][posX + 2];
                t3 = src[i][posX + 3];

                p0 = t0 + t2;
                p1 = t0 - t2;
                p2 = (t1 >> 1) - t3;
                p3 = t1 + (t3 >> 1);

                tmp[tmpIdx++] = p0 + p3;
                tmp[tmpIdx++] = p1 + p2;
                tmp[tmpIdx++] = p1 - p2;
                tmp[tmpIdx++] = p0 - p3;
            }

            // Vertical
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                tmpIdx = i;
                t0 = tmp[tmpIdx];
                t1 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                t2 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                t3 = tmp[tmpIdx + Partition.Block.Block_4X4Size];

                p0 = t0 + t2;
                p1 = t0 - t2;
                p2 = (t1 >> 1) - t3;
                p3 = t1 + (t3 >> 1);

                var ii = posX + i;
                dst[posY + 0][ii] = p0 + p3;
                dst[posY + 1][ii] = p1 + p2;
                dst[posY + 2][ii] = p1 - p2;
                dst[posY + 3][ii] = p0 - p3;
            }
        }

        public void Hadamard4X4(int[][] src, int[][] dst)
        {
            int p0, p1, p2, p3;
            int t0, t1, t2, t3;
            var tmpIdx = 0;

            // Horizontal
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                p0 = src[i][0];
                p1 = src[i][1];
                p2 = src[i][2];
                p3 = src[i][3];

                t0 = p0 + p3;
                t1 = p1 + p2;
                t2 = p1 - p2;
                t3 = p0 - p3;

                tmp[tmpIdx++] = t0 + t1;
                tmp[tmpIdx++] = t3 + t2;
                tmp[tmpIdx++] = t0 - t1;
                tmp[tmpIdx++] = t3 - t2;
            }

            // Vertical
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                tmpIdx = i;
                p0 = tmp[tmpIdx];
                p1 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                p2 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                p3 = tmp[tmpIdx + Partition.Block.Block_4X4Size];

                t0 = p0 + p3;
                t1 = p1 + p2;
                t2 = p1 - p2;
                t3 = p0 - p3;

                dst[0][i] = (t0 + t1) >> 1;
                dst[1][i] = (t2 + t3) >> 1;
                dst[2][i] = (t0 - t1) >> 1;
                dst[3][i] = (t3 - t2) >> 1;
            }
        }

        public void Ihadamard4X4(int[][] src, int[][] dst)
        {
            int p0, p1, p2, p3;
            int t0, t1, t2, t3;
            var tmpIdx = 0;

            // Horizontal
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                t0 = src[i][0];
                t1 = src[i][1];
                t2 = src[i][2];
                t3 = src[i][3];

                p0 = t0 + t2;
                p1 = t0 - t2;
                p2 = t1 - t3;
                p3 = t1 + t3;

                tmp[tmpIdx++] = p0 + p3;
                tmp[tmpIdx++] = p1 + p2;
                tmp[tmpIdx++] = p1 - p2;
                tmp[tmpIdx++] = p0 - p3;
            }

            // Vertical
            for (var i = 0; i < Partition.Block.Block_4X4Size; i++)
            {
                tmpIdx = i;

                t0 = tmp[tmpIdx];
                t1 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                t2 = tmp[tmpIdx += Partition.Block.Block_4X4Size];
                t3 = tmp[tmpIdx + Partition.Block.Block_4X4Size];

                p0 = t0 + t2;
                p1 = t0 - t2;
                p2 = t1 - t3;
                p3 = t1 + t3;

                dst[0][i] = p0 + p3;
                dst[1][i] = p1 + p2;
                dst[2][i] = p1 - p2;
                dst[3][i] = p0 - p3;
            }
        }

        public void Hadamard2X2(int[][] src, int[][] dst)
        {
            var p0 = src[0][0] + src[0][1];
            var p1 = src[0][0] - src[0][1];
            var p2 = src[1][0] + src[1][1];
            var p3 = src[1][0] - src[1][1];

            dst[0][0] = (p0 + p2);
            dst[0][1] = (p1 + p3);
            dst[1][0] = (p0 - p2);
            dst[1][1] = (p1 - p3);
        }

        public void Ihadamard2X2(int[][] src, int[][] dst)
        {
            var t0 = src[0][0] + src[0][1];
            var t1 = src[0][0] - src[0][1];
            var t2 = src[1][0] + src[1][1];
            var t3 = src[1][0] - src[1][1];

            dst[0][0] = (t0 + t2);
            dst[0][1] = (t1 + t3);
            dst[1][0] = (t0 - t2);
            dst[1][1] = (t1 - t3);
        }
    }
}