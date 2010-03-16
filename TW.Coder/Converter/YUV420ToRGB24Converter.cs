namespace TW.Coder.Converter
{
    public class YUV420ToRGB24Converter : ConverterBase
    {
        private static readonly long[] CrvTab = new long[256];
        private static readonly long[] CbuTab = new long[256];
        private static readonly long[] CguTab = new long[256];
        private static readonly long[] CgvTab = new long[256];
        private static readonly long[] Tab76309 = new long[256];
        private static readonly byte[] Clip = new byte[1024];	

        static YUV420ToRGB24Converter()
        {
            int i;

            const long crv = 104597;
            const long cbu = 132201;
            const long cgu = 25675;
            const long cgv = 53279;

            for (i = 0; i < 256; i++)
            {
                CrvTab[i] = (i - 128) * crv;
                CbuTab[i] = (i - 128) * cbu;
                CguTab[i] = (i - 128) * cgu;
                CgvTab[i] = (i - 128) * cgv;
                Tab76309[i] = 76309 * (i - 16);
            }

            for (i = 0; i < 384; i++)
                Clip[i] = 0;
            var ind = 384;
            for (i = 0; i < 256; i++)
                Clip[ind++] = (byte)i;
            ind = 640;
            for (i = 0; i < 384; i++)
                Clip[ind++] = 255;
        }


        public override Frame Process(Frame sourceFrame)
        {
            var width = sourceFrame.Width;
            var height = sourceFrame.Height;

            var destinationFrame = new RgbFrame(width, height, PixelFormatType.PIX_FMT_RGB24);

            var ySourceIndex1 = 0;
            var ySourceIndex2 = ySourceIndex1 + width;

            var destinationIndex1 = 0;
            var destinationIndex2 = destinationIndex1 + 3 * width;

            for (int j = 0, k = 0; j < height; j += 2)
            {
                for (var i = 0; i < width; i += 2)
                {
                    int u = sourceFrame[1][k];
                    int v = sourceFrame[2][k];

                    var c1 = (int)CrvTab[v];
                    var c2 = (int)CguTab[u];
                    var c3 = (int)CgvTab[v];
                    var c4 = (int)CbuTab[u];

                    //up-left
                    var y1 = (int)Tab76309[sourceFrame[0][ySourceIndex1]];
                    ySourceIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 + c1) >> 16)];
                    destinationIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 - c2 - c3) >> 16)];
                    destinationIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 + c4) >> 16)];
                    destinationIndex1++;

                    //down-left
                    var y2 = (int)Tab76309[sourceFrame[0][ySourceIndex2]];
                    ySourceIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 + c1) >> 16)];
                    destinationIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 - c2 - c3) >> 16)];
                    destinationIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 + c4) >> 16)];
                    destinationIndex2++;

                    //up-right
                    y1 = (int)Tab76309[sourceFrame[0][ySourceIndex1]];
                    ySourceIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 + c1) >> 16)];
                    destinationIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 - c2 - c3) >> 16)];
                    destinationIndex1++;
                    destinationFrame.Data[destinationIndex1] = Clip[384 + ((y1 + c4) >> 16)];
                    destinationIndex1++;

                    //down-right
                    y2 = (int)Tab76309[sourceFrame[0][ySourceIndex2]];
                    ySourceIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 + c1) >> 16)];
                    destinationIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 - c2 - c3) >> 16)];
                    destinationIndex2++;
                    destinationFrame.Data[destinationIndex2] = Clip[384 + ((y2 + c4) >> 16)];
                    destinationIndex2++;

                    k++;
                }
                destinationIndex1 += 3 * width;
                destinationIndex2 += 3 * width;
                ySourceIndex1 += width;
                ySourceIndex2 += width;
            }

            return destinationFrame;
        }
    }
}