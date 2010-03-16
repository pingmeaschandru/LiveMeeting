using System;

namespace TW.Coder.Converter
{
    public class YUV444ToRGB24Converter : ConverterBase
    {
        public override Frame Process(Frame sourceFrame)
        {
            var width = sourceFrame.Width;
            var height = sourceFrame.Height;

            var destData = new RgbFrame(width, height, PixelFormatType.PIX_FMT_RGB24);

            for (int y = 0, k = 0, l = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cVal = sourceFrame[0][k] - 16;
                    var dVal = sourceFrame[1][k] - 128;
                    var eVal = sourceFrame[2][k] - 128;

                    destData.Data[l] = (byte)Clip(0, 255, (298 * cVal + 409 * eVal + 128) >> 8);
                    l++;
                    destData.Data[l] = (byte)Clip(0, 255, (298 * cVal - 100 * dVal - 208 * eVal + 128) >> 8);
                    l++;
                    destData.Data[l] = (byte)Clip(0, 255, (298 * cVal + 516 * dVal + 128) >> 8);
                    l++;

                    k++;
                }
            }

            return destData;
        }


        public static int Clip(int min, int max, double value)
        {
            value = Math.Ceiling(value);
            value = Math.Max(min, value);
            value = Math.Min(max, value);

            return (int)value;
        }
    }
}