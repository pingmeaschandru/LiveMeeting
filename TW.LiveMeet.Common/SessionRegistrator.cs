using TW.Core.Collections;
using TW.LiveMeet.Server.Common.Connection;

namespace TW.LiveMeet.Server.Common
{
    public class SessionRegistrator : SyncDictionary<string, IConnection>, 
        IRegistrator<string, IConnection>
    {
    }
}