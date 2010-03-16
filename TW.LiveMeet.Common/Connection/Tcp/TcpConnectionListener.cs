using System.Net;
using TW.Core.Sockets;

namespace TW.LiveMeet.Server.Common.Connection.Tcp
{
    public class TcpConnectionListener 
    {
        private TcpSocketServer tcpListener;
        private readonly IRegistrator<string, IConnection> registrator;
        private readonly IConnectionFactory<TcpSocketClient> connectionFactory;

        public TcpConnectionListener(IConnectionFactory<TcpSocketClient> connectionFactory, IRegistrator<string, IConnection> registrator)
        {
            this.registrator = registrator;
            this.connectionFactory = connectionFactory;
            tcpListener = new TcpSocketServer();
        }

        public void Start(int port)
        {
            tcpListener.OnNewConnection += OnNewConnection;
            tcpListener.OnExceptionThrown += OnSocketServerExceptionThrown;
            tcpListener.Listen(new IPEndPoint(IPAddress.Any, port));
        }

        protected virtual void OnSocketServerExceptionThrown(object sender, SocketExceptionEventArgs e)
        {
            Close();
        }

        private void OnNewConnection(object sender, SocketEventArgs e)
        {
            var tcpSocketClient = e.Socket as TcpSocketClient;
            if (tcpSocketClient == null)
                return;

            var connectionContext = connectionFactory.Create(tcpSocketClient);
            registrator.Add(tcpSocketClient.ConnectionId, connectionContext);
        }

        public void Close()
        {
            if (tcpListener == null) return;

            tcpListener.OnNewConnection -= OnNewConnection;
            tcpListener.OnExceptionThrown -= OnSocketServerExceptionThrown;
            tcpListener.Close();
            tcpListener = null;
        }
    }
}