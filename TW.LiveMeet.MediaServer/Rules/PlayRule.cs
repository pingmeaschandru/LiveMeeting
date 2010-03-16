using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class PlayRule : RuleBase, IStateRule
    {
        public PlayRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private PlayEvent TypeCast(IEventMessage message)
        {
            return message is PlayEvent ? message as PlayEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            throw new RuleException();
        }

        protected override string OnReadyState(IEventMessage message)
        {
            var eventMessage = TypeCast(message);
            if (eventMessage == null) return StateType.READY;

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

            return StateType.PLAYING;
        }

        protected override string OnPlayingState(IEventMessage message)
        {
            return StateType.PLAYING;
        }
    }
}