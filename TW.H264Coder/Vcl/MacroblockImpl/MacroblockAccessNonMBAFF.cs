using System.Drawing;

namespace TW.H264Coder.Vcl.MacroblockImpl
{
    public class MacroblockAccessNonMbaff : MacroblockAccess
    {
        public MacroblockAccessNonMbaff(Macroblock macroblock)
            : base(macroblock)
        {
        }

        public override void CheckAvailableNeighbours()
        {
            var mbNrA = mbNr - 1;
            var mbNrB = mbNr - picWidthInMbs;
            var mbNrC = mbNr - picWidthInMbs + 1;
            var mbNrD = mbNr - picWidthInMbs - 1;

            if (Column(mbNr) != 0) macroblockA = GetMacroblock(mbNrA);
            macroblockB = GetMacroblock(mbNrB);
            if (Column(mbNr + 1) != 0) macroblockC = GetMacroblock(mbNrC);
            if (Column(mbNr) != 0) macroblockD = GetMacroblock(mbNrD);
        }

        public override bool GetNeighbour(int xN, int yN, int maxW, int maxH, Point p)
        {
            Macroblock neighbour = null;

            if ((xN < 0) && (yN < 0))
                neighbour = macroblockD;
            else if ((xN < 0) && ((yN >= 0) && (yN < maxH)))
                neighbour = macroblockA;
            else if (((xN >= 0) && (xN < maxW)) && (yN < 0))
                neighbour = macroblockB;
            else if (((xN >= 0) && (xN < maxW)) && ((yN >= 0) && (yN < maxH)))
                neighbour = macroblockX;
            else if ((xN >= maxW) && (yN < 0))
                neighbour = macroblockC;

            if (neighbour == null) return false;

            var xW = xN & (maxW - 1); // xW = (xN + maxW) % maxW
            var yW = yN & (maxH - 1); // yW = (yN + maxH) % maxH
            var posX = xW + Column(neighbour.MbNr)*maxW;
            var posY = yW + Line(neighbour.MbNr)*maxH;
            p.X = posX;
            p.Y = posY;

            return true;
        }
    }
}