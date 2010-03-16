using System.Net;
using TW.Core.Sockets;

namespace TW.LiveMeet.Server.Media
{
    internal class TcpMeetingBridgeAgentListener
    {
        private readonly MeetingAgentType type;
        private readonly IMeetingBridge meetingBridge;
        private readonly int port;
        private TcpSocketServer tcpListener;

        public TcpMeetingBridgeAgentListener(MeetingAgentType type, IMeetingBridge meetingBridge, int port)
        {
            this.type = type;
            this.meetingBridge = meetingBridge;
            this.port = port;
            tcpListener = new TcpSocketServer();
        }

        public void Start()
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

            if (!meetingBridge.Add(type, new TcpMeetingBridgeAgent(tcpSocketClient)))
                tcpSocketClient.Disconnect(false);
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