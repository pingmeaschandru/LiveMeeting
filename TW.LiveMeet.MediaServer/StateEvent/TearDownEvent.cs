using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    public class TearDownEvent : EventBase, IEventMessage, IRequest, IResponse
    {
        public string TypeOfEvent
        {
            get { return EventType.TEARDOWN; }
        }
    }
}