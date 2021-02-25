using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace KeyMouseDo
{
    public class FetchColor
    {
        /*
        public struct POINTAPI
        {
            public int x;
            public int y;
        }

        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetDC(int hwnd);
        [DllImport("gdi32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern uint GetPixel(int hdc, int X, int y);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)] 
        private static extern int ReleaseDC(int hwnd, int hdc);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]             
        public static extern int GetCursorPos(ref POINTAPI lpPoint);


        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ScreenToClient(int hwnd, ref POINTAPI lpPoint);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        //[DllImport("user32.dll", EntryPoint = "WindowFromPoint")]           
        private static extern int WindowFromPoint(int xPoint, int yPoint);
        */
        
        static WinApis.POINTAPI mpt;
        static WinApis.POINTAPI mptclient;

        public FetchColor()
        {

        }

        public static Color gtColorMouse()
        {
            WinApis.GetCursorPos(ref mpt);                          

            return gtColor(mpt.X, mpt.Y);
        }
        public static Color gtColor(int x, int y)
        {
            //int h = WinMon.gtWinMouse();


            int hwnd = WinApis.WindowFromPoint(x, y);

            return getColor(hwnd, x, y);
            
        }
        public static Color getColorClient(int hwnd, int x,int y)
        {
            int hD = WinApis.GetDC(hwnd);
            uint pixel = WinApis.GetPixel(hD, x, y);
            WinApis.ReleaseDC(hwnd, hD);

            int r = (byte)pixel;
            int g = (byte)(pixel >> 8);
            int b = (byte)(pixel >> 16);
            Color c = Color.FromArgb(r, g, b);

            return c;
        }
        public static Color getColor(int hwnd, int x, int y)
        {
            mptclient.X = x;
            mptclient.Y = y;
            WinApis.ScreenToClient(hwnd, ref mptclient);

            return getColorClient(hwnd, mptclient.X, mptclient.Y);
        }
    }
}
