using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public class VideoInfoHeader
    {
        public DsRect SrcRect;
        public DsRect TargetRect;
        public int BitRate;
        public int BitErrorRate;
        public long AvgTimePerFrame;
        public BitmapInfoHeader BmiHeader;
    }
}