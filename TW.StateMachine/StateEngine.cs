using System.Collections.Generic;
using TW.Core.Collections;
using TW.Core.Threading;

namespace TW.StateMachine
{
    public class StateEngine : AThread
    {
        private readonly string name;
        private readonly Dictionary<string, State> states;
        private readonly BlockingQueue<Event> eventQueue;
        private State currentState;

        public StateEngine(string name, IEnumerable<State> states, string startStateName)
        {
            this.name = name;
            this.states = new Dictionary<string, State>();
            foreach (var stateObj in states)
                this.states.Add(stateObj.Name, stateObj);

            eventQueue = new BlockingQueue<Event>();
            SetState(startStateName);
            start();
        }

        public void Process(Event eventObj)
        {
            eventQueue.Enqueue(eventObj);
        }

        protected override void OnRun()
        {
            var eventObjs = eventQueue.Dequeue(10);
            foreach (var eventObj in eventObjs)
                currentState.Process(eventObj, this);
        }

        internal void SetState(string stateName)
        {
            states.TryGetValue(stateName, out currentState);
        }

        public void Close()
        {
            eventQueue.Clear();
            stop();
        }

        public string Name
        {
            get { return name; }
        }

        public string CurrentState
        {
            get { return currentState.Name; }
        }
    }
}