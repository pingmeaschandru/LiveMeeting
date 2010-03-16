using TW.Coder;

namespace TW.LiveMeet.DesktopAgent
{
    public interface IDesktopCaptureStorage
    {
        void Process(RgbFrame frame);
    }
}