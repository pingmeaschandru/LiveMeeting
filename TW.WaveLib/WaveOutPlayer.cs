using System;
using System.Threading;
using System.Runtime.InteropServices;
using TW.Core.Native;

namespace TW.WaveLib
{
    public class WaveOutPlayer : IDisposable
    {
        private IntPtr waveOut;
        private WaveOutBuffer buffers;
        private WaveOutBuffer currentBuffer;
        private Thread playThread;
        private BufferFillEventHandler fillProc;
        private bool finished;
        private readonly byte zero;
        private readonly Winmm.WaveDelegate bufferProc = WaveOutBuffer.WaveOutProc;

        public static int DeviceCount
        {
            get { return Winmm.waveOutGetNumDevs(); }
        }

        public WaveOutPlayer(int device, WaveFormat format, int bufferSize, int bufferCount, BufferFillEventHandler fillProc)
        {
            zero = format.wBitsPerSample == 8 ? (byte)128 : (byte)0;
            this.fillProc = fillProc;
            WaveOutHelper.Try(Winmm.waveOutOpen(out waveOut, device, format, bufferProc, 0, Winmm.CALLBACK_FUNCTION));
            AllocateBuffers(bufferSize, bufferCount);
            playThread = new Thread(ThreadProc);
            playThread.Start();
        }

        ~WaveOutPlayer()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            if (playThread != null)
                try
                {
                    finished = true;                   
                    if (playThread.IsAlive)
                        playThread.Join();

                    fillProc = null;
                    FreeBuffers();
                    if (waveOut != IntPtr.Zero)
                    {
                        Winmm.waveOutReset(waveOut);
                        Winmm.waveOutClose(waveOut);
                    }
                }
                finally
                {
                    playThread = null;
                    waveOut = IntPtr.Zero;
                }
            GC.SuppressFinalize(this);
        }
        private void ThreadProc()
        {
            while (!finished)
            {
                Advance();
                if (fillProc != null && !finished)
                    fillProc(currentBuffer.Data, currentBuffer.Size);
                else
                {
                    var v = zero;
                    var b = new byte[currentBuffer.Size];
                    for (var i = 0; i < b.Length; i++) b[i] = v;

                    Marshal.Copy(b, 0, currentBuffer.Data, b.Length);
                }
                currentBuffer.Play();
            }
            WaitForAllBuffers();
        }
        private void AllocateBuffers(int bufferSize, int bufferCount)
        {
            FreeBuffers();
            if (bufferCount > 0)
            {
                buffers = new WaveOutBuffer(waveOut, bufferSize);
                var Prev = buffers;
                try
                {
                    for (var i = 1; i < bufferCount; i++)
                    {
                        var buffer = new WaveOutBuffer(waveOut, bufferSize);
                        Prev.NextBuffer = buffer;
                        Prev = buffer;
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
                } while (Current != First);
            }
        }
        private void Advance()
        {
            currentBuffer = currentBuffer == null ? buffers : currentBuffer.NextBuffer;
            currentBuffer.WaitFor();
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