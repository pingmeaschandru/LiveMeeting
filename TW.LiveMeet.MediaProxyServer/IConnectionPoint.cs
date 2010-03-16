namespace TW.LiveMeet.Server.Media
{
    public interface IConnectionPoint
    {
        IMeetingBridge MeetingBridgeObj { get; }
        void Close();
    }
}