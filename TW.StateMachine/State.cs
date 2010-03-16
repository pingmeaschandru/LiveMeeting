using System.Collections.Generic;

namespace TW.StateMachine
{
    public class State
    {
        private readonly string name;
        private readonly Dictionary<string, IStateRule> stateRules;

        public State(string name, IEnumerable<EventRulePair> eventRules)
        {
            this.name = name;
            stateRules = new Dictionary<string, IStateRule>();
            foreach (var rule in eventRules)
                stateRules.Add(rule.EventName, rule.StateRule);
        }

        public void Process(Event eventObj, StateEngine engineObj)
        {
            IStateRule ruleObj;
            if (!stateRules.TryGetValue(eventObj.Name, out ruleObj)) throw new UnKnownStateEventException();
            engineObj.SetState(ruleObj.Action(eventObj.Message, name));
        }

        public string Name
        {
            get { return name; }
        }
    }
}