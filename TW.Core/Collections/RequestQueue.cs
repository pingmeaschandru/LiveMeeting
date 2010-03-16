using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TW.Core.Collections
{
    public class RequestQueue<T> : BlockingQueue<T>
    {
        private const int DEFAULT_PAGING_SIZE = 5000;

        private Queue<T> tempQueue;
        private readonly string prefixFileName;
        private uint headIndex;
        private uint tailIndex;
        private readonly int pagingSize;
        private readonly int maxSize;

        public RequestQueue(string tmpFilePath, int pagingSize, int maxQSize)
        {
            this.pagingSize = pagingSize;
            maxSize = maxQSize;

            if (this.pagingSize <= 0)
                this.pagingSize = DEFAULT_PAGING_SIZE;

            if (this.pagingSize > maxSize)
                maxSize = this.pagingSize;

            if (!Directory.Exists(tmpFilePath))
                Directory.CreateDirectory(tmpFilePath);

            prefixFileName = tmpFilePath + @"\" + Guid.NewGuid();
            headIndex = 0;
            tailIndex = 0;
            tempQueue = null;
        }

        public RequestQueue(string tmpFilePath, int maxQSize)
            : this(tmpFilePath, DEFAULT_PAGING_SIZE, maxQSize)
        {
        }

        public RequestQueue(string tmpFilePath)
            : this(tmpFilePath, DEFAULT_PAGING_SIZE, 0x0FFFFFFF)
        {
        }

        private void pWriteQueueInFile(Queue<T> obj)
        {
            if (obj == null)
                return;

            if (File.Exists(LastFileName))
                File.Delete(LastFileName);

            using (var fs = File.Open(LastFileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var serializer = new BinaryFormatter();
                serializer.Serialize(fs, obj);
            }

            tailIndex = tailIndex + 1;
        }

        private Queue<T> pReadQueueFromFile()
        {
            Queue<T>  deserialisedQueue = null;

            if (File.Exists(FirstFileName))
            {
                using (var fs = File.Open(FirstFileName, FileMode.Open, FileAccess.Read))
                {
                    var deserializer = new BinaryFormatter();
                     deserialisedQueue = (Queue<T>)deserializer.Deserialize(fs);
                }

                File.Delete(FirstFileName);
                headIndex = headIndex + 1;
            }

            return  deserialisedQueue;
        }

        protected override void enqueue(T request)
        {
            if (queueCount() > maxSize)
                throw new OverflowException();

            if (queue.Count >= pagingSize)
            {
                pWriteQueueInFile(queue);
                queue.Clear();
            }

            base.enqueue(request);
        }

        protected override T dequeue()
        {
            if (TmpFileCount > 0 && tempQueue == null)
                tempQueue = pReadQueueFromFile();

            T obj = default(T);

            if (tempQueue != null)
            {
                obj = tempQueue.Dequeue();
                if (tempQueue.Count == 0)
                    tempQueue = null;
            }
            else
                obj = base.dequeue();

            return obj;
        }

        protected override T peek()
        {
            if (TmpFileCount > 0 && tempQueue == null)
                tempQueue = pReadQueueFromFile();

            var obj = ((tempQueue != null) ? tempQueue.Peek() : base.peek());

            return obj;
        }

        protected override bool contains(T obj)
        {
            throw new NotImplementedException();
        }

        protected override int queueCount()
        {
            var cnt = base.queueCount() + (TmpFileCount * pagingSize);
            if (tempQueue != null)
                cnt = cnt + tempQueue.Count;

            return cnt;
        }

        private string FirstFileName
        {
            get { return prefixFileName + "_" + headIndex + ".bin"; }
        }

        private string LastFileName
        {
            get { return prefixFileName + "_" + tailIndex + ".bin"; }
        }

        private int TmpFileCount
        {
            get
            {
                int diff;

                if (tailIndex < headIndex)
                    diff = (int)((uint.MaxValue - headIndex) + tailIndex) + 1;
                else
                    diff = (int)(tailIndex - headIndex);

                return diff;
            }
        }
    }
}