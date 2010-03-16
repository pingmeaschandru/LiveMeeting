using System.Net;
using TW.Core.Sockets;

namespace TW.LiveMeet.Server.Common.Connection.Tcp
{
    public class TcpConnection : IConnection
    {
        private TcpSocketClient socketClient;
        private readonly ISessionContext sessionContext;
        private readonly IRegistrator<string, IConnection> registrator;

        public TcpConnection(TcpSocketClient socketClient, ISessionContext sessionContext, IRegistrator<string, IConnection> registrator)
        {
            this.socketClient = socketClient;
            this.sessionContext = sessionContext;
            this.registrator = registrator;
            this.socketClient.OnDataRecieved += OnDataRecieved;
            this.socketClient.OnExceptionThrown += OnSocketClientExceptionThrown;
        }

        private void OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            sessionContext.Process(e.Buffer, 0, e.Buffer.Length);
        }

        private void OnSocketClientExceptionThrown(object sender, SocketExceptionEventArgs e)
        {
            Close();
        }

        public void Send(byte[] dataToWrite, int startIndex, int count)
        {
            socketClient.Send(dataToWrite, startIndex, count);
        }

        public void Close()
        {
            if (socketClient == null) return;

            registrator.Remove(socketClient.ConnectionId);
            
            socketClient.OnDataRecieved -= OnDataRecieved;
            socketClient.OnExceptionThrown -= OnSocketClientExceptionThrown;
            socketClient.Close();
            socketClient = null;

            sessionContext.Close();
        }

        public EndPoint LocalEndPoint
        {
            get { return socketClient.LocalEndPoint; }
        }

        public string ConnectionId
        {
            get { return socketClient.ConnectionId; }
        }

        public EndPoint RemoteEndPoint
        {
            get { return socketClient.RemoteEndPoint; }
        }
    }
}