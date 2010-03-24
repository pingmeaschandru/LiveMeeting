using System;
using System.Net;
using TW.Core.IO;
using TW.Core.Sockets;
using TW.LiveMeet.RDAP;
using TW.LiveMeet.RDAP.Parser;

namespace TW.LiveMeet.Server.Media
{
    internal class TcpMeetingBridgeAgent : IMeetingBridgeAgent
    {
        public event Action<byte[]> OnDataRecieveEvent = delegate { };
        public event Action<string> OnCloseEvent = delegate { };
        
        private readonly FifoStream parserStream;
        private readonly RdapMessageParser parser;
        private readonly string connectionId;
        private TcpSocketClient socketClient;

        public TcpMeetingBridgeAgent(TcpSocketClient socketClient)
        {
            parserStream = new FifoStream();
            parser = new RdapMessageParser(parserStream);
            connectionId = socketClient.ConnectionId;
            DoSend = true;
            this.socketClient = socketClient;
            this.socketClient.OnDataRecieved += OnDataRecieved;
            this.socketClient.OnExceptionThrown += OnException;
        }

        public void Send(byte[] dataToSend, int startIndex, int count)
        {
            if (DoSend) socketClient.Send(dataToSend, startIndex, count);
        }

        private void OnException(object sender, SocketExceptionEventArgs e)
        {
            if (sender == socketClient)
                OnCloseEvent(ConnectionId);
        }

        private void OnDataRecieved(object sender, SocketMessageEventArgs e)
        {
            parserStream.Write(e.Buffer, 0, e.Buffer.Length);
            RdapMessage rdapMessage;
            if (!parser.TryParseMessage(out rdapMessage)) return;
            OnDataRecieveEvent(rdapMessage.ToBytes());
        }

        public void Close()
        {
            if (socketClient == null) return;

            Console.WriteLine("Closed connection, RemoteEp : " + socketClient.RemoteEndPoint + " , LocalEp : " + socketClient.LocalEndPoint);

            socketClient.OnDataRecieved -= OnDataRecieved;
            socketClient.OnExceptionThrown -= OnException;
            socketClient.Close();
            socketClient = null;
        }

        public string ConnectionId
        {
            get { return connectionId; }
        }

        public bool DoSend { get; set;}
        public EndPoint RemoteEndpoint
        {
            get { return socketClient.RemoteEndPoint; }
        }
    }
}