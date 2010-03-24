using System;
using System.Net;
using TW.Core.Sockets;
using TW.LiveMeet.Server.Media;

namespace UdpMockServer
{
    public class Program
    {
        static void Main()
        {
            var publisherSocket = new UdpSocket(new IPEndPoint(IPAddress.Loopback, 5000));
            var subscriberSocket = new UdpSocket(new IPEndPoint(IPAddress.Loopback, 6000));


            var connectionPoint = ConnectionPointFactory.CreateUdpConnectionPoint(8);
            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Publisher,
                                                 new UdpMeetingBridgeAgent(publisherSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 5001)));


            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
                                                 new UdpMeetingBridgeAgent(subscriberSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 6001)));

            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
                                                 new UdpMeetingBridgeAgent(subscriberSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 6002)));

            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
                                                 new UdpMeetingBridgeAgent(subscriberSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 6003)));

            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
                                                 new UdpMeetingBridgeAgent(subscriberSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 6004)));

            connectionPoint.MeetingBridgeObj.Add(MeetingAgentType.Subscriber,
                                                 new UdpMeetingBridgeAgent(subscriberSocket,
                                                                           new IPEndPoint(IPAddress.Loopback, 6005)));


            Console.WriteLine("Server Listening in 5000......");

            Console.ReadLine();
        }
    }
}