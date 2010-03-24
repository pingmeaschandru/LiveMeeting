using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TW.Core.Native
{
    public static class InputSimulator
    {
        public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
        {
            var result = User32.GetAsyncKeyState((UInt16)keyCode);
            return (result < 0);
        }

        public static bool IsKeyDown(VirtualKeyCode keyCode)
        {
            var result = User32.GetKeyState((UInt16)keyCode);
            return (result < 0);
        }

        public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            var result = User32.GetKeyState((UInt16)keyCode);
            return (result & 0x01) == 0x01;
        }

        public static void SimulateKeyDown(VirtualKeyCode keyCode)
        {
            var down = new Input();
            down.Type = (UInt32)InputType.KEYBOARD;
            down.Data.Keyboard = new KeybdInput
                                     {
                                         Vk = (UInt16) keyCode,
                                         Scan = 0,
                                         Flags = 0,
                                         Time = 0,
                                         ExtraInfo = IntPtr.Zero
                                     };

            var inputList = new Input[1];
            inputList[0] = down;

            var numberOfSuccessfulSimulatedInputs = User32.SendInput(1, inputList, Marshal.SizeOf(typeof(Input)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
        }

        public static void SimulateKeyUp(VirtualKeyCode keyCode)
        {
            var up = new Input();
            up.Type = (UInt32)InputType.KEYBOARD;
            up.Data.Keyboard = new KeybdInput
                                   {
                                       Vk = (UInt16) keyCode,
                                       Scan = 0,
                                       Flags = (UInt32) KeyboardFlag.KEYUP,
                                       Time = 0,
                                       ExtraInfo = IntPtr.Zero
                                   };

            var inputList = new Input[1];
            inputList[0] = up;

            var numberOfSuccessfulSimulatedInputs = User32.SendInput(1, inputList, Marshal.SizeOf(typeof(Input)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
        }

        public static void SimulateKeyPress(VirtualKeyCode keyCode)
        {
            var down = new Input();
            down.Type = (UInt32)InputType.KEYBOARD;
            down.Data.Keyboard = new KeybdInput
                                     {
                                         Vk = (UInt16) keyCode,
                                         Scan = 0,
                                         Flags = 0,
                                         Time = 0,
                                         ExtraInfo = IntPtr.Zero
                                     };

            var up = new Input();
            up.Type = (UInt32)InputType.KEYBOARD;
            up.Data.Keyboard = new KeybdInput
                                   {
                                       Vk = (UInt16) keyCode,
                                       Scan = 0,
                                       Flags = (UInt32) KeyboardFlag.KEYUP,
                                       Time = 0,
                                       ExtraInfo = IntPtr.Zero
                                   };

            var inputList = new Input[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = User32.SendInput(2, inputList, Marshal.SizeOf(typeof(Input)));
            if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
        }

        public static void SimulateTextEntry(string text)
        {
            if (text.Length > UInt32.MaxValue / 2) throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", UInt32.MaxValue / 2), "text");

            var chars = Encoding.ASCII.GetBytes(text);
            var len = chars.Length;
            var inputList = new Input[len * 2];
            for (var x = 0; x < len; x++)
            {
                UInt16 scanCode = chars[x];

                var down = new Input();
                down.Type = (UInt32)InputType.KEYBOARD;
                down.Data.Keyboard = new KeybdInput
                                         {
                                             Vk = 0,
                                             Scan = scanCode,
                                             Flags = (UInt32) KeyboardFlag.UNICODE,
                                             Time = 0,
                                             ExtraInfo = IntPtr.Zero
                                         };

                var up = new Input();
                up.Type = (UInt32)InputType.KEYBOARD;
                up.Data.Keyboard = new KeybdInput
                                       {
                                           Vk = 0,
                                           Scan = scanCode,
                                           Flags = (UInt32) (KeyboardFlag.KEYUP | KeyboardFlag.UNICODE),
                                           Time = 0,
                                           ExtraInfo = IntPtr.Zero
                                       };

                // Handle extended keys:
                // If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
                // we need to include the KEYEVENTF_EXTENDEDKEY flag in the MouseEventFlags property. 
                if ((scanCode & 0xFF00) == 0xE000)
                {
                    down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
                    up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
                }

                inputList[2*x] = down;
                inputList[2*x + 1] = up;

            }

            User32.SendInput((UInt32)len * 2, inputList, Marshal.SizeOf(typeof(Input)));
        }

        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            SimulateKeyDown(modifierKeyCode);
            SimulateKeyPress(keyCode);
            SimulateKeyUp(modifierKeyCode);
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(SimulateKeyDown);
            SimulateKeyPress(keyCode);
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(SimulateKeyUp);
        }

        public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            SimulateKeyDown(modifierKey);
            if (keyCodes != null) keyCodes.ToList().ForEach(SimulateKeyPress);
            SimulateKeyUp(modifierKey);
        }

        public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(SimulateKeyDown);
            if (keyCodes != null) keyCodes.ToList().ForEach(SimulateKeyPress);
            if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(SimulateKeyUp);
        }
    }
}