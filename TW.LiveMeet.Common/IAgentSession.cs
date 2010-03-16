using System.Net;

namespace TW.LiveMeet.Server.Common
{
    public interface IAgentSession
    {
        string AgentName { get; set; }
        string ConferenceId { get; set; }
        string PublisherUrl { get; set; }
        IPEndPoint EndpointAddress { get; set; }
        bool IsPublisher { get; set; }
        void Process(IResponse response);
    }
}