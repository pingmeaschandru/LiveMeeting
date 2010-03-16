using TW.Core.Sockets;
using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Common.Connection;
using TW.LiveMeet.Server.Common.Connection.Tcp;

namespace TW.LiveMeet.Server.Rdap.Connection
{
    public class RdapConnectionListener
    {
        private readonly TcpConnectionListener connectionListener;
        private readonly IRegistrator<string, IConnection> sessionRegister;
        private readonly IRegistrator<string, ISessionContext> publisherRegistrator;
        private readonly IConnectionFactory<TcpSocketClient> connectionFactory;

        public RdapConnectionListener()
        {
            sessionRegister = new SessionRegistrator();
            publisherRegistrator = new PublisherRegistrator();
            connectionFactory = new RdapConnectionFactory(sessionRegister, publisherRegistrator);
            connectionListener = new TcpConnectionListener(connectionFactory, sessionRegister);
        }

        public void Start(int port)
        {
            connectionListener.Start(port);
        }
    }
}