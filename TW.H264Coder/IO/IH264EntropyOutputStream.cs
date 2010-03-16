using TW.H264Coder.Nal;
using TW.H264Coder.ParameterSet;
using TW.H264Coder.Vcl.Entropy;

namespace TW.H264Coder.IO
{
    public interface IH264EntropyOutputStream : IEntropyOutputStream
    {
        void Flush();
        void Close();
        long Length { get; }
        Nalu Nalu { get; set; }
        SequenceParameterSet Sps { get; }
        PictureParameterSet Pps { get; }
    }
}