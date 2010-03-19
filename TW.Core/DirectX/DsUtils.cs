using System;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    public static class DsUtils
    {
        public static Guid GetPinCategory(IPin pPin)
        {
            var guidRet = Guid.Empty;
            var iSize = Marshal.SizeOf(typeof(Guid));
            var ipOut = Marshal.AllocCoTaskMem(iSize);

            try
            {
                var g = PropSetID.Pin;
                var pKs = pPin as IKsPropertySet;
                if (pKs != null)
                {
                    int cbBytes;
                    var hr = pKs.Get(g, (int)AMPropertyPin.Category, IntPtr.Zero, 0, ipOut, iSize, out cbBytes);
                    DsError.ThrowExceptionForHR(hr);
                    guidRet = (Guid)Marshal.PtrToStructure(ipOut, typeof(Guid));
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(ipOut);
            }

            return guidRet;
        }

        public static void FreeAMMediaType(AMMediaType mediaType)
        {
            if (mediaType == null) return;
            if (mediaType.formatSize != 0)
            {
                Marshal.FreeCoTaskMem(mediaType.formatPtr);
                mediaType.formatSize = 0;
                mediaType.formatPtr = IntPtr.Zero;
            }
            if (mediaType.unkPtr == IntPtr.Zero) return;
            Marshal.Release(mediaType.unkPtr);
            mediaType.unkPtr = IntPtr.Zero;
        }

        public static void FreePinInfo(PinInfo pinInfo)
        {
            if (pinInfo.filter == null) return;
            Marshal.ReleaseComObject(pinInfo.filter);
            pinInfo.filter = null;
        }

    }
}