using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace WrittingHelper.libs
{

    

    public class WinApis
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }


        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern int GetCursorPos(ref POINTAPI lpPoint);
        public static bool GetCursorPos(ref int x, ref int y)
        {
            POINTAPI mpt32 = new WinApis.POINTAPI();
            int d= GetCursorPos(ref mpt32);

            x = mpt32.X;
            y = mpt32.Y;

            return (d != 0) ? true : false;
        }
        public static bool GetCursorPos(ref Point pt)
        {
            POINTAPI mpt32 = new WinApis.POINTAPI();
            int d = GetCursorPos(ref mpt32);

            pt.X = mpt32.X;
            pt.Y = mpt32.Y;

            return (d != 0) ? true : false;
        }

        // 2021-02-13, window,
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        //[DllImport("user32.dll", EntryPoint = "WindowFromPoint")]           
        public static extern int WindowFromPoint(int xPoint, int yPoint);

        // 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);
        
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        // MoveWindow moves a window or changes its size based on a window handle.
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);



        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")]
        public static extern int ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ScreenToClient(int hwnd, ref POINTAPI lpPoint);




        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetDC(int hwnd);
        [DllImport("gdi32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern uint GetPixel(int hdc, int X, int y);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int ReleaseDC(int hwnd, int hdc);







    }

}
