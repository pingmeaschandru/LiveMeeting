using System.Net;
using System.Net.Sockets;

namespace TW.Core.Sockets
{
    public abstract class SocketBase
    {
        public const int MAX_BUFF_SIZE = 2048;
        public const int BACKLOG = 10;

        protected Socket socket;
        protected byte[] buffer;

        public EndPoint RemoteEndPoint
        {
            get { return socket != null ? socket.RemoteEndPoint : null; }
        }

        public EndPoint LocalEndPoint
        {
            get { return socket != null ? socket.LocalEndPoint : null; }
        }
    }
}