using TW.Core.Timers;

namespace TW.LiveMeet.Server.Common
{
    public interface ISessionContext
    {
        string SessionId { get; }
        IAgentSession SessionOfAgent { get; }
        void Register(IAgentSession agentSession);
        void UnRegister();
        void Process(byte[] data, int startIndex, int count);
        void Close();
        IAgentSession GetPublisherSession(string presentationUrl);
        ElapsedTimerContext ResponseTimeoutContext { get; }
        ElapsedTimerContext SessionTimeoutContext { get; }
    }
}