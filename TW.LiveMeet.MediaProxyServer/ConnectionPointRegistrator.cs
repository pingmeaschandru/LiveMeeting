using TW.Core.Collections;
using TW.LiveMeet.Server.Common;

namespace TW.LiveMeet.Server.Media
{
    public class ConnectionPointRegistrator : SyncDictionary<string, IConnectionPoint>,
                                                        IRegistrator<string, IConnectionPoint>
    {
    }
}