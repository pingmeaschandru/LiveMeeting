using System;
using System.Net;
using TW.Core.Sockets;

namespace TW.LiveMeet.Server.Media
{
    public class UdpMeetingBridgeAgent : IMeetingBridgeAgent
    {
        public event Action<byte[]> OnDataRecieveEvent = delegate { };
        public event Action<string> OnCloseEvent = delegate { };

        private readonly string connectionId;
        private readonly IPEndPoint remoteEp;
        private UdpSocket socketClient;

        public UdpMeetingBridgeAgent(UdpSocket socketClient, IPEndPoint remoteEp)
        {
            connectionId = socketClient.ConnectionId;
            DoSend = true;
            this.socketClient = socketClient;
            this.remoteEp = remoteEp;
            this.socketClient.OnDataRecieved += OnDataRecieved;
            this.socketClient.OnExceptionThrown += OnException;
        }

        public void Send(byte[] dataToSend, int startIndex, int count)
        {
            if (DoSend) socketClient.Send(dataToSend, startIndex, count, remoteEp);
        }

        private void OnException(object sender, SocketExceptionEventArgs e)
        {
            if (sender == socketClient)
                OnCloseEvent(ConnectionId);
        }

        private void OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            if (e.RemoteEndPoint.ToString() != remoteEp.ToString())
                return;

            OnDataRecieveEvent(e.Buffer);
        }

        public void Close()
        {
            if (socketClient == null) return;

            socketClient.OnDataRecieved -= OnDataRecieved;
            socketClient.OnExceptionThrown -= OnException;
            socketClient.Close();
            socketClient = null;
        }

        public string ConnectionId
        {
            get { return connectionId; }
        }

        public bool DoSend { get; set; }
        public EndPoint RemoteEndpoint
        {
            get { return socketClient.RemoteEndPoint; }
        }
    }
}