using System;
using TW.LiveMeet.Server.Media;

namespace MockServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new ConnectionPointRegistrator
                {{"2121212121", ConnectionPointFactory.CreateTcpConnectionPoint(5, 5000, 5001)}};

            Console.WriteLine("Server Listening in 5000......");

            Console.ReadLine();
        }
    }
}
