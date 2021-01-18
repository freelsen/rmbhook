using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace RmbHook
{
    public class FetchColor
    {
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
        
        static POINTAPI mpt;
        static POINTAPI mptclient;

        public FetchColor()
        {

        }

        public static Color gtColorMouse()
        {
            GetCursorPos(ref mpt);                          

            return gtColor(mpt.x, mpt.y);
        }
        public static Color gtColor(int x, int y)
        {
            //int h = WinMon.gtWinMouse();
            //Console.Out.WriteLine(" hwnd=" + h.ToString() );
            

            int hwnd = WindowFromPoint(x, y);
            //Console.Out.WriteLine(" mouse=" + x.ToString() + "," + y.ToString());
            //Console.Out.WriteLine(" hwnd=" + hwnd.ToString());

            int hD = GetDC(hwnd);

            mptclient.x = x;
            mptclient.y = y;
            ScreenToClient(hwnd, ref mptclient);
            //Console.Out.WriteLine(" client=" +mptclient.x.ToString() + ","+mptclient.y.ToString());

            uint pixel = GetPixel(hD, mptclient.x, mptclient.y);
            //Console.Out.WriteLine(" pixel=" + pixel.ToString());

            ReleaseDC(hwnd,hD);

            int r = (byte) pixel;
            int g = (byte)(pixel >> 8);
            int b = (byte)(pixel >> 16);
            //Color clr = Color.FromArgb(c);
            Color c = Color.FromArgb(r, g, b);

            return c;
        }
    }
}
