using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TW.Core.Sockets
{
    public class UdpSocket : SocketBase
    {
        private const int bufferSize = 4096;

        public event EventHandler<SocketMessageEventArgs> OnDataRecieved = delegate { };
        public event EventHandler<SocketExceptionEventArgs> OnExceptionThrown = delegate { };

        private Thread receiveThread;
        private volatile bool receiving;

        public UdpSocket(EndPoint localEndPoint)
            : base(bufferSize, new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.Bind(localEndPoint);
            receiving = true;
            receiveThread = new Thread(Receive)
                                {
                                    Name = "DatagramSocket.ReceiveThread " + localEndPoint
                                };
            receiveThread.Start();
        }

        public UdpSocket(Socket socket)
            : base(bufferSize, socket)
        {
            receiving = true;
            receiveThread = new Thread(Receive)
                                {
                                    Name = "DatagramSocket.ReceiveThread " + socket.LocalEndPoint
                                };
            receiveThread.Start();
        }

        private void Receive()
        {
            try
            {
                var sourceEp = new IPEndPoint(IPAddress.Any, 0);
                var tempEp = (EndPoint)sourceEp;
                while (receiving)
                {
                    if (!socket.Poll(10000, SelectMode.SelectRead)) continue;
                    var numRead = socket.ReceiveFrom(Buffer, 0, Buffer.Length, SocketFlags.None, ref tempEp);
                    if (numRead <= 0) continue;

                    var buf = new byte[numRead];
                    Array.Copy(Buffer, 0, buf, 0, buf.Length);

                    if (OnDataRecieved != null)
                        OnDataRecieved(this, new SocketMessageEventArgs(buf, tempEp, LocalEndPoint));
                }
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public void Send(byte[] dataToBytes, int startIndex, int count, IPEndPoint remoteEp)
        {
            try
            {
                if (socket == null) return;
                socket.SendTo(dataToBytes, 0, dataToBytes.Length, SocketFlags.None, remoteEp);
            }
            catch (Exception ex)
            {
                OnExceptionThrown(this, new SocketExceptionEventArgs(ex));
            }
        }

        public void Close()
        {
            if (socket == null)
                return;

            receiving = false;

            receiveThread.Join();
            receiveThread = null;

            socket.Close();
            socket = null;
        }
    }
}
