using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class ResponseRule: RuleBase, IStateRule
    {
        public ResponseRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private ResponseEvent TypeCast(IEventMessage message)
        {
            return message is ResponseEvent ? message as ResponseEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            sessionContext.ResponseTimeoutContext.StopWatchObj.Stop();

            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.READY;

            var agentSession = sessionContext.GetPublisherSession(eventMessage.PresentationUrl);
            agentSession.Process(eventMessage);

            return StateType.INIT;
        }

        protected override string OnPlayingState(IEventMessage message)
        {
            sessionContext.ResponseTimeoutContext.StopWatchObj.Stop();

            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.READY;

            var agentSession = sessionContext.GetPublisherSession(eventMessage.PresentationUrl);
            agentSession.Process(eventMessage);

            return StateType.PLAYING;
        }

        protected override string OnReadyState(IEventMessage message)
        {
            sessionContext.ResponseTimeoutContext.StopWatchObj.Stop();

            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.READY;

            var agentSession = sessionContext.GetPublisherSession(eventMessage.PresentationUrl);
            agentSession.Process(eventMessage);

            return StateType.READY;
        }
    }
}