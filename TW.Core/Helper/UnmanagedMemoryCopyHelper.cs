using System;
using TW.Core.Native;

namespace TW.Core.Helper
{
    public class UnmanagedMemoryCopyHelper
    {
        public static IntPtr CopyUnmanagedMemory(IntPtr dst, IntPtr src, int count)
        {
            unsafe
            {
                CopyUnmanagedMemory((byte*)dst.ToPointer(), (byte*)src.ToPointer(), count);
            }
            return dst;
        }

        public static unsafe byte* CopyUnmanagedMemory(byte* dst, byte* src, int count)
        {
            return Ntdll.memcpy(dst, src, count);
        }


        public static IntPtr SetUnmanagedMemory(IntPtr dst, int filler, int count)
        {
            unsafe
            {
                SetUnmanagedMemory((byte*)dst.ToPointer(), filler, count);
            }
            return dst;
        }


        public static unsafe byte* SetUnmanagedMemory(byte* dst, int filler, int count)
        {
            return Ntdll.memset(dst, filler, count);
        }
    }
}
