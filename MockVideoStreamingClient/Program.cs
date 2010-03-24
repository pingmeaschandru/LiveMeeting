using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using TW.Coder;
using TW.Core.Sockets;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Messages;

namespace MockVideoStreamingClient
{
    public class Program
    {
        static void Main()
        {
            var publisher = new TcpSocketClient(8000);
            publisher.Connect(new IPEndPoint(IPAddress.Loopback, 5000));

            var cam = new VideoCapture(0, 15, 160, 120);
            cam.Start();

            Console.ReadLine();

            for (var i = 0; i < 200000000; i++)
            {
                var ip = cam.GetBitMap();
                var image = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, ip);
                image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                var frame = RgbFrameFactory.CreateFrame(image);
                var message = new VideoFrameMessage(RdapImagePixelFormatType.PIX_FMT_RGB24, (uint)frame.Width, (uint)frame.Height, frame.Data);
                var dataToSend = new RdapMessage(RdapMessageType.VideoFrameMessage, message.ToBytes()).ToBytes();
                publisher.Send(dataToSend, 0, dataToSend.Length);
                image.Dispose();
            }

            Console.ReadLine();
        }
    }
}
