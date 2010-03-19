using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    public class Winmm
    {
        public enum WaveFormats
        {
            Pcm = 1,
            Float = 3
        }

        [StructLayout(LayoutKind.Sequential)]
        public class WaveFormat
        {
            public short wFormatTag;
            public short nChannels;
            public int nSamplesPerSec;
            public int nAvgBytesPerSec;
            public short nBlockAlign;
            public short wBitsPerSample;
            public short cbSize;

            public WaveFormat(int rate, int bits, int channels)
            {
                wFormatTag = (short)WaveFormats.Pcm;
                nChannels = (short)channels;
                nSamplesPerSec = rate;
                wBitsPerSample = (short)bits;
                cbSize = 0;

                nBlockAlign = (short)(channels * (bits / 8));
                nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
            }
        }

        public const int MMSYSERR_NOERROR = 0;
        public const int MM_WIM_DATA = 0x3C0;
        public const int MM_WOM_OPEN = 0x3BB;
        public const int MM_WOM_CLOSE = 0x3BC;
        public const int MM_WOM_DONE = 0x3BD;
        public const int CALLBACK_FUNCTION = 0x00030000;
        public const int TIME_MS = 0x0001; 
        public const int TIME_SAMPLES = 0x0002; 
        public const int TIME_BYTES = 0x0004; 

        public delegate void WaveDelegate(IntPtr hdrvr, int uMsg, int dwUser, ref WaveHdr wavhdr, int dwParam2);

        [StructLayout(LayoutKind.Sequential)]
        public struct WaveHdr
        {
            public IntPtr lpData; 
            public int dwBufferLength; 
            public int dwBytesRecorded; 
            public IntPtr dwUser; 
            public int dwFlags; 
            public int dwLoops; 
            public IntPtr lpNext;
            public int reserved;
        }

        [DllImport("winmm.dll")]
        public static extern int waveOutGetNumDevs();
        [DllImport("winmm.dll")]
        public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveOutWrite(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveOutOpen(out IntPtr hWaveOut, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
        [DllImport("winmm.dll")]
        public static extern int waveOutReset(IntPtr hWaveOut);
        [DllImport("winmm.dll")]
        public static extern int waveOutClose(IntPtr hWaveOut);
        [DllImport("winmm.dll")]
        public static extern int waveOutPause(IntPtr hWaveOut);
        [DllImport("winmm.dll")]
        public static extern int waveOutRestart(IntPtr hWaveOut);
        [DllImport("winmm.dll")]
        public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);


        [DllImport("winmm.dll")]
        public static extern int waveInGetNumDevs();
        [DllImport("winmm.dll")]
        public static extern int waveInAddBuffer(IntPtr hwi, ref WaveHdr pwh, int cbwh);
        [DllImport("winmm.dll")]
        public static extern int waveInClose(IntPtr hwi);
        [DllImport("winmm.dll")]
        public static extern int waveInOpen(out IntPtr phwi, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
        [DllImport("winmm.dll")]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
        [DllImport("winmm.dll")]
        public static extern int waveInReset(IntPtr hwi);
        [DllImport("winmm.dll")]
        public static extern int waveInStart(IntPtr hwi);
        [DllImport("winmm.dll")]
        public static extern int waveInStop(IntPtr hwi);

    }
}
