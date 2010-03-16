using System.Runtime.InteropServices;
using System.Security;

namespace TW.Core.Native
{
    public class Kernel32
    {
        [DllImport("KERNEL32"), SuppressUnmanagedCodeSecurity]
        public static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

        [DllImport("KERNEL32"), SuppressUnmanagedCodeSecurity]
        public static extern bool QueryPerformanceFrequency(ref long lpFrequency); 
    }
}
