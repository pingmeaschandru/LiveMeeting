using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using TW.Core.IO;

namespace TW.Core.Test.IO
{
    [TestFixture]
    public class FifoStreamTest
    {
        [Test]
        public void ShouldAbleToWriteBytes()
        {
            var data = new byte[]
                              {
                                  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10
                              };

            var fs = new FifoStream();
            fs.Write(data, 0, data.Length);
            Assert.AreEqual(10, fs.Length);
        }

        [Test]
        public void ShouldAbleToReadWroteBytes()
        {
            var data = new byte[]
                              {
                                  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10
                              };

            var fs = new FifoStream();
            fs.Write(data, 0, data.Length);

            var finalData = new byte[10];
            fs.Read(finalData, 0, finalData.Length);

            for (var i = 0; i < data.Length; i++)
                if (data[i] != finalData[i])
                    Assert.Fail("Array value is not same");
        }

        [Test]
        public void ShouldAbleToAdvanceTheBytes()
        {
            var data = new byte[]
                              {
                                  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10
                              };

            var fs = new FifoStream();
            fs.Write(data, 0, data.Length);

            fs.Advance(2);

            var finalData = new byte[8];
            fs.Read(finalData, 0, finalData.Length);

            for (var i = 2; i < data.Length; i++)
                if (data[i] != finalData[i - 2])
                    Assert.Fail("Array value is not same");
        }

        [Test]
        public void ShouldAbleToGetThePeekBytes()
        {
            var data = new byte[]
                              {
                                  0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10
                              };

            var fs = new FifoStream();
            fs.Write(data, 0, data.Length);

            var peekData = new byte[10];
            fs.Peek(peekData, 0, peekData.Length);

            for (var i = 0; i < data.Length; i++)
                if (data[i] != peekData[i])
                    Assert.Fail("Array value is not same");

            Assert.AreEqual(10, fs.Length);
        }

        [Test]
        public void CheckForThreadSafe()
        {
            var fs = new FifoStream();
            var actualResult = new List<byte>();
            var running = true;
            var dataToWrite = new byte[]
                                  {
                                      0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
                                  };

            var readThread = new Thread(new ParameterizedThreadStart(delegate
            {
                while (running || fs.Length > 0)
                {
                    if (fs.Length <= 0) continue;
                    var dataBuffer = new byte[10];
                    var actualCount = fs.Read(dataBuffer, 0, dataBuffer.Length);

                    for (var i = 0; i < actualCount; i++)
                        actualResult.Add(dataBuffer[i]);
                }
            }));

            readThread.Start();

            var writeThread = new Thread(new ParameterizedThreadStart(delegate
            {
                for (var i = 0; i < 100; i++)
                    fs.Write(dataToWrite, 0, dataToWrite.Length);
            }));

            writeThread.Start();

            Assert.AreEqual(true, writeThread.Join(60000));

            running = false;

            Assert.AreEqual(true, readThread.Join(60000));


            var exceptionFlag = false;
            for (int i = 0, k=0; i < 100; i++)
            {
                foreach (var b in dataToWrite)
                {
                    exceptionFlag = (actualResult[k] != b);
                    k++;
                    if (exceptionFlag == false)
                        break;
                }
            }

            Assert.AreEqual(true, exceptionFlag);
        }
    }
}