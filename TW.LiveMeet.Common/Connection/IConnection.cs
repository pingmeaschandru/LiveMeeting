using System;
using System.Net;
namespace TW.LiveMeet.Server.Common.Connection
{
    public interface IConnection
    {
        string ConnectionId { get; }
        EndPoint RemoteEndPoint { get; }
        EndPoint LocalEndPoint { get; }
        void Send(byte[] data, int startIndex, int count);
        void Close();
    }
}