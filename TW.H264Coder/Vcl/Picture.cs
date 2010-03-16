using System;
using System.Collections.Generic;
using TW.H264Coder.Vcl.Datatype;

namespace TW.H264Coder.Vcl
{
    public abstract class Picture
    {
        private Slice slice; // TODO only one slice for now
        protected int width;
        protected int height;
        private readonly int picSizeInMbs; // can be half of FrameSizeInMbs for field
        private int currentMbNr;
        private readonly int number; // absolute frame index for all pictures
        private readonly int frameNum; // frame index relative to the last IDR picture
        private readonly bool idrFlag;
        private readonly int qp;
        private readonly SliceType type;

        protected Picture(VideoSequence videoSequence, int pictureNumber, int lastIdr)
        {
            number = pictureNumber;
            idrFlag = (pictureNumber == lastIdr);
            frameNum = idrFlag ? 0 : pictureNumber - lastIdr;
            qp = 28; // TODO QPISlice QP for I Slices must come from Control
            picSizeInMbs = videoSequence.FrameSizeInMbs; // frame picture
            width = videoSequence.FrameSize.Width;
            height = videoSequence.FrameSize.Height;
            type = SliceType.Slice; // TODO always set to Intra
            InitFrame();
        }

        private static void InitFrame()
        {
            // Set quantizer parameter for I-frame
        }

        public void Encode(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            CodePicture(inFrameBuffer, outFrameBuffer);
        }

        protected void CodePicture(YuvFrameBuffer inFrameBuffer, YuvFrameBuffer outFrameBuffer)
        {
            var numberOfCodedMBs = 0;

            // TODO FmoInit()
            // TODO FmoStartPicture() -> picture level initialization of FMO

            CalculateQuantParam();

            // loop over slices
            while (numberOfCodedMBs < picSizeInMbs)
            {
                slice = new Slice(this, type);
                numberOfCodedMBs += slice.Encode(inFrameBuffer, outFrameBuffer);
            }
        }

        private static void CalculateQuantParam()
        {
        }

        public List<Slice> Slices
        {
            get
            {
                var slices = new List<Slice> {slice};
                return slices;
            }
        }

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public int PicSizeInMbs
        {
            get { return picSizeInMbs; }
        }

        public int CurrentMbNr
        {
            get { return currentMbNr; }
        }

        public int Number
        {
            get { return number; }
        }

        public int FrameNum
        {
            get { return frameNum; }
        }

        public bool IsIDR
        {
            get { return idrFlag; }
        }

        public int Qp
        {
            get { return qp; }
        }

        public SliceType TypeOfSlice
        {
            get
            {
                const int sameSlicetypeForWholeFrame = 5;
                return ((SliceType) (sameSlicetypeForWholeFrame + ((int) type)));
                    //SliceType.valueOf(type.value() + sameSlicetypeForWholeFrame);
            }
        }
    }
}