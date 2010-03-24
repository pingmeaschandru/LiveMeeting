using System;
using System.Drawing;
using System.Runtime.InteropServices;
using TW.Coder;
using TW.Core.Collections;
using TW.Core.IO;
using TW.Core.Native;
using TW.Core.Threading;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;
using TW.WaveLib;

namespace MockDesktopPlayer
{
    public class RenderingEngine : AThread
    {
        private readonly Action<Bitmap> action;
        private readonly BlockingQueue<RdapMessage> queue = new BlockingQueue<RdapMessage>();
        private readonly WaveOutPlayer player;
        private readonly FifoStream fifoStream;

        public RenderingEngine(Action<Bitmap> action)
        {
            fifoStream = new FifoStream();
            player = new WaveOutPlayer(-1, new WaveFormat(44100, 16, 2), 16384, 3, Filler);
            this.action = action;
            start();
        }

        public void Process(RdapMessage dataToRender)
        {
            queue.Enqueue(dataToRender);
        }

        protected override void OnRun()
        {
            var rdapMessage = queue.Dequeue();

            if (rdapMessage.MessageType != RdapMessageType.DesktopWindowImageFrameMessage) return;
            var windowInfoMessage = new DesktopWindowImageFrameMessage(rdapMessage.Data);
            if (windowInfoMessage.FormatType != RdapImagePixelFormatType.PIX_FMT_RGB24) return;
            var bitmapData = RgbFrameFactory.CreateBitmap(new RgbFrame((int)windowInfoMessage.Width,
                                                                       (int)windowInfoMessage.Height,
                                                                       PixelFormatType.PIX_FMT_RGB24,
                                                                       windowInfoMessage.ImageBytes));

            if (bitmapData != null) action(bitmapData);


            //if (rdapMessage.MessageType != RdapMessageType.VideoFrameMessage) return;
            //var windowInfoMessage = new VideoFrameMessage(rdapMessage.Data);
            //if (windowInfoMessage.FormatType != RdapImagePixelFormatType.PIX_FMT_RGB24) return;
            //var bitmapData = RgbFrameFactory.CreateBitmap(new RgbFrame((int)windowInfoMessage.Width,
            //                                                           (int)windowInfoMessage.Height,
            //                                                           PixelFormatType.PIX_FMT_RGB24,
            //                                                           windowInfoMessage.ImageBytes));

            //if (bitmapData != null) action(bitmapData);


            //if (rdapMessage.MessageType != RdapMessageType.AudioFrameMessage) return;
            //var audioFrameMessage = new AudioFrameMessage(rdapMessage.Data);
            //if (audioFrameMessage.FormatType != AudioFormatType.Wave) return;
            //fifoStream.Write(audioFrameMessage.AudioBytes, 0, audioFrameMessage.AudioBytes.Length);
        }

        private void Filler(IntPtr data, int size)
        {
            var playBuffer = new byte[size];
            if (fifoStream.Length >= size)
                fifoStream.Read(playBuffer, 0, size);
            else
                for (var i = 0; i < playBuffer.Length; i++)
                    playBuffer[i] = 0;

            Marshal.Copy(playBuffer, 0, data, size);
        }

        public void Close()
        {
            stop();
        }
    }
}
