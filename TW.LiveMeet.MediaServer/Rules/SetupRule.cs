using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class SetupRule : RuleBase, IStateRule
    {
        public SetupRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private SetupEvent TypeCast(IEventMessage message)
        {
            return message is SetupEvent ? message as SetupEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.INIT;

            var agentSession = new AgentSession(eventMessage.Handler)
                                   {
                                       AgentName = eventMessage.AgentName,
                                       ConferenceId = eventMessage.ConferenceId,
                                       PublisherUrl = eventMessage.PresentationUrl,
                                       EndpointAddress = eventMessage.EndpointAddress,
                                       IsPublisher = false
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

        protected override string OnPlayingState(IEventMessage message)
        {
            return StateType.PLAYING;
        }

        protected override string OnReadyState(IEventMessage message)
        {
            return StateType.READY;
        }
    }
}