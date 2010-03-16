using TW.Core.Collections;

namespace TW.LiveMeet.Server.Common
{
    public class PublisherRegistrator : SyncDictionary<string, ISessionContext>,
                                        IRegistrator<string, ISessionContext>
    {
    }
}