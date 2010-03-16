using System;
using System.Threading;

namespace TW.Core.Threading
{
    public abstract class AThread
    {
        private Thread aThread;
        protected bool running;

        protected AThread()
        {
            running = false;
        }

        protected abstract void OnRun();

        protected void start()
        {
            if (running) return;
            running = true;

            aThread = new Thread(pRun) {Name = "AThread # " + GetHashCode()};

            aThread.Start();
        }

        protected void stop()
        {
            if (running)
            {
                running = false;

                try
                {
                    aThread.Join(new TimeSpan(0, 1, 0));
                }
                catch (Exception)
                {
                }
                finally
                {
                    aThread = null;
                }
            }
        }

        protected bool isAlive()
        {
            return running;
        }

        private void pRun()
        {
            while (running)
            {
                try
                {
                    OnRun();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}