using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class WaveFormatEx
    {
        public short wFormatTag;
        public short nChannels;
        public int nSamplesPerSec;
        public int nAvgBytesPerSec;
        public short nBlockAlign;
        public short wBitsPerSample;
        public short cbSize;
    }
}