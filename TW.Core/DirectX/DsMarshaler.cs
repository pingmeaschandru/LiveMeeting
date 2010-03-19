using System;
using System.Runtime.InteropServices;

namespace TW.Core.DirectX
{
    internal abstract class DsMarshaler : ICustomMarshaler
    {
        protected string cookie;
        protected object obj;

        protected DsMarshaler(string cookie)
        {
            this.cookie = cookie;
        }

        public virtual IntPtr MarshalManagedToNative(object managedObj)
        {
            obj = managedObj;
            var iSize = GetNativeDataSize() + 3;
            var p = Marshal.AllocCoTaskMem(iSize);

            for (var x = 0; x < iSize/4; x++)
                Marshal.WriteInt32(p, x*4, 0);

            return p;
        }

        public virtual object MarshalNativeToManaged(IntPtr pNativeData)
        {
            return obj;
        }

        public virtual void CleanUpNativeData(IntPtr pNativeData)
        {
            if (pNativeData != IntPtr.Zero)
                Marshal.FreeCoTaskMem(pNativeData);
        }

        public virtual void CleanUpManagedData(object managedObj)
        {
            obj = null;
        }

        public abstract int GetNativeDataSize();
    }
}