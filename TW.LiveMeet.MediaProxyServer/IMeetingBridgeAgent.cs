using System;
using System.Net;

namespace TW.LiveMeet.Server.Media
{
    public interface IMeetingBridgeAgent
    {
        event Action<byte[]> OnDataRecieveEvent;
        event Action<string> OnCloseEvent;

        void Send(byte[] dataToSend, int startIndex, int count);
        void Close();
        string ConnectionId { get; }
        bool DoSend { get; set; }
        EndPoint RemoteEndpoint { get; }
    }
}