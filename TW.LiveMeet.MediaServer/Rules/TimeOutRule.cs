using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal class TimeOutRule : RuleBase, IStateRule
    {
        public TimeOutRule(ISessionContext sessionContext)
            : base(sessionContext)
        {
        }

        private TimeOutEvent TypeCast(IEventMessage message)
        {
            return message is TearDownEvent ? message as TimeOutEvent : null;
        }

        protected override string OnInitState(IEventMessage message)
        {
            sessionContext.UnRegister();
            return StateType.END;
        }

        protected override string OnPlayingState(IEventMessage message)
        {
            return StateType.END;
        }

        protected override string OnReadyState(IEventMessage message)
        {
            return StateType.END;
        }
    }
}