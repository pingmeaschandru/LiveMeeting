using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using TW.Core.IO;
using TW.Core.Sockets;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;
using TW.LiveMeet.RDAP.Parser;

namespace MockDesktopPlayer
{
    public partial class DesktopPlayerControl : UserControl
    {
        private TcpSocketClient subscriber;
        private readonly FifoStream parserStream = new FifoStream();
        private readonly RdapMessageParser parser;
        private readonly RenderingEngine engine;

        private int dragStartX;
        private int dragStartY;
        private MouseEventType dragStartMouseEventType = MouseEventType.Left;

        private int dragEndX;
        private int dragEndY;
        private MouseEventType dragEndMouseEventType = MouseEventType.Left;

        public DesktopPlayerControl()
        {
            parser = new RdapMessageParser(parserStream);
            engine = new RenderingEngine(RenderBitmap);
            InitializeComponent();
        }

        private void play(object sender, EventArgs e)
        {
            if (subscriber != null) return;
            subscriber = new TcpSocketClient();
            subscriber.Connect(new IPEndPoint(IPAddress.Loopback, 5001));//(IPAddress.Parse("10.7.3.160"), 5001));
            subscriber.OnDataRecieved += OnDataRecieved;
        }

        private void stop(object sender, EventArgs e)
        {
        }

        private void RenderBitmap(Image bitmap)
        {
            try
            {
                pbDesktopCapturePane.Image = bitmap;
            }
            catch (Exception)
            {

            }
        }

        private void OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            parserStream.Write(e.Buffer, 0, e.Buffer.Length);
            RdapMessage rdapMessage;
            if (!parser.TryParseMessage(out rdapMessage)) return;

            engine.Process(rdapMessage);
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    dragStartMouseEventType = MouseEventType.Left;
                    break;
                case MouseButtons.Middle:
                    dragStartMouseEventType = MouseEventType.Middle;
                    break;
                case MouseButtons.Right:
                    dragStartMouseEventType = MouseEventType.Right;
                    break;
                default:
                    return;
            }

            dragStartX = e.X;
            dragStartY = e.Y;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    dragEndMouseEventType = MouseEventType.Left;
                    break;
                case MouseButtons.Middle:
                    dragEndMouseEventType = MouseEventType.Middle;
                    break;
                case MouseButtons.Right:
                    dragEndMouseEventType = MouseEventType.Right;
                    break;
                default:
                    return;
            }

            dragEndX = e.X;
            dragEndY = e.Y;

            byte[] dataToSend;
            if (dragEndX == dragStartX && dragEndY == dragStartY)
            {
                var message = new MouseClickEventMessage(dragEndMouseEventType, (uint)e.X, (uint)e.Y);
                var messageRdap = new RdapMessage(RdapMessageType.MouseClickEventMessage, message.ToBytes());
                dataToSend = messageRdap.ToBytes();
            }
            else
            {
                var message = new MouseDragEventMesssage(dragEndMouseEventType, (uint)dragStartX, (uint)dragStartY, (uint)dragEndX, (uint)dragEndY);
                var messageRdap = new RdapMessage(RdapMessageType.MouseClickEventMessage, message.ToBytes());
                dataToSend = messageRdap.ToBytes();
            }

            if (subscriber != null)
                subscriber.Send(dataToSend, 0, dataToSend.Length);  
        }

        public void SendKeyEvent(char c)
        {
            var eventMessage = new KeyboardEventMessage((byte) c, 0x00);
            var messageRdap = new RdapMessage(RdapMessageType.KeyboardEventMessage, eventMessage.ToBytes());
            var dataToSend = messageRdap.ToBytes();

            if (subscriber != null)
                subscriber.Send(dataToSend, 0, dataToSend.Length);
        }
    }
}
