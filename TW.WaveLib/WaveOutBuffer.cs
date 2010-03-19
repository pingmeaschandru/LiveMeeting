using System;
using System.Threading;
using System.Runtime.InteropServices;
using TW.Core.Native;

namespace TW.WaveLib
{
    internal class WaveOutBuffer : IDisposable
    {
        public WaveOutBuffer NextBuffer;

        private readonly AutoResetEvent playEvent = new AutoResetEvent(false);
        private readonly IntPtr waveOut;

        private Winmm.WaveHdr header;
        private readonly byte[] headerData;
        private GCHandle headerHandle;
        private GCHandle headerDataHandle;

        private bool m_Playing;

        internal static void WaveOutProc(IntPtr hdrvr, int uMsg, int dwUser, ref Winmm.WaveHdr wavhdr, int dwParam2)
        {
            if (uMsg == Winmm.MM_WOM_DONE)
            {
                try
                {
                    var h = (GCHandle)wavhdr.dwUser;
                    var buf = (WaveOutBuffer)h.Target;
                    buf.OnCompleted();
                }
                catch
                {
                }
            }
        }

        public WaveOutBuffer(IntPtr waveOutHandle, int size)
        {
            waveOut = waveOutHandle;

            headerHandle = GCHandle.Alloc(header, GCHandleType.Pinned);
            header.dwUser = (IntPtr)GCHandle.Alloc(this);
            headerData = new byte[size];
            headerDataHandle = GCHandle.Alloc(headerData, GCHandleType.Pinned);
            header.lpData = headerDataHandle.AddrOfPinnedObject();
            header.dwBufferLength = size;
            WaveOutHelper.Try(Winmm.waveOutPrepareHeader(waveOut, ref header, Marshal.SizeOf(header)));
        }
        ~WaveOutBuffer()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (header.lpData != IntPtr.Zero)
            {
                Winmm.waveOutUnprepareHeader(waveOut, ref header, Marshal.SizeOf(header));
                headerHandle.Free();
                header.lpData = IntPtr.Zero;
            }
            playEvent.Close();
            if (headerDataHandle.IsAllocated)
                headerDataHandle.Free();
            GC.SuppressFinalize(this);
        }

        public int Size
        {
            get { return header.dwBufferLength; }
        }

        public IntPtr Data
        {
            get { return header.lpData; }
        }

        public bool Play()
        {
            lock (this)
            {
                playEvent.Reset();
                m_Playing = Winmm.waveOutWrite(waveOut, ref header, Marshal.SizeOf(header)) == Winmm.MMSYSERR_NOERROR;
                return m_Playing;
            }
        }
        public void WaitFor()
        {
            if (m_Playing)
            {
                m_Playing = playEvent.WaitOne();
            }
            else
            {
                Thread.Sleep(0);
            }
        }
        public void OnCompleted()
        {
            playEvent.Set();
            m_Playing = false;
        }
    }
}