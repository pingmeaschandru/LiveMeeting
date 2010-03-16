using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    internal class TimeOutEvent: EventBase, IEventMessage, IRequest, IResponse
    {
        public TimeOutReason Reason { get; set; }

        public string TypeOfEvent
        {
            get { return EventType.TIME_OUT; }
        }
    }
}