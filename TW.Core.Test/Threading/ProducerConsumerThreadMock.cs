using System;
using System.Collections.Generic;
using TW.Core.Threading;

namespace TW.Core.Test.Threading
{
    public class ProducerConsumerThreadMock : ProducerConsumerThread<SharedObjectTest.MockObject>
    {
        private int currentVal;
        private readonly List<int> intArray;
        private readonly int maxLimit;       
        public event EventHandler OnComplete;

        public ProducerConsumerThreadMock()
        {
            currentVal = 0;
            maxLimit = 10000;
            intArray = new List<int>();
        }

        protected override SharedObjectTest.MockObject OnProduce()
        {
            return currentVal <= maxLimit ? new SharedObjectTest.MockObject(currentVal++) : null;
        }

        protected override void OnConsume(SharedObjectTest.MockObject obj)
        {
            intArray.Add(obj.Data);

            Console.WriteLine(obj.Data);

            if (intArray.Count == maxLimit && OnComplete != null)
                OnComplete(this, new EventArgs());
        }

        public List<int> IntArray
        {
            get { return intArray; }
        }
    }
}