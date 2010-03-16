using TW.Core.Sockets;
using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Common.Connection;
using TW.LiveMeet.Server.Common.Connection.Tcp;
using TW.LiveMeet.Server.Rdap.Handler;
using TW.LiveMeet.Server.Streaming;

namespace TW.LiveMeet.Server.Rdap.Connection
{
    public class RdapConnectionFactory : IConnectionFactory<TcpSocketClient>
    {
        private readonly IRegistrator<string, IConnection> sessionRegister;
        private readonly IRegistrator<string, ISessionContext> publisherRegistrator;

        public RdapConnectionFactory(IRegistrator<string, IConnection> sessionRegister,
            IRegistrator<string, ISessionContext> publisherRegistrator)
        {
            this.sessionRegister = sessionRegister;
            this.publisherRegistrator = publisherRegistrator;
        }

        public override IConnection Create(TcpSocketClient clientSocket)
        {
            var responseHandler = new RdapResponseHandler(clientSocket.ConnectionId, sessionRegister);
            var requestHandler = new RdapRequestHandler(responseHandler);
            var sessionContext = new SessionContext(clientSocket.ConnectionId, requestHandler, publisherRegistrator);

            return new TcpConnection(clientSocket, sessionContext, sessionRegister);
        }
    }
}
