using System.Drawing;

namespace TW.H264Coder.Vcl.MacroblockImpl
{
    public abstract class MacroblockAccess
    {
        protected Macroblock macroblockX; // current macroblock
        protected Macroblock macroblockA; // left
        protected Macroblock macroblockB; // upper
        protected Macroblock macroblockC; // upper-right
        protected Macroblock macroblockD; // upper-left
        protected int mbNr;
        protected int frameWidth;
        protected int picWidthInMbs;
        protected int picSizeInMbs;

        protected MacroblockAccess(Macroblock macroblock)
        {
            macroblockX = macroblock;
            mbNr = macroblockX.MbNr;
            frameWidth = macroblockX.Slice.Picture.Width;
            picWidthInMbs = frameWidth / Macroblock.MbWidth;
            picSizeInMbs = macroblockX.Slice.Picture.PicSizeInMbs;
        }

        public abstract void CheckAvailableNeighbours();
        public abstract bool GetNeighbour(int xN, int yN, int maxW, int maxH, Point p);

        public bool IsUpAvailable(int maxW)
        {
            var p = new Point();
            const int maxH = 0;
            return GetNeighbour(0, -1, maxW, maxH, p);
        }

        public bool IsLeftAvailable(int maxH)
        {
            var p = new Point();
            const int maxW = 0;

            return GetNeighbour(-1, 0, maxW, maxH, p);
        }

        public bool IsLeftUpAvailable()
        {
            var p = new Point();
            const int maxW = 0;
            const int maxH = 0;
            return GetNeighbour(-1, -1, maxW, maxH, p);
        }

        public Macroblock GetMacroblock(int mbNrVal)
        {
            if ((mbNrVal < 0) || (mbNrVal > (picSizeInMbs - 1)))
                return null;

            return macroblockX.Slice.Macroblocks[mbNrVal];
        }

        public int Column(int mbNrVal)
        {
            return (mbNrVal%picWidthInMbs);
        }

        public int Line(int mbNrVal)
        {
            return (mbNrVal/picWidthInMbs);
        }

        public Macroblock MacroblockA
        {
            get { return macroblockA; }
        }

        public Macroblock MacroblockB
        {
            get { return macroblockB; }
        }

        public Macroblock MacroblockC
        {
            get { return macroblockC; }
        }

        public Macroblock MacroblockD
        {
            get { return macroblockD; }
        }
    }
}