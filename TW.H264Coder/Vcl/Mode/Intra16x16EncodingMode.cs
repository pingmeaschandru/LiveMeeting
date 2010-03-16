using System.Collections.Generic;
using TW.H264Coder.IO;
using TW.H264Coder.Vcl.Algorithm;
using TW.H264Coder.Vcl.Datatype;
using TW.H264Coder.Vcl.MacroblockImpl;
using TW.H264Coder.Vcl.Mode.Prediction;

namespace TW.H264Coder.Vcl.Mode
{
    public class Intra16X16EncodingMode : AbstractEncodingMode
    {
        protected IIntraPredictor[] lumaModes;
        protected IIntraPredictor[] chromaModes;
        protected IIntraPredictor bestLumaMode;
        protected IIntraPredictor bestChromaMode;
        protected int bestLumaModeIdx;
        protected int bestChromaModeIdx;
        protected List<int> successPredLumaModes;
        protected List<int> successPredChromaModes;
        private int codedBlockPattern;

        public Intra16X16EncodingMode(Macroblock macroblock)
            : base(macroblock, MacroblockType.I16Mb)
        {
            codedBlockPattern = 0;

            var x = macroblock.PixelX;
            var y = macroblock.PixelY;
            var xc = macroblock.PixelChromaX;
            var yc = macroblock.PixelChromaY;
            const int qp = 28;
            IAlgorithmFactory algorithms = new BaselineProfileFactory(qp);

            lumaModes = new IIntraPredictor[4];
            lumaModes[0] = new Intra16X16LumaVerticalPredictor(x, y, macroblock, algorithms);
            lumaModes[1] = new Intra16X16LumaHorizontalPredictor(x, y, macroblock, algorithms);
            lumaModes[2] = new Intra16X16LumaDCPredictor(x, y, macroblock, algorithms);
            lumaModes[3] = new Intra16X16LumaPlanePredictor(x, y, macroblock, algorithms);

            chromaModes = new IIntraPredictor[4];
            chromaModes[0] = new Intra8X8ChromaDCPredictor(xc, yc, macroblock, algorithms);
            chromaModes[1] = new Intra8X8ChromaHorizontalPredictor(xc, yc, macroblock, algorithms);
            chromaModes[2] = new Intra8X8ChromaVerticalPredictor(xc, yc, macroblock, algorithms);
            chromaModes[3] = new Intra8X8ChromaPlanePredictor(xc, yc, macroblock, algorithms);

            successPredLumaModes = new List<int>();
            successPredChromaModes = new List<int>();
        }

        public override void Reconstruct(YuvFrameBuffer outFrameBuffer)
        {
            bestLumaMode.Reconstruct(outFrameBuffer);
            bestChromaMode.Reconstruct(outFrameBuffer);
        }

        protected override void DoEncode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer codedFrameBuffer)
        {
            for (var i = 0; i < lumaModes.Length; i++)
                if (lumaModes[i].Predict(inFrameBuffer, codedFrameBuffer))
                    successPredLumaModes.Add(i);

            var bestSad = int.MaxValue;
            int currentSad;
            foreach (var i in successPredLumaModes)
            {
                currentSad = lumaModes[i].GetDistortion();
                if (currentSad >= bestSad) continue;
                bestSad = currentSad;
                bestLumaMode = lumaModes[i];
                bestLumaModeIdx = i;
            }

            codedBlockPattern += bestLumaMode.Encode(inFrameBuffer, codedFrameBuffer);
            for (var i = 0; i < chromaModes.Length; i++)
                if (chromaModes[i].Predict(inFrameBuffer, codedFrameBuffer))
                    successPredChromaModes.Add(i);

            bestSad = int.MaxValue;
            for (var i = 0; i < successPredChromaModes.Count; i++)
            {
                currentSad = chromaModes[i].GetDistortion();
                if (currentSad >= bestSad) continue;
                bestSad = currentSad;
                bestChromaMode = chromaModes[i];
                bestChromaModeIdx = i;
            }

            codedBlockPattern += bestChromaMode.Encode(inFrameBuffer, codedFrameBuffer);
            ReleaseUnusedModes();
        }

        protected override void DoWrite(IH264EntropyOutputStream outStream)
        {
            var cbpL = codedBlockPattern % 16;
            var cbpC = codedBlockPattern / 16;
            var mbTypeValue = 1 + bestLumaModeIdx + 4 * cbpC + (((cbpL != 0) ? 1 : 0) * 12);
            outStream.WriteMbType(mbTypeValue);
            var intraChromaPredMode = bestChromaModeIdx;
            outStream.WriteIntraChromaPredMode(intraChromaPredMode);
            const int mbQpDelta = 0;
            outStream.WriteMbQpDelta(mbQpDelta);
            bestLumaMode.Write(outStream, codedBlockPattern);
            bestChromaMode.Write(outStream, codedBlockPattern);
        }

        private void ReleaseUnusedModes()
        {
            for (var i = 0; i < lumaModes.Length; i++)
                lumaModes[i] = null;

            lumaModes = null;

            for (var i = 0; i < chromaModes.Length; i++)
                chromaModes[i] = null;

            chromaModes = null;
        }

        public override MacroblockInfo LumaMacroblockInfo
        {
            get { return bestLumaMode.GetMacroblockInfo(); }
        }

        public override MacroblockInfo ChromaMacroblockInfo
        {
            get { return bestChromaMode.GetMacroblockInfo(); }
        }
    }
}