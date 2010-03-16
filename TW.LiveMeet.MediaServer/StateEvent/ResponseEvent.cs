using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    public class ResponseEvent : EventBase, IEventMessage, IResponse
    {
        public ResponseStatus ResponseStatus { get; set; }
        public string TypeOfEvent
        {
            get { return EventType.RESPONSE; }
        }
    }
}