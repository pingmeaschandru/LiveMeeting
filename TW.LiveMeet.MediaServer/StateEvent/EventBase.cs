namespace TW.LiveMeet.Server.Streaming.StateEvent
{
    public abstract class EventBase
    {
        public string AgentName { get; set; }
        public string SessionId { get; set; }
        public string ConferenceId { get; set; }
        public string PresentationUrl { get; set; }
    }
}