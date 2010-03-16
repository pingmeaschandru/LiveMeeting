using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using TW.Core.Threading;

namespace TW.Core.Test.Threading
{
    [TestFixture]
    public class SharedObjectTest
    {
        public class MockObject
        {
            private readonly int _data;

            public MockObject(int i)
            {
                _data = i;
            }

            public int Data
            {
                get { return _data; }
            }

            public override string ToString()
            {
                return _data.ToString();
            }
        }

        [Test]
        public void CheckThreadTerminatesProperlyAfterWriteOperation()
        {
            var sharedObject = new SharedObject<MockObject>();
            var o = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               o.ObjectValue = new MockObject(5);
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));
        }

        [Test]
        public void CheckThreadTerminatesProperlyAfterReadOperation()
        {
            var sharedObject = new SharedObject<MockObject> { ObjectValue = new MockObject(5) };
            var o = sharedObject;
            var tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                          {
                                                                              Console.WriteLine(o.ObjectValue.Data);
                                                                          }));
            tReadThread.Start();

            Assert.AreEqual(true, tReadThread.Join(6000));
        }

        [Test]
        public void CheckSharedObjectAfterWriteFromAnotherThread()
        {
            var sharedObject = new SharedObject<MockObject>();
            var o = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               o.ObjectValue = new MockObject(5);
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));
            Assert.AreEqual(5, sharedObject.ObjectValue.Data);
        }

        [Test]
        public void CheckThreadSafeWhenReadWriteThousandSequenceNumbers()
        {
            var actualResult = new List<int>();
            var sharedObject = new SharedObject<MockObject>();
            var running = true;
            var o = sharedObject;
            var tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                          {
                                                                              while (running)
                                                                              {
                                                                                  var tmpSharedObject = o.ObjectValue;
                                                                                  if (tmpSharedObject != null)
                                                                                      actualResult.Add(tmpSharedObject.Data);
                                                                              }
                                                                          }));
            tReadThread.Start();

            var o1 = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               for (var i = 0; i < 1000; i++)
                                                                                   o1.ObjectValue = new MockObject(i);
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));

            running = false;
            sharedObject.Clear();

            Assert.AreEqual(true, tReadThread.Join(60000));

            var exceptionFlag = false;
            for (var i = 0; i < actualResult.Count; i++)
            {
                exceptionFlag = (actualResult[i] == i);
                if (exceptionFlag == false)
                    break;
            }

            Assert.AreEqual(true, exceptionFlag);
        }

        [Test]
        public void CheckThreadSafeWhenReadThreadEndedFirst()
        {
            var sharedObject = new SharedObject<MockObject>();
            var running = true;

            var o = sharedObject;
            var tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                          {
                                                                              while (running)
                                                                              {
                                                                                  var tmpSharedObject = o.ObjectValue;
                                                                                  if (tmpSharedObject != null)
                                                                                      Console.WriteLine(tmpSharedObject.Data);

                                                                                  break;
                                                                              }
                                                                          }));
            tReadThread.Start();

            var o1 = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               var i = 0;
                                                                               while (running)
                                                                                   o1.ObjectValue = new MockObject(i++);
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tReadThread.Join(60000));

            running = false;
            sharedObject.Clear();

            Assert.AreEqual(true, tWriteThread.Join(60000));
        }

        [Test]
        public void CheckThreadSafeWhenWriteThreadEndedFirst()
        {
            var sharedObject = new SharedObject<MockObject>();
            var running = true;

            var o1 = sharedObject;
            var tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                          {
                                                                              while (running)
                                                                              {
                                                                                  var tmpSharedObject = o1.ObjectValue;
                                                                                  if (tmpSharedObject != null)
                                                                                      Console.WriteLine(tmpSharedObject.Data);
                                                                              }
                                                                          }));
            tReadThread.Start();

            var o = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               var i = 0;
                                                                               while (running)
                                                                               {
                                                                                   o.ObjectValue = new MockObject(i++);
                                                                                   break;
                                                                               }
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));

            running = false;
            sharedObject.Clear();

            Assert.AreEqual(true, tReadThread.Join(60000));
        }

        [Test]
        public void ShouldBeAbleToReuseTheSharedMemory()
        {
            /////////////////// First Use //////////////////////////////
            var sharedObject = new SharedObject<MockObject>();
            var running = true;

            var o1 = sharedObject;
            var tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                          {
                                                                              while (running)
                                                                              {
                                                                                  var tmpSharedObject = o1.ObjectValue;
                                                                                  if (tmpSharedObject != null)
                                                                                      Console.WriteLine(tmpSharedObject.Data);
                                                                              }
                                                                          }));
            tReadThread.Start();

            var o = sharedObject;
            var tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                           {
                                                                               for (var i = 0; i < 1000; i++)
                                                                                   o.ObjectValue = new MockObject(i);
                                                                           }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));

            running = false;
            sharedObject.Clear();

            Assert.AreEqual(true, tReadThread.Join(60000));
            /////////////////// First Use //////////////////////////////

            /////////////////// Second Use /////////////////////////////
            sharedObject = new SharedObject<MockObject>();
            running = true;

            var sharedObject1 = sharedObject;
            tReadThread = new Thread(new ParameterizedThreadStart(delegate
                                                                      {
                                                                          while (running)
                                                                          {
                                                                              var tmpSharedObject = sharedObject1.ObjectValue;
                                                                              if (tmpSharedObject != null)
                                                                                  Console.WriteLine(tmpSharedObject.Data);
                                                                          }
                                                                      }));
            tReadThread.Start();

            var o2 = sharedObject;
            tWriteThread = new Thread(new ParameterizedThreadStart(delegate
                                                                       {
                                                                           for (var i = 0; i < 1000; i++)
                                                                               o2.ObjectValue = new MockObject(i);
                                                                       }));
            tWriteThread.Start();

            Assert.AreEqual(true, tWriteThread.Join(60000));

            running = false;
            sharedObject.Clear();

            Assert.AreEqual(true, tReadThread.Join(60000));
            /////////////////// Second Use /////////////////////////////
        }
    }
}