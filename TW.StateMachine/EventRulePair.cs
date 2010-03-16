namespace TW.StateMachine
{
    public class EventRulePair
    {
        public string EventName{ get; set; }
        public IStateRule StateRule { get; set; }

        public EventRulePair(string eventName, IStateRule stateRule)
        {
            EventName = eventName;
            StateRule = stateRule;
        }
    }
}