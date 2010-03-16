using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using TW.Coder;
using TW.Core.Sockets;
using TW.LiveMeet.DesktopAgent.Command;

namespace MockClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = new TcpSocketClient();
            publisher.Connect(new IPEndPoint(IPAddress.Loopback, 5000));
            
            var dCapture = new DesktopCapture(PixelFormat.Format24bppRgb, new Size(640, 400));

            var ss = new SocketStorage(publisher, dCapture); 
            
            Console.ReadLine();

            for (var i = 0; i < 200000000; i++)
            {
                //Thread.Sleep(500);
                var frame = dCapture.Execute();
                ss.Process(frame);
            }


            Console.ReadLine();
        }
    }
}
