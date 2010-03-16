using System;

namespace TW.Core.Sockets
{
    public class SocketEventArgs : EventArgs
    {
        public SocketBase Socket { get; set; }

        public SocketEventArgs(SocketBase socket)
        {
            Socket = socket;
        }
    }
}