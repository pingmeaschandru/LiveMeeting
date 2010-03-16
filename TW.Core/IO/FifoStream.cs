using System;
using System.IO;
using System.Collections;

namespace TW.Core.IO
{
    public class FifoStream : Stream
    {
        private const int BLOCK_SIZE = 65536;
        private const int MAX_BLOCKS_IN_CACHE = (3 * 1024 * 1024) / BLOCK_SIZE;

        private int size;
        private int rPos;
        private int wPos;
        private readonly Stack usedBlocks = new Stack();
        private readonly ArrayList blocks = new ArrayList();

        private byte[] AllocBlock()
        {
            return usedBlocks.Count > 0 ? (byte[])usedBlocks.Pop() : new byte[BLOCK_SIZE];
        }

        private void FreeBlock(byte[] block)
        {
            if (usedBlocks.Count < MAX_BLOCKS_IN_CACHE)
                usedBlocks.Push(block);
        }

        private byte[] GetWBlock()
        {
            byte[] result;
            if (wPos < BLOCK_SIZE && blocks.Count > 0)
                result = (byte[])blocks[blocks.Count - 1];
            else
            {
                result = AllocBlock();
                blocks.Add(result);
                wPos = 0;
            }
            return result;
        }

        public int Advance(int count)
        {
            lock(this)
            {
                var sizeLeft = count;
                while (sizeLeft > 0 && size > 0)
                {
                    if (rPos == BLOCK_SIZE)
                    {
                        rPos = 0;
                        FreeBlock((byte[])blocks[0]);
                        blocks.RemoveAt(0);
                    }
                    var toFeed = blocks.Count == 1 ? Math.Min(wPos - rPos, sizeLeft) : Math.Min(BLOCK_SIZE - rPos, sizeLeft);
                    rPos += toFeed;
                    sizeLeft -= toFeed;
                    size -= toFeed;
                }
                return count - sizeLeft;
            }
        }

        public int Peek(byte[] buf, int ofs, int count)
        {
            lock(this)
            {
                var sizeLeft = count;
                var tempBlockPos = rPos;
                var tempSize = size;

                var currentBlock = 0;
                while (sizeLeft > 0 && tempSize > 0)
                {
                    if (tempBlockPos == BLOCK_SIZE)
                    {
                        tempBlockPos = 0;
                        currentBlock++;
                    }
                    var upper = currentBlock < blocks.Count - 1 ? BLOCK_SIZE : wPos;
                    var toFeed = Math.Min(upper - tempBlockPos, sizeLeft);
                    Array.Copy((byte[])blocks[currentBlock], tempBlockPos, buf, ofs + count - sizeLeft, toFeed);
                    sizeLeft -= toFeed;
                    tempBlockPos += toFeed;
                    tempSize -= toFeed;
                }
                return count - sizeLeft;
            }
        }

        public override void SetLength(long len)
        {
            throw new InvalidOperationException();
        }

        public override long Seek(long pos, SeekOrigin o)
        {
            throw new InvalidOperationException();
        }

        public override int Read(byte[] buf, int ofs, int count)
        {
            lock (this)
            {
                var result = Peek(buf, ofs, count);
                Advance(result);
                return result;
            }
        }

        public override void Write(byte[] buf, int ofs, int count)
        {
            lock (this)
            {
                var left = count;
                while (left > 0)
                {
                    var toWrite = Math.Min(BLOCK_SIZE - wPos, left);
                    Array.Copy(buf, ofs + count - left, GetWBlock(), wPos, toWrite);
                    wPos += toWrite;
                    left -= toWrite;
                }
                size += count;
            }
        }

        public override void Close()
        {
            Flush();
        }

        public override void Flush()
        {
            lock (this)
            {
                foreach (byte[] block in blocks)
                    FreeBlock(block);
                blocks.Clear();
                rPos = 0;
                wPos = 0;
                size = 0;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get
            {
                lock (this)
                    return size;
            }
        }

        public override long Position
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }
    }
}