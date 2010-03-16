namespace TW.Core.Sockets
{
    public class SocketMessageEventArgs : SocketEventArgs
    {
        public byte[] Buffer { get; set; }

        public SocketMessageEventArgs(SocketBase socketBase, byte[] buffer)
            : base(socketBase)
        {
            Buffer = buffer;
        }
    }
}