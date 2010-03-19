using System;
using TW.Core.Native;

namespace TW.WaveLib
{
    public delegate void BufferDoneEventHandler(IntPtr data, int size);

    internal class WaveInHelper
    {
        public static void Try(int err)
        {
            if (err != Winmm.MMSYSERR_NOERROR)
                throw new Exception(err.ToString());
        }
    }
}