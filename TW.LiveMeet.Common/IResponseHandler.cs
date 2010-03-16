namespace TW.LiveMeet.Server.Common
{
    public interface IResponseHandler
    {
        void Process(IResponse response);
    }
}