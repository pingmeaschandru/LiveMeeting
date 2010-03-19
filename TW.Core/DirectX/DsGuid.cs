using System;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    [StructLayout(LayoutKind.Explicit)]
    public class DsGuid
    {
        [FieldOffset(0)]
        private Guid guid;
        public static readonly DsGuid Empty = Guid.Empty;

        public DsGuid()
        {
            guid = Guid.Empty;
        }

        public DsGuid(string g)
        {
            guid = new Guid(g);
        }

        public DsGuid(Guid g)
        {
            guid = g;
        }

        public override string ToString()
        {
            return guid.ToString();
        }

        public string ToString(string format)
        {
            return guid.ToString(format);
        }

        public override int GetHashCode()
        {
            return guid.GetHashCode();
        }

        public static implicit operator Guid(DsGuid g)
        {
            return g.guid;
        }

        public static implicit operator DsGuid(Guid g)
        {
            return new DsGuid(g);
        }

        public Guid ToGuid()
        {
            return guid;
        }

        public static DsGuid FromGuid(Guid g)
        {
            return new DsGuid(g);
        }
    }
}