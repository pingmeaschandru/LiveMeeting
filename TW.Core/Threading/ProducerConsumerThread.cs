using System;
using System.Collections.Generic;
using System.Threading;

namespace TW.Core.Threading
{
    public abstract class ProducerConsumerThread<T> where T : class
    {
        private const uint DEFAULT_SHARED_OBJECT_SIZE = 20;

        private Thread producerThread;
        private Thread consumerThread;
        private readonly IList<SharedObject<T>> sharedObjects;
        private bool running;

        protected ProducerConsumerThread()
            : this(DEFAULT_SHARED_OBJECT_SIZE)
        {
        }

        protected ProducerConsumerThread(uint poolSize)
        {
            poolSize = (poolSize > DEFAULT_SHARED_OBJECT_SIZE) ? poolSize : DEFAULT_SHARED_OBJECT_SIZE;

            sharedObjects = new List<SharedObject<T>>();
            for (long i = 0; i < poolSize; i++)
                sharedObjects.Add(new SharedObject<T>());

            running = false;
        }

        protected abstract T OnProduce();
        protected abstract void OnConsume(T obj);

        private void Produce()
        {
            var readPointer = 0;

            while (running)
            {
                try
                {
                    sharedObjects[readPointer%sharedObjects.Count].ObjectValue = OnProduce();
                    readPointer++;

                    if (readPointer >= sharedObjects.Count)
                        readPointer = 0;
                }
                catch
                {
                }
            }
        }

        private void Consume()
        {
            var readPointer = 0;

            while (running)
            {
                try
                {
                    var message = sharedObjects[readPointer % sharedObjects.Count].ObjectValue;
                    if (!Equals(message, default(T)))
                        OnConsume(message);

                    readPointer++;

                    if (readPointer >= sharedObjects.Count)
                        readPointer = 0;
                }
                catch
                {
                }
            }
        }

        public virtual bool IsAlive()
        {
            return running;
        }

        public virtual void Start()
        {
            if (running)
                return;

            running = true;

            producerThread = new Thread(Produce) { Name = "Producer Thread # " + GetHashCode() };
            consumerThread = new Thread(Consume) { Name = "Consumer Thread # " + GetHashCode() };

            consumerThread.Start();
            producerThread.Start();
        }

        public virtual void Stop()
        {
            if (!running)
                return;

            running = false;

            foreach (var obj in sharedObjects)
                obj.Clear();
            
            try
            {
                producerThread.Join(new TimeSpan(0, 1, 0));
                consumerThread.Join(new TimeSpan(0, 1, 0));
            }
            catch
            {
            }
            finally
            {
                producerThread = null;
                consumerThread = null;
            }
        }
    }
}