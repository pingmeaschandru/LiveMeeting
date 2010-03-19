using System;
using TW.Core.Native;

namespace TW.WaveLib
{
    public delegate void BufferFillEventHandler(IntPtr data, int size);

    internal class WaveOutHelper
    {
        public static void Try(int err)
        {
            if (err != Winmm.MMSYSERR_NOERROR)
                throw new Exception(err.ToString());
        }
    }
}