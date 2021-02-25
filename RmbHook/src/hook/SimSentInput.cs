using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KeyMouseDo
{
    class SimSentInput
    {
        // Input Region Begin
        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }

        ///// <summary>
        ///// Synthesizes keystrokes, mouse motions, and button clicks.
        ///// </summary>
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        // INPUT
        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi; // Mouse, Keyboard, Hardware, Input
        }

        enum SendInputEventType : int
        {
            MOUSE = 0,
            KEYBOARD = 1,
            HARDWARE = 2
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi; // mouse input

            [FieldOffset(0)]
            public KEYBDINPUT ki; // keyboard input

            [FieldOffset(0)]
            public HARDWAREINPUT hi; // hardware input
        }

        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        //________________________________________________________________________________________
        // Input Region End


        // #############################################################################
        // MOUSE INPUT 
        // #############################################################################
        public static void ClickLeftMouseButton() // left click
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.MOUSE;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));

            /*
             * NOTE***
             * Since I have now gotten to this point, I have not done a mouse move yet, but I  believe you
             * might need to your movement to dx,dy like so:
             */
            //mouseDownInput.mkhi.mi.dx -= 50;
            //mouseDownInput.mkhi.mi.dy -= 80;
            ///*
            // * ...or apply it's coordinates to the mouse, like this
            // */
            ////mouseDownInput.mkhi.mi.dx = Cursor.Position.X;
            ////mouseDownInput.mkhi.mi.dy = Cursor.Position.Y;

            //Console.WriteLine(mouseDownInput.mkhi.mi.dx + "," + mouseDownInput.mkhi.mi.dy +
            //    " | " + mouseDownInput.mkhi.mi.mouseData + " | " + mouseDownInput.mkhi.mi.time);

            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.MOUSE;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

        public static void ClickRightMouseButton() // right click
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.MOUSE;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));

            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.MOUSE;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }
    }
}
