using System;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    internal class EMTMarshaler : DsMarshaler
    {
        public EMTMarshaler(string cookie)
            : base(cookie)
        {
        }

        public override object MarshalNativeToManaged(IntPtr pNativeData)
        {
            var emt = obj as AMMediaType[];
            if (emt == null) return null;

            for (var x = 0; x < emt.Length; x++)
            {
                var p = Marshal.ReadIntPtr(pNativeData, x*IntPtr.Size);
                emt[x] = p != IntPtr.Zero ? (AMMediaType) Marshal.PtrToStructure(p, typeof (AMMediaType)) : null;
            }

            return null;
        }

        public override int GetNativeDataSize()
        {
            var i = ((Array)obj).Length;
            var j = i * IntPtr.Size;

            return j;
        }

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new EMTMarshaler(cookie);
        }
    }
}