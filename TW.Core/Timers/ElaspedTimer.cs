using System;
using System.Timers;

namespace TW.Core.Timers
{
    public class ElaspedTimer
    {
        private readonly Action elaspedHandler;
        private Timer sessionElapseTimer;
        private volatile bool synTimerLock;

        public ElaspedTimer(int interval, Action elaspedHandler)
        {
            this.elaspedHandler = elaspedHandler;

            sessionElapseTimer = new Timer {Interval = interval};
            sessionElapseTimer.Elapsed += OnsessionElapsed;

            synTimerLock = false;

            sessionElapseTimer.Start();
        }

        private void OnsessionElapsed(object sender, ElapsedEventArgs e)
        {
            if (synTimerLock)
                return;

            synTimerLock = true;

            elaspedHandler();

            synTimerLock = false;
        }

        public void Close()
        {
            if (sessionElapseTimer == null) return;

            sessionElapseTimer.Stop();
            sessionElapseTimer.Elapsed -= OnsessionElapsed;
            sessionElapseTimer.Dispose();
            sessionElapseTimer = null;
        }
    }
}