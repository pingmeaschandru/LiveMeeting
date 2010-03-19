using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [StructLayout(LayoutKind.Sequential)]
    public class DsLong
    {
        private readonly long value;

        public DsLong(long value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static implicit operator long(DsLong l)
        {
            return l.value;
        }

        public static implicit operator DsLong(long l)
        {
            return new DsLong(l);
        }

        public long ToInt64()
        {
            return value;
        }

        public static DsLong FromInt64(long l)
        {
            return new DsLong(l);
        }
    }
}