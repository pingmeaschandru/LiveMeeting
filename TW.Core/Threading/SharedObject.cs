using System;
using System.Threading;

namespace TW.Core.Threading
{
    public sealed class SharedObject<T>  where T : class 
    {
        private T objectValue;
        private readonly Object syncroot;
        private bool complete;

        public SharedObject()
        {
            objectValue = default(T);
            syncroot = new Object();           
            complete = false;
        }

        public T ObjectValue
        {
            get
            {
                lock (syncroot)
                {
                    while (Equals(objectValue, default(T)) && !complete)
                        Monitor.Wait(syncroot);

                    complete = false;
                    var obj = objectValue;
                    objectValue = default(T);

                    Monitor.PulseAll(syncroot);

                    return obj;
                }
            }
            set
            {
                lock (syncroot)
                {
                    while (!Equals(objectValue, default(T)) && !complete)
                        Monitor.Wait(syncroot);

                    complete = false;
                    objectValue = value;

                    Monitor.PulseAll(syncroot);
                }
            }
        }

        public void Clear()
        {
            lock (syncroot)
            {
                complete = true;
                Monitor.PulseAll(syncroot);
            }
        }
    }
}