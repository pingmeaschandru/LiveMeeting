namespace TW.LiveMeet.Server.Media
{
    internal class UdpConnectionPoint : IConnectionPoint
    {
        public UdpConnectionPoint(int maxSubscribers)
        {
            MeetingBridgeObj = new MeetingBridge(maxSubscribers);
        }

        public void Close()
        {
            if (MeetingBridgeObj == null) return;
            MeetingBridgeObj.Close();
            MeetingBridgeObj = null;
        }

        public IMeetingBridge MeetingBridgeObj { get; private set; }
    }
}
