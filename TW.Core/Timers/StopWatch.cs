using System;
using TW.Core.Native;

namespace TW.Core.Timers
{
    public class StopWatch
    {
        private long totalCount;
        private long startCount;
        private long stopCount;
        private long frequency;

        public void Start()
        {
            lock (this)
            {
                startCount = 0;
                Kernel32.QueryPerformanceCounter(ref startCount);
            }
        }
	
        public void Stop()
        {
            lock (this)
            {
                stopCount = 0;
                Kernel32.QueryPerformanceCounter(ref stopCount);
                totalCount += stopCount - startCount;
            }
        }

        public void Restart()
        {
            lock (this)
            {
                totalCount = 0;
                startCount = 0;
                Kernel32.QueryPerformanceCounter(ref startCount);
            }
        }

        public float TotalSeconds
        {
            get
            {
                lock (this)
                {
                    frequency = 0;
                    Kernel32.QueryPerformanceFrequency(ref frequency);
                    return (totalCount / (float)frequency);
                }
            }
        }
	
        public double MFlops(double total_flops)
        {
            return (total_flops / (1e6 * TotalSeconds));
        }
		
        public override string ToString()
        {
            return String.Format("{0:F3} seconds", TotalSeconds);
        }
    }
}