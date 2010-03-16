using System;

namespace TW.Core.Sockets
{
    public class SocketExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public SocketExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
