using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Runtime.InteropServices;
using TW.Core.Sockets;
using TW.LiveMeet.DesktopAgent.Command;

namespace MockDesktopStreamingClient
{
    class Program
    {
        [DllImport("crtdll.dll")]
        public static extern int _kbhit();

        static void Main()
        {
            Console.Write("Enter IP Address : ");
            var strIPAddress = Console.ReadLine();

            Console.Write("Enter Port : ");
            var strPort = Console.ReadLine();
            var port = int.Parse(strPort);

            var publisher = new TcpSocketClient(8000);
            publisher.Connect(new IPEndPoint(IPAddress.Parse(strIPAddress), port));
            
            var dCapture = new DesktopCapture(PixelFormat.Format24bppRgb, new Size(640, 400));

            var ss = new SocketStorage(publisher, dCapture); 
            
            Console.WriteLine("Press Enter To Start streaming....");

            Console.ReadLine();

            Console.WriteLine("Press any Key To Exit....");

            while (_kbhit() == 0)
            {
                var frame = dCapture.Execute();
                ss.Process(frame);
            }
        }
    }
}