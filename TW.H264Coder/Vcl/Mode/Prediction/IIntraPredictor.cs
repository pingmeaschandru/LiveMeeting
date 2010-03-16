using TW.H264Coder.IO;
using TW.H264Coder.Vcl.MacroblockImpl;

namespace TW.H264Coder.Vcl.Mode.Prediction
{
    public interface IIntraPredictor 
    {
        bool Predict(YuvFrameBuffer origiFrameBuffer, YuvFrameBuffer codedFrameBuffer);
        int Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer);
        void Reconstruct(YuvFrameBuffer outFrameBuffer);
        void Write(IH264EntropyOutputStream outStream, int codedBlockPattern);
        int GetDistortion();
        MacroblockInfo GetMacroblockInfo();
    }
}