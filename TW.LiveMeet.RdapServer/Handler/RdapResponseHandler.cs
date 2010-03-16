using TW.LiveMeet.Server.Common;
using TW.LiveMeet.Server.Common.Connection;

namespace TW.LiveMeet.Server.Rdap.Handler
{
    public class RdapResponseHandler : IResponseHandler
    {
        private readonly string connectionId;
        private readonly IRegistrator<string, IConnection> registrator;

        public RdapResponseHandler(string connectionId, IRegistrator<string, IConnection> registrator)
        {
            this.connectionId = connectionId;
            this.registrator = registrator;
        }

        public void Process(IResponse response)
        {
            //connection.Write();
        }
    }
}