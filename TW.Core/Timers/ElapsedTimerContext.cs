namespace TW.Core.Timers
{
    public class ElapsedTimerContext
    {
        public ElapsedTimerContext(int expiryTime)
        {
            ExpiryTime = expiryTime;
            StopWatchObj = new StopWatch();
        }

        public bool IsExpired()
        {
            return (ElapsedTime > ExpiryTime);
        }

        public StopWatch StopWatchObj { get; private set; }
        public int ExpiryTime { get; private set; }

        private long ElapsedTime
        {
            get
            {
                StopWatchObj.Stop();
                var elapTime = (long)StopWatchObj.TotalSeconds;
                StopWatchObj.Start();

                return elapTime;
            }
        }
    }
}