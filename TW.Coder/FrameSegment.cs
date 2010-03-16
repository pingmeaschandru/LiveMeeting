using System;

namespace TW.Coder
{
    public class FrameSegment
    {
        private readonly byte[] data;
        private readonly int startIndex;
        private readonly int count;
        private readonly int depth;

        public FrameSegment(byte[] data, int startIndex, int count, int depth)
        {
            this.data = data;
            this.startIndex = startIndex;
            this.count = count;
            this.depth = depth;
        }

        public void Copy(FrameSegment frameSegment)
        {
            if (Length < frameSegment.Length)
                throw new Exception();

            for (var i = 0; i < frameSegment.Length; i++)
                this[i] = frameSegment[i];
        }

        public int Length
        {
            get { return count; }
        }

        public byte this[int i]
        {
            get { return data[startIndex + (i*depth)]; }
            set { data[startIndex + (i*depth)] = value; }
        }
    }
}