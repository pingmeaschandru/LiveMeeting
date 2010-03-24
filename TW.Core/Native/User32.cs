using System;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    public class User32
    {
        public const Int32 CURSOR_SHOWING = 0x00000001;

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr handlePtr, ref Rectangle rectangle);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CursorInfo pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out IconInfo piconinfo);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern UInt32 SendInput(UInt32 numberOfInputs, Input[] inputs, Int32 sizeOfInputStructure);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int16 GetAsyncKeyState(UInt16 virtualKeyCode);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int16 GetKeyState(UInt16 virtualKeyCode);
        
        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();
    }
}