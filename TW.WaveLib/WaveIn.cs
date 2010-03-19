using System;
using System.Threading;
using System.Runtime.InteropServices;
using TW.Core.Native;

namespace TW.WaveLib
{
    internal class WaveInBuffer : IDisposable
    {
        public WaveInBuffer NextBuffer;

        private readonly AutoResetEvent recordEvent = new AutoResetEvent(false);
        private readonly IntPtr waveIn;

        private Winmm.WaveHdr header;
        private readonly byte[] headerData;
        private GCHandle headerHandle;
        private GCHandle headerDataHandle;

        private bool recording;

        internal static void WaveInProc(IntPtr hdrvr, int uMsg, int dwUser, ref Winmm.WaveHdr wavhdr, int dwParam2)
        {
            if (uMsg == Winmm.MM_WIM_DATA)
            {
                try
                {
                    var h = (GCHandle)wavhdr.dwUser;
                    var buf = (WaveInBuffer)h.Target;
                    buf.OnCompleted();
                }
                catch
                {
                }
            }
        }

        public WaveInBuffer(IntPtr waveInHandle, int size)
        {
            waveIn = waveInHandle;

            headerHandle = GCHandle.Alloc(header, GCHandleType.Pinned);
            header.dwUser = (IntPtr)GCHandle.Alloc(this);
            headerData = new byte[size];
            headerDataHandle = GCHandle.Alloc(headerData, GCHandleType.Pinned);
            header.lpData = headerDataHandle.AddrOfPinnedObject();
            header.dwBufferLength = size;
            WaveInHelper.Try(Winmm.waveInPrepareHeader(waveIn, ref header, Marshal.SizeOf(header)));
        }
        ~WaveInBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (header.lpData != IntPtr.Zero)
            {
                Winmm.waveInUnprepareHeader(waveIn, ref header, Marshal.SizeOf(header));
                headerHandle.Free();
                header.lpData = IntPtr.Zero;
            }
            recordEvent.Close();
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

        public bool Record()
        {
            lock(this)
            {
                recordEvent.Reset();
                recording = Winmm.waveInAddBuffer(waveIn, ref header, Marshal.SizeOf(header)) == Winmm.MMSYSERR_NOERROR;
                return recording;
            }
        }

        public void WaitFor()
        {
            if (recording)
                recording = recordEvent.WaitOne();
            else
                Thread.Sleep(0);
        }

        private void OnCompleted()
        {
            recordEvent.Set();
            recording = false;
        }
    }
}