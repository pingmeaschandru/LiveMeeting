using System;
using System.Net;
using System.Net.Sockets;

namespace TW.Core.Sockets
{
    public class TcpSocketClient : SocketBase
    {
        public event EventHandler<SocketMessageEventArgs> OnDataRecieved = delegate { };
        public event EventHandler<SocketExceptionEventArgs> OnExceptionThrown = delegate { };

        public TcpSocketClient()
        {
            buffer = new byte[80000];
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ConnectionId = Guid.NewGuid().ToString();
        }

        public TcpSocketClient(Socket clientSocket)
        {
            buffer = new byte[80000];
            socket = clientSocket;
            ConnectionId = Guid.NewGuid().ToString();
            Receive();
        }

        public void Connect(IPEndPoint ipEndpoint)
        {
            try
            {
                if (!socket.Connected)
                {
                    socket.Connect(ipEndpoint);
                    Receive();
                }
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
        }

        private void Receive()
        {
            try
            {
                socket.BeginReceive(buffer, 
                    0, 
                    buffer.Length, 
                    SocketFlags.None, 
                    new AsyncCallback(OnReceive), 
                    buffer);
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        private void OnReceive(IAsyncResult result)
        {
            var data = (byte[])result.AsyncState;

            try
            {
                var numRead = socket.EndReceive(result);
                if (0 != numRead)
                {
                    var buf = new byte[numRead];
                    Array.Copy(data, 0, buf, 0, buf.Length);

                    OnDataRecieved(this, new SocketMessageEventArgs(this, buf));

                    Receive();
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public void Disconnect(bool reuse)
        {
            try
            {
                if (socket != null)
                    socket.Disconnect(reuse);
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public void Send(byte[] rawBuffer, int startIndex, int length)
        {
            try
            {
                if (rawBuffer == null)
                    return;

                if (socket.Connected)
                    socket.Send(rawBuffer, startIndex, length, SocketFlags.None);
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public string ConnectionId { get; private set;}
    }
}