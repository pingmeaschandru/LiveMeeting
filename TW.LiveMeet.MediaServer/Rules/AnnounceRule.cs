using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class AnnounceRule : RuleBase, IStateRule
    {
        public AnnounceRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private AnnounceEvent TypeCast(IEventMessage message)
        {
            return message is AnnounceEvent ? message as AnnounceEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.INIT;

            var agentSession = new AgentSession(eventMessage.Handler)
            {
                ConferenceId = eventMessage.ConferenceId,
                AgentName = eventMessage.AgentName,
                PublisherUrl = eventMessage.PresentationUrl,
                EndpointAddress = eventMessage.EndpointAddress,
                IsPublisher = true
            };

            sessionContext.Register(agentSession);

            agentSession.Process(new ResponseEvent
            {
                AgentName = sessionContext.SessionOfAgent.AgentName,
                ConferenceId = sessionContext.SessionOfAgent.ConferenceId,
                PresentationUrl = eventMessage.PresentationUrl,
                SessionId = sessionContext.SessionId,
                ResponseStatus = ResponseStatus.Ok
            });

            return StateType.READY;
        }
    }
}