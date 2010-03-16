using System;
using System.Drawing;
using TW.H264Coder.Vcl;

namespace TW.H264Coder
{
    public class YuvFrameBuffer : FrameBuffer
    {
        private readonly YuvFormatType yuvType;
        private int[][] yComponent;
        private int[][] crComponent;
        private int[][] cbComponent;

        public YuvFrameBuffer(YuvFormatType yuvType, Size frameSize)
            : base(frameSize)
        {
            this.yuvType = yuvType;
            Init();
        }

        private void Init()
        {
            if(yuvType != YuvFormatType.Yuv420)
                throw new Exception();

            UVWidth = Width / 2;
            UVHeight = Height / 2;

            yComponent = new int[Height][];
            for (var i = 0; i < yComponent.Length; i++)
                yComponent[i] = new int[Width];

            crComponent = new int[UVHeight][];
            for (var i = 0; i < crComponent.Length; i++)
                crComponent[i] = new int[UVWidth];

            cbComponent = new int[UVHeight][];
            for (var i = 0; i < cbComponent.Length; i++)
                cbComponent[i] = new int[UVWidth];
        }

        public void CopyData(byte[] data)
        {
            var cbOffset = Width*Height;
            var crOffset = cbOffset + (UVWidth*UVHeight);

            for (var i = 0; i < Height; i++)
                for (var j = 0; j < Width; j++)
                {
                    var indexY = j + i*Width;
                    yComponent[i][j] = (data[indexY] & 0xFF);
                }

            for (var i = 0; i < UVHeight; i++)
                for (var j = 0; j < UVWidth; j++)
                {
                    var indexUV = j + i*UVWidth;
                    cbComponent[i][j] = (data[indexUV + cbOffset] & 0xFF);
                    crComponent[i][j] = (data[indexUV + crOffset] & 0xFF);
                }
        }

        public void CopyData(YuvFrameBuffer yuvObj)
        {
            if ((yuvObj.Width > Width) || (yuvObj.Height > Height))
            {
                yComponent = new int[yuvObj.Height][];
                for (var i = 0; i < yComponent.Length; i++)
                    yComponent[i] = new int[yuvObj.Width];
            }
            if ((yuvObj.UVWidth > UVWidth) || (yuvObj.UVHeight > UVHeight))
            {
                cbComponent = new int[yuvObj.UVHeight][];
                for (var i = 0; i < cbComponent.Length; i++)
                    cbComponent[i] = new int[yuvObj.UVWidth];

                crComponent = new int[yuvObj.UVHeight][];
                for (var i = 0; i < crComponent.Length; i++)
                    crComponent[i] = new int[yuvObj.UVWidth];
            } 

            Array.Copy(yuvObj.yComponent, 0, yComponent, 0, yComponent.Length);
            Array.Copy(yuvObj.cbComponent, 0, cbComponent, 0, cbComponent.Length);
            Array.Copy(yuvObj.crComponent, 0, crComponent, 0, crComponent.Length);
        }

        public int UVWidth { get; private set; }
        public int UVHeight { get; private set; }

        public int GetY8Bit(int x, int y)
        {
            return yComponent[y][x];
        }

        public void SetY8Bit(int x, int y, int value)
        {
            yComponent[y][x] = value;
        }

        public int GetCb8Bit(int x, int y)
        {
            return cbComponent[y][x];
        }

        public void SetCb8Bit(int x, int y, int value)
        {
            cbComponent[y][x] = value;
        }

        public int GetCr8Bit(int x, int y)
        {
            return crComponent[y][x];
        }

        public void SetCr8Bit(int x, int y, int value)
        {
            crComponent[y][x] = value;
        }
    }
}