using System.Net;
using TW.LiveMeet.Server.Common;

namespace TW.LiveMeet.Server.Streaming
{
    internal class AgentSession : IAgentSession
    {
        private readonly IResponseHandler handler;

        public AgentSession(IResponseHandler handler)
        {
            this.handler = handler;
        }

        public void Process(IResponse response)
        {
            handler.Process(response);
        }

        public string AgentName { get; set; }
        public string ConferenceId { get; set; }
        public string PublisherUrl { get; set; }
        public IPEndPoint EndpointAddress { get; set; }
        public bool IsPublisher { get; set; }
    }
}