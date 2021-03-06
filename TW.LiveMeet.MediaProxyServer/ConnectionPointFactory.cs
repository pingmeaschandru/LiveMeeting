﻿namespace TW.LiveMeet.Server.Media
{
    public class ConnectionPointFactory
    {
        public static IConnectionPoint CreateTcpConnectionPoint(int maxSubscriberCount, int publisherListeningPort, int subscriberListeningPort)
        {
            return new TcpConnectionPoint(maxSubscriberCount, publisherListeningPort, subscriberListeningPort);
        }

        public static IConnectionPoint CreateUdpConnectionPoint(int maxSubscriberCount)
        {
            return new UdpConnectionPoint(maxSubscriberCount);
        }
    }
}
