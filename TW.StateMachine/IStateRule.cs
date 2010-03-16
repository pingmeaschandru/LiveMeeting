namespace TW.StateMachine
{
    public interface IStateRule
    {
        string Action(IEventMessage message, string currentState);
    }
}
