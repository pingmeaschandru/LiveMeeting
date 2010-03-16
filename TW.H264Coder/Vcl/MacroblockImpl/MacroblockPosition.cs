namespace TW.H264Coder.Vcl.MacroblockImpl
{
    public class MacroblockPosition
    {
        public int MbX { get; set; }
        public int MbY { get; set; }
        public int PixelX { get; set; }
        public int PixelY { get; set; }
        public int PixelChromaX { get; set; }
        public int PixelChromaY { get; set; }

        public MacroblockPosition(Macroblock macroblock)
        {
            var mbNr = macroblock.MbNr;
            var frameWidth = macroblock.Slice.Picture.Width;
            var picWidthInMbs = frameWidth / Macroblock.MbWidth;
            MbX = mbNr % picWidthInMbs;
            MbY = mbNr / picWidthInMbs;
            PixelX = MbX * Macroblock.MbWidth;
            PixelY = MbY * Macroblock.MbHeight;
            PixelChromaX = (MbX * Macroblock.MbChromaWidth);
            PixelChromaY = (MbY * Macroblock.MbChromaHeight);
        }
    }
}