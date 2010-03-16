using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;

namespace TW.H264Coder.Vcl.Mode
{
    public interface IEncodingMode 
    {
        void Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer);
        void Reconstruct(YuvFrameBuffer outFrameBuffer);
        int GetDistortion();
        void Write(IH264EntropyOutputStream outStream);
        MacroblockType MbType { get; }
        MacroblockInfo LumaMacroblockInfo { get; }
        MacroblockInfo ChromaMacroblockInfo { get; }
    }
}