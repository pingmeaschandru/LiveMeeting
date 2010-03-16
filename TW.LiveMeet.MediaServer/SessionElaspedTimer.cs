using TW.Core.Timers;
using TW.LiveMeet.Server.Streaming.StateEvent;
using TW.StateMachine;

namespace TW.LiveMeet.Server.Streaming
{
    internal class SessionElaspedTimer
    {
        private ElaspedTimer elaspedTimer;
        private readonly StateEngine agentStateEngine;
        private readonly SessionContext sessionContext;

        public SessionElaspedTimer(StateEngine agentStateEngine, SessionContext sessionContext)
        {
            this.agentStateEngine = agentStateEngine;
            this.sessionContext = sessionContext;
            elaspedTimer = new ElaspedTimer(SessionTimerConstants.TIMER_ELASPED_TIME, OnTimeElaspedHandler);
        }

        public void Close()
        {
            if (elaspedTimer == null) return;
            elaspedTimer.Close();
            elaspedTimer = null;
        }

        private void OnTimeElaspedHandler()
        {
            lock (agentStateEngine)
            {
                if (sessionContext.SessionTimeoutContext.IsExpired())
                {
                    agentStateEngine.Process(new Event(EventType.TIME_OUT, new TimeOutEvent
                                                                               {
                                                                                   AgentName = sessionContext.SessionOfAgent.AgentName,
                                                                                   //SessionId = sessionContext.SessionId,
                                                                                   ConferenceId = sessionContext.SessionOfAgent.ConferenceId,
                                                                                   Reason = TimeOutReason.SessionTimeout
                                                                               }));

                } 
                else if (sessionContext.ResponseTimeoutContext.IsExpired())
                {
                    agentStateEngine.Process(new Event(EventType.TIME_OUT, new TimeOutEvent
                                                                               {
                                                                                   AgentName = sessionContext.SessionOfAgent.AgentName,
                                                                                   //SessionId = sessionContext.SessionId,
                                                                                   ConferenceId = sessionContext.SessionOfAgent.ConferenceId,
                                                                                   Reason = TimeOutReason.ResponseReceivedTimeout
                                                                               }));
                }
            }
        }
    }
}