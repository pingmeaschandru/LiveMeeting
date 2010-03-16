using System;

namespace TW.Coder
{
    public class RgbFrame : Frame
    {   
        private int noOfChannels;
        private int depth;
        private FrameSegment[] rgbFrameSegments;

        public RgbFrame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType)
            : base(frameWidth, frameHeight, pixelFormatType)
        {
            GetPixelInfo();
            CreateRgbDataSegments();
        }


        public RgbFrame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType, byte[] data)
            : base(frameWidth, frameHeight, pixelFormatType, data)
        {
            GetPixelInfo();
            CreateRgbDataSegments();
        }

        private void CreateRgbDataSegments()
        {
            var length = noOfChannels * (Width * Height);

            if (Data == null)
                Data = new byte[length];
            else if (length != Data.Length)
                throw new InvalidFrameDataException();

            rgbFrameSegments = new FrameSegment[noOfChannels];
            for (int i = 0, startIndex = 0; i < rgbFrameSegments.Length; i++)
            {
                rgbFrameSegments[i] = new FrameSegment(Data, startIndex, Width*Height, depth);
                startIndex = startIndex + (Width*Height);
                startIndex++;
            }
        }

        private void GetPixelInfo()
        {
            switch (PixelFormatType)
            {
                case PixelFormatType.PIX_FMT_RGB24: 
                    noOfChannels = 3;
                    depth = 3;
                    break;
                case PixelFormatType.PIX_FMT_RGB32: 
                    noOfChannels = 4;
                    depth = 3;
                    break;
                default:
                    throw new Exception();
            }
        }

        public override int NoOfChannels
        {
            get { return noOfChannels; }
        }

        public override FrameSegment this[int i]
        {
            get { return rgbFrameSegments[i]; }
        }
    }
}