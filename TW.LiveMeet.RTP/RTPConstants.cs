namespace TW.LiveMeet.RTP
{
    public static class RTPConstants
    {
        public static int GetTimeStampDiff(uint startTS, uint endTS)
        {
            int diffTS;

            if (endTS < startTS)
                diffTS = (int)((0xFFFFFFFF - startTS) + endTS) + 1;
            else
                diffTS = (int)(endTS - startTS);

            return diffTS;
        }
    }
}