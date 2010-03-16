using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    public static class Ntdll
    {
        [DllImport("ntdll.dll")]
        public static unsafe extern byte* memcpy(byte* dst,byte* src,int count);
        [DllImport("ntdll.dll")]
        public static unsafe extern byte* memset(byte* dst,int filler,int count);
    }
}
