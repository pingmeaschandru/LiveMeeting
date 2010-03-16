namespace TW.LiveMeet.Server.Common
{
    public interface IResponse
    {
        string AgentName { get; }
        string TypeOfEvent { get; }
    }
}