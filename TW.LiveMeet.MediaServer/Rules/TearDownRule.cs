using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class TearDownRule : RuleBase, IStateRule
    {
        public TearDownRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private TearDownEvent TypeCast(IEventMessage message)
        {
            return message is TearDownEvent ? message as TearDownEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            return StateType.INIT;
        }

        protected override string OnPlayingState(IEventMessage message)
        {
            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.PLAYING;

            var agentSession = sessionContext.GetPublisherSession(eventMessage.PresentationUrl);
            agentSession.Process(eventMessage);

            sessionContext.SessionOfAgent.Process(new ResponseEvent
            {
                AgentName = sessionContext.SessionOfAgent.AgentName,
                ConferenceId = sessionContext.SessionOfAgent.ConferenceId,
                PresentationUrl = eventMessage.PresentationUrl,
                SessionId = sessionContext.SessionId,
                ResponseStatus = ResponseStatus.Continue
            });

            sessionContext.ResponseTimeoutContext.StopWatchObj.Start();

            return StateType.INIT;
        }

        protected override string OnReadyState(IEventMessage message)
        {
            return StateType.INIT;
        }
    }
}