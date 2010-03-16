using System;

namespace TW.Coder
{
    public class YuvFrame : Frame
    {
        private int noOfChannels;
        private byte xChromaShift;
        private byte yChromaShift;
        private FrameSegment[] yuvFrameSegments;
        private int depth;

        public YuvFrame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType)
            : base(frameWidth, frameHeight, pixelFormatType)
        {
            GetPixelInfo();
            CreateYuvDataSegments();
        }


        public YuvFrame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType, byte[] data)
            : base(frameWidth, frameHeight, pixelFormatType, data)
        {
            GetPixelInfo();
            CreateYuvDataSegments();
        }


        protected void CreateYuvDataSegments()
        {
            if (noOfChannels == 3)
            {
                var startIndex = 0;
                var width2 = (Width + (1 << xChromaShift) - 1) >> xChromaShift;
                var height2 = (Height + (1 << yChromaShift) - 1) >> yChromaShift;

                var length = (Width * Height) + (2 * (width2 * height2));

                if (Data == null)
                    Data = new byte[length];
                else if (Data.Length != length)
                    throw new InvalidFrameDataException();

                yuvFrameSegments = new FrameSegment[noOfChannels];
                yuvFrameSegments[0] = new FrameSegment(Data, startIndex, Width * Height, depth);
                startIndex = startIndex + (Width * Height);
                yuvFrameSegments[1] = new FrameSegment(Data, startIndex, width2 * height2, depth);
                startIndex = startIndex + (width2 * height2);
                yuvFrameSegments[2] = new FrameSegment(Data, startIndex, width2 * height2, depth);
            }
            else
                throw new Exception();
        }

        private void GetPixelInfo()
        {
            switch (PixelFormatType)
            {
                case PixelFormatType.PIX_FMT_YUV410:
                    noOfChannels = 3;
                    xChromaShift = 2;
                    yChromaShift = 2;
                    depth = 1;
                    break;
                case PixelFormatType.PIX_FMT_YUV411:
                    noOfChannels = 3;
                    xChromaShift = 2;
                    yChromaShift = 0;
                    depth = 1;
                    break;
                case PixelFormatType.PIX_FMT_YUV420:
                    noOfChannels = 3;
                    xChromaShift = 1;
                    yChromaShift = 1;
                    depth = 1;
                    break;
                case PixelFormatType.PIX_FMT_YUV422:
                    noOfChannels = 3;
                    xChromaShift = 1;
                    yChromaShift = 0;
                    depth = 1;
                    break;
                case PixelFormatType.PIX_FMT_YUV440:
                    noOfChannels = 3;
                    xChromaShift = 0;
                    yChromaShift = 1;
                    depth = 1;
                    break;
                case PixelFormatType.PIX_FMT_YUV444:
                    noOfChannels = 3;
                    xChromaShift = 0;
                    yChromaShift = 0;
                    depth = 1;
                    break;
            }
        }

        public override FrameSegment this[int i]
        {
            get { return yuvFrameSegments[i]; }
        }

        public override int NoOfChannels
        {
            get { return noOfChannels; }
        }
    }
}