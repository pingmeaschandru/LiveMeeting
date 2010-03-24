using System;
using TW.LiveMeet.Server.Media;

namespace MockServer
{
    class Program
    {
        static void Main()
        {
            Console.Write("Enter MeetingId : ");
            var meetingId = Console.ReadLine();

            Console.Write("Enter Publisher Port : ");
            var strPort = Console.ReadLine();
            var publiherPort = int.Parse(strPort);

            Console.Write("Enter Subscriber Port : ");
            strPort = Console.ReadLine();
            var subscriberPort = int.Parse(strPort);

            Console.Write("Enter Number of subscriber : ");
            var strNum = Console.ReadLine();
            var subscriberCount = int.Parse(strNum);

            var tcpConnectionPoint = ConnectionPointFactory.CreateTcpConnectionPoint(subscriberCount, publiherPort, subscriberPort);
            var registrator =  new ConnectionPointRegistrator { { meetingId, tcpConnectionPoint } };

            Console.WriteLine("Server Listening in {0}-{1}......", publiherPort, subscriberPort);

            Console.WriteLine("Press Enter To Exit.....");

            Console.ReadLine();

            tcpConnectionPoint.Close();
            registrator.Clear();

            //var publisherSocket = new UdpSocket(new IPEndPoint(IPAddress.Loopback, 5000));
            //var subscriberSocket = new UdpSocket(new IPEndPoint(IPAddress.Loopback, 6000));


            //var connectionPoint = ConnectionPointFactory.CreateUdpConnectionPoint(8);
            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Publisher,
            //                                     new UdpMeetingBridgeAgent(publisherSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 5001)));


            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
            //                                     new UdpMeetingBridgeAgent(subscriberSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 6001)));

            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
            //                                     new UdpMeetingBridgeAgent(subscriberSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 6002)));

            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
            //                                     new UdpMeetingBridgeAgent(subscriberSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 6003)));

            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
            //                                     new UdpMeetingBridgeAgent(subscriberSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 6004)));

            //connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
            //                                     new UdpMeetingBridgeAgent(subscriberSocket,
            //                                                               new IPEndPoint(IPAddress.Loopback, 6005)));


            //Console.WriteLine("Server Listening in 5000......");

            //Console.ReadLine();
        }
    }
}
