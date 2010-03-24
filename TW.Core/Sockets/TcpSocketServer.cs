using System;
using System.Net;
using System.Net.Sockets;

namespace TW.Core.Sockets
{
    public class TcpSocketServer
    {
        private readonly int buffSize;
        public const int BACKLOG = 10;

        public event EventHandler<SocketConnectionEventArgs> OnNewConnection = delegate { };
        public event EventHandler<SocketExceptionEventArgs> OnExceptionThrown = delegate { };

        protected Socket socket;

        public TcpSocketServer(int buffSize)
        {
            this.buffSize = buffSize;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {NoDelay = true};

            IsListening = false;
        }

        public void Listen(IPEndPoint endpoint)
        {
            try
            {
                if (IsListening) return;

                socket.Bind(endpoint);
                socket.Listen(BACKLOG);

                IsListening = true;

                Accept();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public void Close()
        {
            if (socket == null) return;

            if (socket.Connected)
                socket.Shutdown(SocketShutdown.Both);

            socket.Close();
            socket = null;
            IsListening = false;
        }

        private void Accept()
        {
            try
            {
                socket.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        private void OnAccept(IAsyncResult result)
        {
            try
            {
                var clientSocket = socket.EndAccept(result);

                if (clientSocket.Connected)
                {
                    var tcpsocket = new TcpSocketClient(buffSize, clientSocket);

                    if (OnNewConnection != null)
                        OnNewConnection(this, new SocketConnectionEventArgs(tcpsocket));
                }

                Accept();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public bool IsListening { get; private set; }
    }
}