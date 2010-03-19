//using System;
//using System.Net;
//using System.Net.Sockets;

//namespace TW.Core.Sockets
//{
//    public class UdpSocketEndpoint
//    {
//        public event EventHandler<SocketMessageEventArgs> OnDataRecieved = delegate { };
//        public event EventHandler<SocketExceptionEventArgs> OnExceptionThrown = delegate { };
//        protected Socket socket;
//        protected byte[] buffer;

//        public UdpSocketEndpoint(EndPoint endPoint)
//        {
//            buffer = new byte[80000];
//            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
//            ConnectionId = Guid.NewGuid().ToString();
//            socket.Bind(endPoint);

//            EndPoint ipeSender = new IPEndPoint(IPAddress.Any, 0);
//            socket.BeginReceiveFrom(byteData, 0, byteData.Length, SocketFlags.None, ref ipeSender, OnReceive, ipeSender);     

//            Receive();
//        }

//        public void Connect(IPEndPoint ipEndpoint)
//        {
//            try
//            {
//                if (!socket.Connected)
//                {
//                    socket.Connect(ipEndpoint);
//                    Receive();
//                }
//            }
//            catch (Exception ex)
//            {
//                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
//            }
//        }

//        public void Close()
//        {
//            if (socket == null) return;

//            if (socket.Connected)
//                socket.Shutdown(SocketShutdown.Both);

//            socket.Close();
//            socket = null;
//        }

//        private void Receive()
//        {
//            try
//            {
//                socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ipeSender, OnReceive, ipeSender);  
//            }
//            catch (Exception ex)
//            {
//                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
//            }
//        }

//        private void OnReceive(IAsyncResult result)
//        {
//            var data = (byte[])result.AsyncState;

//            try
//            {
//                var numRead = socket.EndReceive(result);
//                if (0 != numRead)
//                {
//                    var buf = new byte[numRead];
//                    Array.Copy(data, 0, buf, 0, buf.Length);

//                    OnDataRecieved(this, new SocketMessageEventArgs(this, buf));

//                    Receive();
//                }
//                else
//                    throw new Exception();
//            }
//            catch (Exception ex)
//            {
//                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
//            }
//        }

//        public void Disconnect(bool reuse)
//        {
//            try
//            {
//                if (socket != null)
//                    socket.Disconnect(reuse);
//            }
//            catch (Exception ex)
//            {
//                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
//            }
//        }

//        public void Send(byte[] rawBuffer, int startIndex, int length)
//        {
//            try
//            {
//                if (rawBuffer == null)
//                    return;

//                if (socket.Connected)
//                    socket.Send(rawBuffer, startIndex, length, SocketFlags.None);
//            }
//            catch (Exception ex)
//            {
//                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
//            }
//        }

//        public string ConnectionId { get; private set;}
        
//        public EndPoint RemoteEndPoint
//        {
//            get { return socket != null ? socket.RemoteEndPoint : null; }
//        }

//        public EndPoint LocalEndPoint
//        {
//            get { return socket != null ? socket.LocalEndPoint : null; }
//        }
//    }
//}
