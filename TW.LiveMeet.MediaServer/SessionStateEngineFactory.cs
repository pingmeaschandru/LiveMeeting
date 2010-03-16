using System.Collections.Generic;
using TW.LiveMeet.Server.Streaming.Rules;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming
{
    internal class SessionStateEngineFactory
    {
        public static StateEngine Create(SessionContext sessionContext)
        {
            var eventRulePairs = new List<EventRulePair>
                                     {
                                         new EventRulePair(EventType.PLAY, new PlayRule(sessionContext)),
                                         new EventRulePair(EventType.SETUP, new SetupRule(sessionContext)),
                                         new EventRulePair(EventType.TEARDOWN, new TearDownRule(sessionContext)),
                                         new EventRulePair(EventType.TIME_OUT, new TimeOutRule(sessionContext)),
                                         new EventRulePair(EventType.ANNOUNCE, new AnnounceRule(sessionContext))
                                     };

            var initState = new State(StateType.INIT, eventRulePairs);
            var readyState = new State(StateType.READY, eventRulePairs);
            var playingState = new State(StateType.PLAYING, eventRulePairs);

            return new StateEngine("SessionStateEngine",
                                   new List<State>
                                       {
                                           initState,
                                           readyState,
                                           playingState
                                       }, 
                                   StateType.INIT);
        }
    }
}