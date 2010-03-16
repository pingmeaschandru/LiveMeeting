namespace TW.LiveMeet.Server.Common
{
    public interface IRegistrator<TKey, TValue>
    {
        void Add(TKey key, TValue value);
        bool Remove(TKey key);
        bool TryGetValue(TKey key, out TValue value);
    }
}