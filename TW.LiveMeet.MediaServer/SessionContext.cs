using TW.Core.Timers;
using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming
{
    public class SessionContext : ISessionContext
    {
        private readonly IRegistrator<string, ISessionContext> publisherRegistrator;
        private StateEngine sessionStateEngine;
        private SessionElaspedTimer sessionElapsedTimer;

        public SessionContext(string sessionId, IRequestHandler requestHandler, IRegistrator<string, ISessionContext> publisherRegistrator)
        {
            this.publisherRegistrator = publisherRegistrator;
            SessionId = sessionId;
            RequestHandler = requestHandler;
            sessionStateEngine = SessionStateEngineFactory.Create(this);
            SessionTimeoutContext = new ElapsedTimerContext(SessionTimerConstants.SESSION_TIME_OUT);
            ResponseTimeoutContext = new ElapsedTimerContext(SessionTimerConstants.RESPONSE_TIME_OUT);
            sessionElapsedTimer = new SessionElaspedTimer(sessionStateEngine, this);
            UnRegister();
        }

        public void Register(IAgentSession agentSession)
        {
            SessionOfAgent = agentSession;
            if (SessionOfAgent.IsPublisher)
                publisherRegistrator.Add(SessionOfAgent.PublisherUrl, this);

            SessionTimeoutContext.StopWatchObj.Stop();
        }

        public void UnRegister()
        {
            if (SessionOfAgent.IsPublisher)
                publisherRegistrator.Remove(SessionOfAgent.PublisherUrl);

            SessionOfAgent = null;
            SessionTimeoutContext.StopWatchObj.Restart();
        }

        public void Process(byte[] data, int startIndex, int count)
        {
            lock (sessionStateEngine)
            {
                var request = RequestHandler.Process(data, 0, data.Length);
                if (request != null)
                    sessionStateEngine.Process(new Event(request.TypeOfEvent, request as IEventMessage));
            }
        }

        public void Close()
        {
            if (sessionElapsedTimer == null) return;

            UnRegister();
            sessionElapsedTimer.Close();
            sessionElapsedTimer = null;

            sessionStateEngine.Close();
            sessionStateEngine = null;
        }

        public IAgentSession GetPublisherSession(string presentationUrl)
        {
            ISessionContext sessionContext;
            return !publisherRegistrator.TryGetValue(SessionOfAgent.PublisherUrl, out sessionContext) ? null : sessionContext.SessionOfAgent;
        }

        public IRequestHandler RequestHandler { get; set; }
        public IAgentSession SessionOfAgent { get; private set; }
        public ElapsedTimerContext ResponseTimeoutContext { get; private set; }
        public ElapsedTimerContext SessionTimeoutContext { get; private set; }
        public string SessionId { get; private set; }
    }
}