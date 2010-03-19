using TW.Coder;
using TW.Core.IO;
using TW.Core.Sockets;
using TW.LiveMeet.DesktopAgent;
using TW.LiveMeet.DesktopAgent.Command;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;
using TW.LiveMeet.RDAP.Parser;

namespace MockClient
{
    public class SocketStorage : IDesktopCaptureStorage
    {
        private readonly TcpSocketClient client;
        private readonly DesktopCapture desktopCapture;
        private readonly FifoStream parserStream = new FifoStream();
        private readonly RdapMessageParser parser;

        public SocketStorage(TcpSocketClient client, DesktopCapture desktopCapture)
        {
            parser = new RdapMessageParser(parserStream);
            this.client = client;
            this.desktopCapture = desktopCapture;
            this.client.OnDataRecieved += OnDataRecieved;
        }

        private void OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            parserStream.Write(e.Buffer, 0, e.Buffer.Length);
            RdapMessage rdapMessage;
            if (!parser.TryParseMessage(out rdapMessage)) return;

            switch (rdapMessage.MessageType)
            {
                case RdapMessageType.MouseClickEventMessage:
                    DoMouseClick(rdapMessage);
                    break;
                case RdapMessageType.MouseDragEventMesssage:
                    DoMouseDrag(rdapMessage);
                    break;
                case RdapMessageType.KeyboardEventMessage:
                    DoKeyStroke(rdapMessage);
                    break;
            }
        }

        private void DoKeyStroke(RdapMessage message)
        {
            var mouseDragEventMesssage = new KeyboardEventMessage(message.Data);
            new KeyboardStroke(mouseDragEventMesssage.KeyStroke).Execute();
        }

        private void DoMouseDrag(RdapMessage message)
        {
            var mouseDragEventMesssage = new MouseDragEventMesssage(message.Data);
            switch (mouseDragEventMesssage.EventType)
            {
                case MouseEventType.Left:
                    new MouseDrag((int) ((int)mouseDragEventMesssage.StartXPosition * desktopCapture.WidthAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.EndXPosition * desktopCapture.WidthAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.StartYPosition * desktopCapture.HeightAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.EndYPosition * desktopCapture.HeightAspectRatio),
                                  0).Execute();
                    break;
                case MouseEventType.Right:
                    new MouseDrag((int) ((int)mouseDragEventMesssage.StartXPosition * desktopCapture.WidthAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.EndXPosition * desktopCapture.WidthAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.StartYPosition * desktopCapture.HeightAspectRatio),
                                  (int) ((int)mouseDragEventMesssage.EndYPosition * desktopCapture.HeightAspectRatio),
                                  1).Execute();
                    break;
                default:
                    return;
            }
        }

        private void DoMouseClick(RdapMessage message)
        {
            var mouseClickEventMessage = new MouseClickEventMessage(message.Data);
            switch (mouseClickEventMessage.EventType)
            {
                case MouseEventType.Left:
                    new MouseLeft((int) ((int) mouseClickEventMessage.XPosition*desktopCapture.WidthAspectRatio),
                                  (int) ((int) mouseClickEventMessage.YPosition*desktopCapture.HeightAspectRatio))
                                  .Execute();
                    break;
                case MouseEventType.Right:
                    new MouseRight((int)((int)mouseClickEventMessage.XPosition * desktopCapture.WidthAspectRatio),
                                  (int)((int)mouseClickEventMessage.YPosition * desktopCapture.HeightAspectRatio))
                                  .Execute();
                    break;
                default:
                    return;
            }
        }

        public void Process(RgbFrame frame)
        {
            if (frame == null)
                return;

            var message = new DesktopWindowImageFrameMessage(RdapImagePixelFormatType.PIX_FMT_RGB24, (uint)frame.Width, (uint)frame.Height, frame.Data);
            var dataToSend = new RdapMessage(RdapMessageType.DesktopWindowImageFrameMessage, message.ToBytes()).ToBytes();
            client.Send(dataToSend, 0, dataToSend.Length);
        }
    }
}