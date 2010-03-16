using System.Net;

namespace TW.LiveMeet.Server.Media
{
    internal class MeetingBridge : IMeetingBridge
    {
        private readonly int maxSubscribersCount;
        private IMeetingBridgeAgent publisherAgent;
        private readonly MeetingBridgeSubscribers subscriberAgent;

        public MeetingBridge(int maxSubscribersCount)
        {
            this.maxSubscribersCount = maxSubscribersCount;
            subscriberAgent = new MeetingBridgeSubscribers();
        }

        public bool Add(MeetingAgentType type, IMeetingBridgeAgent connection)
        {
            switch (type)
            {
                case MeetingAgentType.Publisher: return AddPublisher(connection);
                case MeetingAgentType.Subscriber: return AddSubscriber(connection);
            }

            return false;
        }

        public IMeetingBridgeAgent Get(MeetingAgentType type, EndPoint endPoint)
        {
            switch (type)
            {
                case MeetingAgentType.Publisher: return publisherAgent;
                case MeetingAgentType.Subscriber: return GetSubscriber(endPoint);
            }

            return null;
        }


        private bool AddPublisher(IMeetingBridgeAgent meetingBridgeAgent)
        {
            if (publisherAgent != null) return false;

            publisherAgent = meetingBridgeAgent;
            publisherAgent.OnDataRecieveEvent += SendToSubscribers;
            publisherAgent.OnCloseEvent += RemovePublisher;

            return true;
        }

        private void RemovePublisher(string connectionId)
        {
            ClosePublisher();
        }

        private void ClosePublisher()
        {
            if (publisherAgent == null) return;

            publisherAgent.OnDataRecieveEvent -= SendToSubscribers;
            publisherAgent.OnCloseEvent -= RemovePublisher;
            publisherAgent.Close();
            publisherAgent = null;
        }

        private void SendToPublisher(byte[] dataToSend)
        {
            publisherAgent.Send(dataToSend, 0, dataToSend.Length);
        }


        private bool AddSubscriber(IMeetingBridgeAgent meetingBridgeAgent)
        {
            if (subscriberAgent.Count >= maxSubscribersCount) return false;
            if (subscriberAgent.ContainsKey(meetingBridgeAgent.ConnectionId)) return true;
            var subscriber = meetingBridgeAgent;
            subscriberAgent.Add(meetingBridgeAgent.ConnectionId, subscriber);
            subscriber.OnDataRecieveEvent += SendToPublisher;
            subscriber.OnCloseEvent += RemoveSubscriber;

            return true;
        }

        private IMeetingBridgeAgent GetSubscriber(string connectionId)
        {
            IMeetingBridgeAgent socketClient;
            return !subscriberAgent.TryGetValue(connectionId, out socketClient) ? null : socketClient;
        }

        private IMeetingBridgeAgent GetSubscriber(EndPoint endPoint)
        {
            foreach (var subscriber in subscriberAgent.Values)
                if (subscriber.RemoteEndpoint.ToString() == endPoint.ToString())
                    return subscriber;

            return null;
        }

        private void RemoveSubscriber(string connectionId)
        {
            var subscriber = GetSubscriber(connectionId);
            if (subscriber == null) return;

            subscriber.OnDataRecieveEvent -= SendToPublisher;
            subscriber.OnCloseEvent -= RemoveSubscriber;
            subscriber.Close();
            subscriberAgent.Remove(connectionId);
        }

        private void SendToSubscribers(byte[] dataToSend)
        {
            foreach (var subscriberConnection in subscriberAgent.Values)
                subscriberConnection.Send(dataToSend, 0, dataToSend.Length);
        }

        
        public void Close()
        {
            ClosePublisher();

            foreach (var subscriber in subscriberAgent.Values)
                subscriber.Close();

            subscriberAgent.Clear();
        }
    }
}
