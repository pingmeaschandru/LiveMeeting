namespace TW.LiveMeet.Server.Common
{
    public interface IRequest
    {
        string AgentName { get; }
        string TypeOfEvent { get; }
    }
}