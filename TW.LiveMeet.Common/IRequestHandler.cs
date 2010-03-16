namespace TW.LiveMeet.Server.Common
{
    public interface IRequestHandler
    {
        IRequest Process(byte[] buffer, int startIndex, int count);
    }
}