namespace TW.LiveMeet.Server.Media
{
    internal class TcpConnectionPoint : IConnectionPoint
    {
        private TcpMeetingBridgeAgentListener publisherAgentListener;
        private TcpMeetingBridgeAgentListener subscriberAgentListener;

        public TcpConnectionPoint(int maxSubscribers, int publisherListeningPort, int subscriberListeningPort)
        {
            MeetingBridgeObj = new MeetingBridge(maxSubscribers);
            publisherAgentListener = new TcpMeetingBridgeAgentListener(MeetingAgentType.Publisher, MeetingBridgeObj, publisherListeningPort);
            subscriberAgentListener = new TcpMeetingBridgeAgentListener(MeetingAgentType.Subscriber, MeetingBridgeObj, subscriberListeningPort);            
            publisherAgentListener.Start();
            subscriberAgentListener.Start();
        }

        public void Close()
        {
            if (MeetingBridgeObj != null)
            {
                MeetingBridgeObj.Close();
                MeetingBridgeObj = null;
            }

            if (publisherAgentListener != null)
            {
                publisherAgentListener.Close();
                publisherAgentListener = null;
            }

            if (subscriberAgentListener != null)
            {
                subscriberAgentListener.Close();
                subscriberAgentListener = null;
            }
        }

        public IMeetingBridge MeetingBridgeObj { get; private set; }
    }
}
