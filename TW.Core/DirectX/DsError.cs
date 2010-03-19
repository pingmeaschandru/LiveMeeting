using System.Runtime.InteropServices;
using System.Text;
using TW.Core.Native;

namespace TW.Core.DirectX
{
    public static class DsError
    {
        public static void ThrowExceptionForHR(int hr)
        {
            if (hr >= 0) return;
            var s = GetErrorText(hr);

            if (s != null)
                throw new COMException(s, hr);
                
            Marshal.ThrowExceptionForHR(hr);
        }

        public static string GetErrorText(int hr)
        {
            const int MAX_ERROR_TEXT_LEN = 160;
            var buf = new StringBuilder(MAX_ERROR_TEXT_LEN, MAX_ERROR_TEXT_LEN);

            return Quartz.AMGetErrorText(hr, buf, MAX_ERROR_TEXT_LEN) > 0 ? buf.ToString() : null;
        }
    }
}