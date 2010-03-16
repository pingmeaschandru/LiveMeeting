using TW.H264Coder.Vcl.Block;

namespace TW.H264Coder.Vcl.MacroblockImpl
{
    public class MacroblockInfo
    {
        private readonly int mbNr;
        private readonly MacroblockAccess access;
        private ResidualBlockInfo lumaDcBlk;
        private readonly ResidualBlockInfo[][] lumaAcBlk;
        private ResidualBlockInfo chromaDcBlk;
        private readonly ResidualBlockInfo[][] chromaAcBlk;

        public MacroblockInfo(Macroblock macroblock)
        {
            mbNr = macroblock.MbNr;
            access = macroblock.MacroblockAccess;
            lumaAcBlk = new ResidualBlockInfo[4][];
            for (var i = 0; i < lumaAcBlk.Length; i++)
                lumaAcBlk[i] = new ResidualBlockInfo[4];

            chromaAcBlk = new ResidualBlockInfo[2][];
            for (var i = 0; i < chromaAcBlk.Length; i++)
                chromaAcBlk[i] = new ResidualBlockInfo[2];
        }

        public ResidualBlockInfo GetLumaAcBlockInfo(int blkIdxX, int blkIdxY)
        {
            return lumaAcBlk[blkIdxX][blkIdxY];
        }

        public ResidualBlockInfo GetChromaAcBlockInfo(int blkIdxX, int blkIdxY)
        {
            return chromaAcBlk[blkIdxX][blkIdxY];
        }

        public void SetLumaAcBlockInfo(int blkIdxX, int blkIdxY, ResidualBlockInfo blockInfo)
        {
            blockInfo.MacroblockInfo = this;
            blockInfo.BlockIndexX = blkIdxX;
            blockInfo.BlockIndexY = blkIdxY;

            lumaAcBlk[blkIdxX][blkIdxY] = blockInfo;
        }

        public void SetChromaAcBlockInfo(int blkIdxX, int blkIdxY, ResidualBlockInfo blockInfo)
        {
            blockInfo.MacroblockInfo = this;
            blockInfo.BlockIndexX = blkIdxX;
            blockInfo.BlockIndexY = blkIdxY;

            chromaAcBlk[blkIdxX][blkIdxY] = blockInfo;
        }

        public int GetPredictedNonZeroCoeff(int blkIdxX, int blkIdxY)
        {
            Macroblock mbA = null;
            Macroblock mbB = null;
            bool isUpperBlockAvailable;
            bool isLeftBlockAvailable;
            int nA, nB, nC;

            // Check the availability of the left block (block A)
            if (blkIdxX == 0)
            {
                if (access.Column(mbNr) != 0)
                {
                    mbA = access.MacroblockA;
                    isLeftBlockAvailable = true;
                }
                else
                    isLeftBlockAvailable = false;
            }
            else
            {
                mbA = access.GetMacroblock(mbNr);
                isLeftBlockAvailable = true;
            }

            // Check the availability of the upper block (block B)
            if (blkIdxY == 0)
            {
                if (access.Line(mbNr) != 0)
                {
                    mbB = access.MacroblockB;
                    isUpperBlockAvailable = true;
                }
                else
                    isUpperBlockAvailable = false;
            }
            else
            {
                mbB = access.GetMacroblock(mbNr);
                isUpperBlockAvailable = true;
            }

            var blkIdxXa = (blkIdxX != 0) ? blkIdxX - 1 : 3;
            var blkIdxYa = blkIdxY;
            var blkIdxXb = blkIdxX;
            var blkIdxYb = (blkIdxY != 0) ? blkIdxY - 1 : 3;

            // Compute nC
            if (!isLeftBlockAvailable && isUpperBlockAvailable)
            {
                var blockB = mbB.MacroblockInfo.GetLumaAcBlockInfo(blkIdxXb,blkIdxYb);
                nB = blockB.NonZeroCoeff;
                nC = nB;
            }
            else if (isLeftBlockAvailable && !isUpperBlockAvailable)
            {
                var blockA = mbA.MacroblockInfo.GetLumaAcBlockInfo(blkIdxXa,blkIdxYa);
                nA = blockA.NonZeroCoeff;
                nC = nA;
            }
            else if (isLeftBlockAvailable)
            {
                var blockA = mbA.MacroblockInfo.GetLumaAcBlockInfo(blkIdxXa, blkIdxYa);
                var blockB = mbB.MacroblockInfo.GetLumaAcBlockInfo(blkIdxXb, blkIdxYb);
                nA = blockA.NonZeroCoeff;
                nB = blockB.NonZeroCoeff;
                nC = (nA + nB + 1) >> 1;
            }
            else
                nC = 0;

            // TODO System.err.printf("[GetPredictedNonZeroCoeff] (blkIdxX =%d, blkIdxY = %d) nC = %d\n", blkIdxX, blkIdxY, nC);

            return nC;
        }

        public ResidualBlockInfo LumaDcBlockInfo
        {
            get { return lumaDcBlk; }
            set
            {
                value.MacroblockInfo = this;
                value.BlockIndexX = 0;
                value.BlockIndexY = 0;

                lumaDcBlk = value;
            }
        }

        public ResidualBlockInfo ChromaDcBlockInfo
        {
            get { return chromaDcBlk; }
            set
            {
                value.MacroblockInfo = this;
                value.BlockIndexX = 0;
                value.BlockIndexY = 0;

                chromaDcBlk = value;
            }
        }
    }
}