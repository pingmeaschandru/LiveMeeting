using System;
using System.Net;
using System.Threading;
using TW.Core.Helper;
using TW.Core.Sockets;

namespace UdpMockPlayer
{
    class Program
    {
        static void Main()
        {
            var publisher = new UdpSocket(new IPEndPoint(IPAddress.Loopback, 6001));
            publisher.OnDataRecieved += publisher_OnDataRecieved;
            publisher.OnExceptionThrown += publisher_OnExceptionThrown;

            Console.ReadLine();

            for (var i = 0; i < 200000000; i++)
            {
                var dataToSend = StringHelper.GetAsciiBytes("Hi Server......");
                publisher.Send(dataToSend, 0, dataToSend.Length, new IPEndPoint(IPAddress.Loopback, 6000));
                Thread.Sleep(100);
            }


            Console.ReadLine();
        }

        static void publisher_OnExceptionThrown(object sender, SocketExceptionEventArgs e)
        {

        }

        static void publisher_OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            Console.WriteLine(StringHelper.GetStringFromAscii(e.Buffer, 0, e.Buffer.Length));
        }
    }
}