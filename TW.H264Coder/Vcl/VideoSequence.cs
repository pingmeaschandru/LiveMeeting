using System.Drawing;

namespace TW.H264Coder.Vcl
{
    public class VideoSequence
    {
        private const int MbBlockSize = 16;

        private Picture currentPicture;
        private int pictureIndex;
        private readonly YuvFrameBuffer currentInputFrame;
        private readonly YuvFrameBuffer currentOutputFrame;
        private readonly int picWidthInMbs;
        private readonly int frameHeightInMbs;
        private readonly int frameSizeInMbs;
        private readonly int lastIdr;
        private readonly Size frameSize;

        public VideoSequence(YuvFormatType yuvFormatType, Size frameSize ) 
        {
            this.frameSize = frameSize;
            currentInputFrame = new YuvFrameBuffer(yuvFormatType, frameSize);
            currentOutputFrame = new YuvFrameBuffer(yuvFormatType, frameSize);
            picWidthInMbs = frameSize.Width / MbBlockSize;
            frameHeightInMbs = frameSize.Height / MbBlockSize;
            frameSizeInMbs = picWidthInMbs*frameHeightInMbs;
            lastIdr = 0;
            pictureIndex = 0;
        }

        public Size FrameSize
        {
            get { return frameSize; }
        }

        public void Encode(byte[] inputFrameData)
        {
            currentInputFrame.CopyData(inputFrameData);
            currentPicture = new Frame(this, pictureIndex, lastIdr);
            currentPicture.Encode(currentInputFrame, currentOutputFrame);
            pictureIndex++;
        }

        public Picture GetCurrentPicture()
        {
            return currentPicture;
        }

        public int PicWidthInMbs
        {
            get { return picWidthInMbs; }
        }

        public int FrameHeightInMbs
        {
            get { return frameHeightInMbs; }
        }

        public int FrameSizeInMbs
        {
            get { return frameSizeInMbs; }
        }
    }
}