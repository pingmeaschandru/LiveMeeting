using System.Net;

namespace TW.LiveMeet.Server.Media
{
    public interface IMeetingBridge
    {
        bool Add(MeetingAgentType type, IMeetingBridgeAgent connection);
        IMeetingBridgeAgent Get(MeetingAgentType type, EndPoint endPoint);
        void Close();
    }
}