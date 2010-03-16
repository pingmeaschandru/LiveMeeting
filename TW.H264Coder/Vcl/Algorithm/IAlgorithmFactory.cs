using TW.H264Coder.Vcl.Mode.Decision;

namespace TW.H264Coder.Vcl.Algorithm
{
    public interface IAlgorithmFactory
    {
        ITransform CreateTransform();
        Quantizer CreateQuantizer();
        IScanner CreateScanner();
        IDistortionMetric CreateDistortionMetric();
    }
}