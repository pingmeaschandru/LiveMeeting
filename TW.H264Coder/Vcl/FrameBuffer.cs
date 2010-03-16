using System.Drawing;

namespace TW.H264Coder.Vcl
{
    public abstract class FrameBuffer 
    {
        private Size frameSize;

        protected FrameBuffer(Size frameSize)
        {
            this.frameSize = frameSize;
        }

        public int Width
        {
            get { return frameSize.Width; }
        }

        public int Height
        {
            get { return frameSize.Height;  }
        }
    }
}