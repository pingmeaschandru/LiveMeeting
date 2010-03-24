using System;
using System.Net;

namespace TW.Core.Sockets
{
    public class SocketMessageEventArgs : EventArgs
    {
        public EndPoint RemoteEndPoint { get; set; }
        public EndPoint LocalEndPoint { get; set; }
        public byte[] Buffer { get; set; }

        public SocketMessageEventArgs(byte[] buffer, EndPoint remoteEndPoint, EndPoint localEndPoint)
        {
            RemoteEndPoint = remoteEndPoint;
            LocalEndPoint = localEndPoint;
            Buffer = buffer;
        }
    }
}