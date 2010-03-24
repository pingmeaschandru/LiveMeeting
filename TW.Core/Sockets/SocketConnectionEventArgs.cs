using System;

namespace TW.Core.Sockets
{
    public class SocketConnectionEventArgs : EventArgs
    {
        public SocketBase Socket { get; set; }

        public SocketConnectionEventArgs(SocketBase socket)
        {
            Socket = socket;
        }
    }
}