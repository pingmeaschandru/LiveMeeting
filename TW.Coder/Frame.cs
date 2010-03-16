namespace TW.Coder
{
    public abstract class Frame
    {
        protected Frame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType, byte[] data)
        {
            PixelFormatType = pixelFormatType;
            Width = frameWidth;
            Height = frameHeight;
            Data = data;
        }

        protected Frame(int frameWidth, int frameHeight, PixelFormatType pixelFormatType)
            : this(frameWidth, frameHeight, pixelFormatType, null)
        {
        }

        public int Height { get; private set; }
        public int Width { get; private set; }
        public PixelFormatType PixelFormatType { get; protected set; }
        public byte[] Data { get; protected set; }
        public abstract int NoOfChannels {get;}
        public abstract FrameSegment this[int i] { get; }
    }
}