using System;
using System.Collections.Generic;
using TW.LiveMeet.Server.Common;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming.Rules
{
    internal abstract class RuleBase
    {
        protected readonly ISessionContext sessionContext;
        private readonly Dictionary<string, Func<IEventMessage, string>> actionPool;

        protected RuleBase(ISessionContext sessionContext)
        {
            this.sessionContext = sessionContext;
            actionPool = new Dictionary<string, Func<IEventMessage, string>>
                             {
                                 {StateType.INIT, OnInitState},
                                 {StateType.PLAYING, OnPlayingState},
                                 {StateType.READY, OnReadyState},
                             };
        }

        public string Action(IEventMessage message, string currentState)
        {
            Func<IEventMessage, string> handler;
            return !actionPool.TryGetValue(currentState, out handler) ? currentState : handler(message);
        }

        protected virtual string OnInitState(IEventMessage message) { return StateType.INIT; }
        protected virtual string OnReadyState(IEventMessage message) { return StateType.READY; }
        protected virtual string OnPlayingState(IEventMessage message) { return StateType.PLAYING; }
    }
}