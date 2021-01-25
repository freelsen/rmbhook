using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace KeyMouseDo
{
    public class WinAPIs
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern int GetCursorPos(ref POINTAPI lpPoint);

        public static bool GetCursorPos(ref int x, ref int y)
        {
            POINTAPI mpt32 = new WinAPIs.POINTAPI();
            int d= GetCursorPos(ref mpt32);

            x = mpt32.X;
            y = mpt32.Y;

            return (d != 0) ? true : false;
        }
        public static bool GetCursorPos(ref Point pt)
        {
            POINTAPI mpt32 = new WinAPIs.POINTAPI();
            int d = GetCursorPos(ref mpt32);

            pt.X = mpt32.X;
            pt.Y = mpt32.Y;

            return (d != 0) ? true : false;
        }
    }
}
