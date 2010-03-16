namespace TW.H264Coder.Vcl
{
    public class Frame : Picture 
    {
        public Frame(VideoSequence videoSequence, int frameNumber, int lastIdr)
            :base(videoSequence, frameNumber, lastIdr)
        {
        }
    }
}