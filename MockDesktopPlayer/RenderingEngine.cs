using System;
using System.Drawing;
using TW.Coder;
using TW.Core.Collections;
using TW.Core.Threading;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;

namespace MockDesktopPlayer
{
    public class RenderingEngine : AThread
    {
        private readonly Action<Bitmap> action;
        private readonly BlockingQueue<RdapMessage> queue = new BlockingQueue<RdapMessage>();

        public RenderingEngine(Action<Bitmap> action)
        {
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

            if (rdapMessage.MessageType != RdapMessageType.DesktopWindowInfoMessage) return;
            var windowInfoMessage = new DesktopWindowInfoMessage(rdapMessage.Data);
            if (windowInfoMessage.FormatType != RdapImagePixelFormatType.PIX_FMT_RGB24) return;
            var bitmapData = RgbFrameFactory.CreateBitmap(new RgbFrame((int) windowInfoMessage.Width,
                                                                       (int) windowInfoMessage.Height,
                                                                       PixelFormatType.PIX_FMT_RGB24,
                                                                       windowInfoMessage.ImageBytes));
            if (bitmapData != null) action(bitmapData);
        }

        public void Close()
        {
            stop();
        }
    }
}
