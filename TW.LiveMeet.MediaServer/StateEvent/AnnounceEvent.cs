using System.Net;
using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    public class AnnounceEvent : EventBase, IEventMessage, IRequest, IResponse
    {
        public IPEndPoint EndpointAddress { get; set; }
        public MediaProtocolType TypeOfProtocol { get; set; }
        public IResponseHandler Handler { get; set; }

        public string TypeOfEvent
        {
            get { return EventType.ANNOUNCE; }
        }
    }
}
