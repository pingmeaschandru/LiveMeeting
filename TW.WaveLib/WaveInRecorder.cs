using System;
using System.Threading;
using TW.Core.Native;

namespace TW.WaveLib
{
    public class WaveInRecorder : IDisposable
    {
        private IntPtr waveIn;
        private WaveInBuffer buffers;
        private WaveInBuffer currentBuffer;
        private Thread recordingThread;
        private BufferDoneEventHandler doneProc;
        private bool finished;

        private readonly Winmm.WaveDelegate bufferProc = WaveInBuffer.WaveInProc;

        public static int DeviceCount
        {
            get { return Winmm.waveInGetNumDevs(); }
        }

        public WaveInRecorder(int device, Winmm.WaveFormat format, int bufferSize, int bufferCount, BufferDoneEventHandler doneProc)
        {
            this.doneProc = doneProc;
            WaveInHelper.Try(Winmm.waveInOpen(out waveIn, device, format, bufferProc, 0, Winmm.CALLBACK_FUNCTION));
            AllocateBuffers(bufferSize, bufferCount);
            for (var i = 0; i < bufferCount; i++)
            {
                SelectNextBuffer();
                currentBuffer.Record();
            }
            WaveInHelper.Try(Winmm.waveInStart(waveIn));
            recordingThread = new Thread(ThreadProc);
            recordingThread.Start();
        }
        ~WaveInRecorder()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (recordingThread != null)
                try
                {
                    finished = true;
                    if (recordingThread.IsAlive)
                        recordingThread.Join();

                    WaitForAllBuffers();
                    doneProc = null;
                    FreeBuffers();

                    if (waveIn != IntPtr.Zero)
                    {
                        Winmm.waveInReset(waveIn);
                        Winmm.waveInClose(waveIn);
                    }
                }
                finally
                {
                    recordingThread = null;
                    waveIn = IntPtr.Zero;
                }
            GC.SuppressFinalize(this);
        }
        private void ThreadProc()
        {
            while (!finished)
            {
                Advance();
                if (doneProc != null && !finished)
                    doneProc(currentBuffer.Data, currentBuffer.Size);
                currentBuffer.Record();
            }
        }
        private void AllocateBuffers(int bufferSize, int bufferCount)
        {
            FreeBuffers();
            if (bufferCount > 0)
            {
                buffers = new WaveInBuffer(waveIn, bufferSize);
                var Prev = buffers;
                try
                {
                    for (var i = 1; i < bufferCount; i++)
                    {
                        var Buf = new WaveInBuffer(waveIn, bufferSize);
                        Prev.NextBuffer = Buf;
                        Prev = Buf;
                    }
                }
                finally
                {
                    Prev.NextBuffer = buffers;
                }
            }
        }
        private void FreeBuffers()
        {
            currentBuffer = null;
            if (buffers != null)
            {
                var First = buffers;
                buffers = null;

                var Current = First;
                do
                {
                    var Next = Current.NextBuffer;
                    Current.Dispose();
                    Current = Next;
                } while(Current != First);
            }
        }
        private void Advance()
        {
            SelectNextBuffer();
            currentBuffer.WaitFor();
        }
        private void SelectNextBuffer()
        {
            currentBuffer = currentBuffer == null ? buffers : currentBuffer.NextBuffer;
        }
        private void WaitForAllBuffers()
        {
            var Buf = buffers;
            while (Buf.NextBuffer != buffers)
            {
                Buf.WaitFor();
                Buf = Buf.NextBuffer;
            }
        }
    }
}