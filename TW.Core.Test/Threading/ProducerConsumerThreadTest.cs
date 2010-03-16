using System.Threading;
using NUnit.Framework;

namespace TW.Core.Test.Threading
{
    [TestFixture]
    public class ProducerConsumerThreadTest
    {
        private readonly ProducerConsumerThreadMock pThread = new ProducerConsumerThreadMock();

        [Test]
        public void CheckProducerConsumerThread()
        {
            var eventReset = new AutoResetEvent(false);
            pThread.OnComplete += delegate
            {
                eventReset.Set();
            };
            pThread.Start();

            Assert.AreEqual(true, eventReset.WaitOne(50000));

            pThread.Stop();

            var flagEx = false;
            for (var i = 0; i < pThread.IntArray.Count; i++)
            {
                flagEx |= (i == pThread.IntArray[i]);
                if (flagEx == false)
                    break;
            }

            Assert.AreEqual(true, flagEx);
        }
    }
}