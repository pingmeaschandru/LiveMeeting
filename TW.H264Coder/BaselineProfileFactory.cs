using TW.H264Coder.Vcl.Algorithm;
using TW.H264Coder.Vcl.Algorithm.Impl;
using TW.H264Coder.Vcl.Mode.Decision;

namespace TW.H264Coder
{
    public class BaselineProfileFactory : IAlgorithmFactory
    {
        private readonly ITransform transform;
        private readonly Quantizer quantizer;
        private readonly IScanner scanner;
        private readonly IDistortionMetric distortionMetric;

        public BaselineProfileFactory(int qp)
        {
            transform = new IntegerTransform();
            quantizer = new IntegerQuantizer(qp);
            scanner = new ZigZagFrameScanner();
            distortionMetric = new Satd(transform);
        }

        public Quantizer CreateQuantizer()
        {
            return quantizer;
        }

        public IScanner CreateScanner()
        {
            return scanner;
        }

        public ITransform CreateTransform()
        {
            return transform;
        }

        public IDistortionMetric CreateDistortionMetric()
        {
            return distortionMetric;
        }
    }
}