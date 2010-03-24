using System;
using System.Net;
using System.Net.Sockets;

namespace TW.Core.Sockets
{
    public abstract class SocketBase
    {
        protected Socket socket;

        protected SocketBase(int buffSize, Socket socket)
        {
            Buffer = new byte[buffSize];
            this.socket = socket;
            ConnectionId = Guid.NewGuid().ToString();
        }

        public EndPoint RemoteEndPoint
        {
            get { return socket != null ? socket.RemoteEndPoint : null; }
        }

        public EndPoint LocalEndPoint
        {
            get { return socket != null ? socket.LocalEndPoint : null; }
        }

        public byte[] Buffer { get; protected set; }
        public string ConnectionId { get; protected set; }
    }
}