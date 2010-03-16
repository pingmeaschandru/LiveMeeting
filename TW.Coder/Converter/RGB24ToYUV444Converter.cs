namespace TW.Coder.Converter
{
    public class RGB24ToYUV444Converter : ConverterBase
    {
        protected static readonly int[] RGB2_YUV_YR = new int[256];
        protected static readonly int[] RGB2_YUV_YG = new int[256];
        protected static readonly int[] RGB2_YUV_YB = new int[256];
        protected static readonly int[] RGB2_YUV_UR = new int[256];
        protected static readonly int[] RGB2_YUV_UG = new int[256];
        protected static readonly int[] RGB2_YUV_UBVR = new int[256];
        protected static readonly int[] RGB2_YUV_VG = new int[256];
        protected static readonly int[] RGB2_YUV_VB = new int[256];

        static RGB24ToYUV444Converter()
        {
            for (var i = 0; i < 256; i++) RGB2_YUV_YR[i] = (int)((float)65.481 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_YG[i] = (int)((float)128.553 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_YB[i] = (int)((float)24.966 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_UR[i] = (int)((float)37.797 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_UG[i] = (int)((float)74.203 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_VG[i] = (int)((float)93.786 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_VB[i] = (int)((float)18.214 * (i << 8));
            for (var i = 0; i < 256; i++) RGB2_YUV_UBVR[i] = (int)((float)112 * (i << 8));
        }

        public override Frame Process(Frame sourceFrame)
        {
            var width = sourceFrame.Width;
            var height = sourceFrame.Height;

            var rSourceIndex = 0;
            var gSourceIndex = rSourceIndex + 1;
            var bSourceIndex = rSourceIndex + 2;

            var destFrame = new YuvFrame(width, height, PixelFormatType.PIX_FMT_YUV444);
            for (int i = 0, k = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    destFrame[0][k] = (byte)((RGB2_YUV_YR[sourceFrame.Data[rSourceIndex]] + RGB2_YUV_YG[sourceFrame.Data[gSourceIndex]] + RGB2_YUV_YB[sourceFrame.Data[bSourceIndex]] + 1048576) >> 16);
                    destFrame[1][k] = (byte)((-RGB2_YUV_UR[sourceFrame.Data[rSourceIndex]] - RGB2_YUV_UG[sourceFrame.Data[gSourceIndex]] + RGB2_YUV_UBVR[sourceFrame.Data[bSourceIndex]] + 8388608) >> 16);
                    destFrame[2][k] = (byte)((RGB2_YUV_UBVR[sourceFrame.Data[rSourceIndex]] - RGB2_YUV_VG[sourceFrame.Data[gSourceIndex]] - RGB2_YUV_VB[sourceFrame.Data[bSourceIndex]] + 8388608) >> 16);

                    rSourceIndex += 3;
                    gSourceIndex += 3;
                    bSourceIndex += 3;

                    k++;
                }
            }

            return destFrame;
        }
    }
}