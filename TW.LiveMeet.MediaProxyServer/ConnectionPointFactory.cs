namespace TW.LiveMeet.Server.Media
{
    public class ConnectionPointFactory
    {
        public static IConnectionPoint CreateTcpConnectionPoint(int maxSubscriberCount, int publisherListeningPort, int subscriberListeningPort)
        {
            return new TcpConnectionPoint(5, publisherListeningPort, subscriberListeningPort);
        }
    }
}
