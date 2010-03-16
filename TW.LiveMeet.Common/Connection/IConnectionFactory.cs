namespace TW.LiveMeet.Server.Common.Connection
{
    public abstract class IConnectionFactory<T>
    {
        public abstract IConnection Create(T obj);
    }
}