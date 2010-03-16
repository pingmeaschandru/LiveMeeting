using System;
using System.Threading;
using System.Collections.Generic;

namespace TW.Core.Collections
{
    public class BlockingQueue<T>
    {
        protected Queue<T> queue;
        private readonly Object syncroot;
        private bool blocking;

        public BlockingQueue()
        {
            blocking = true;
            queue = new Queue<T>();
            syncroot = new Object();
        }

        protected virtual void enqueue(T request)
        {
            queue.Enqueue(request);
            Monitor.PulseAll(syncroot);
        }

        protected virtual T dequeue()
        {
            var obj = default(T);

            while (queue.Count == 0 && blocking)
                Monitor.Wait(syncroot);

            if (queue.Count > 0)
                obj = queue.Dequeue();

            return obj;
        }

        protected virtual T peek()
        {
            var obj = default(T);

            if (queue.Count > 0)
                obj = queue.Peek();

            return obj;
        }

        protected virtual bool contains(T obj)
        {
            return queue.Contains(obj);
        }

        protected virtual int queueCount()
        {
            return queue.Count;
        }

        protected virtual void clear()
        {
            queue.Clear();
        }

        public void Enqueue(T request)
        {
            lock (syncroot)
                enqueue(request);
        }

        public T Dequeue()
        {
            lock (syncroot)
                return dequeue();
        }

        public T[] Dequeue(int cnt)
        {
            if (cnt <= 0)
                return null;

            var tmpList = new List<T>();

            lock (syncroot)
            {
                for (; ;)
                {
                    if (queueCount() == 0 && tmpList.Count > 0)
                        break;

                    tmpList.Add(dequeue());
                    if (tmpList.Count >= cnt)
                        break;
                }
            }

            return tmpList.ToArray();
        }

        public T Peek()
        {
            lock (syncroot)
                return peek();
        }

        public bool Contains(T obj)
        {
            lock (syncroot)
                return contains(obj);
        }

        public void Clear()
        {
            Blocking = false;

            lock (syncroot)
                clear();
        }

        public int Count
        {
            get
            {
                int cnt;
                lock (syncroot)
                    cnt = queueCount();

                return cnt;
            }
        }

        private bool Blocking
        {
            set
            {
                lock (syncroot)
                {
                    blocking = value;
                    if (!blocking)
                        Monitor.PulseAll(syncroot);
                }
            }
        }
    }
}