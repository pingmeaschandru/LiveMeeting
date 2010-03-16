namespace TW.Coder.Converter
{
    public class RGB24ToYUV420Converter : RGB24ToYUV444Converter
    {
        public override Frame Process(Frame sourceFrame)
        {
            var width = sourceFrame.Width;
            var height = sourceFrame.Height;
            
            var destIntermediateFrame = base.Process(sourceFrame);

            var destFrame = new YuvFrame(width, height, PixelFormatType.PIX_FMT_YUV420);
            destFrame[0].Copy(destIntermediateFrame[0]);

            // For U
            var uDestinationIndex1 = 0;
            var uDestinationIndex2 = uDestinationIndex1 + 1;
            var uDestinationIndex3 = uDestinationIndex1 + width;
            var uDestinationIndex4 = uDestinationIndex3 + 1;

            // For V
            var vDestinationIndex1 = 0;
            var vDestinationIndex2 = vDestinationIndex1 + 1;
            var vDestinationIndex3 = vDestinationIndex1 + width;
            var vDestinationIndex4 = vDestinationIndex3 + 1;

            for (int i = 0, k = 0; i < height; i += 2)
            {
                for (var j = 0; j < width; j += 2)
                {
                    destFrame[1][k] = (byte)((destIntermediateFrame[1][uDestinationIndex1] +
                                                 destIntermediateFrame[1][uDestinationIndex2] +
                                                 destIntermediateFrame[1][uDestinationIndex3] +
                                                 destIntermediateFrame[1][uDestinationIndex4]) >> 2);
                    destFrame[2][k] = (byte)((destIntermediateFrame[2][vDestinationIndex1] +
                                                 destIntermediateFrame[2][vDestinationIndex2] +
                                                 destIntermediateFrame[2][vDestinationIndex3] +
                                                 destIntermediateFrame[2][vDestinationIndex4]) >> 2);

                    uDestinationIndex1 += 2;
                    uDestinationIndex2 += 2;
                    uDestinationIndex3 += 2;
                    uDestinationIndex4 += 2;

                    vDestinationIndex1 += 2;
                    vDestinationIndex2 += 2;
                    vDestinationIndex3 += 2;
                    vDestinationIndex4 += 2;

                    k++;
                }

                uDestinationIndex1 += width;
                uDestinationIndex2 += width;
                uDestinationIndex3 += width;
                uDestinationIndex4 += width;

                vDestinationIndex1 += width;
                vDestinationIndex2 += width;
                vDestinationIndex3 += width;
                vDestinationIndex4 += width;
            }

            return destFrame;
        }
    }
}