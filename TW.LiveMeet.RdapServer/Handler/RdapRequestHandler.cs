using System.IO;
using TW.Core.IO;
using TW.LiveMeet.RDAP.Parser;
using TW.LiveMeet.Server.Common;

namespace TW.LiveMeet.Server.Rdap.Handler
{
    public class RdapRequestHandler : IRequestHandler
    {
        private readonly IResponseHandler responseHandler;
        private readonly RdapMessageParser messageParser;
        private readonly Stream readParserStream;

        public RdapRequestHandler(IResponseHandler responseHandler)
        {
            this.responseHandler = responseHandler;
            readParserStream = new FifoStream();
            messageParser = new RdapMessageParser(readParserStream);
        }

        public IRequest Process(byte[] buffer, int startIndex, int count)
        {
            return null;
        }
    }
}