using System.Runtime.InteropServices;
using System.Text;

namespace TW.Core.Native
{
    public class Quartz
    {
        [DllImport("quartz.dll", CharSet = CharSet.Auto)]
        public static extern int AMGetErrorText(int hr, StringBuilder buf, int max);
    }
}
