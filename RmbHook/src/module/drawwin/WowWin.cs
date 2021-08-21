using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ls.libs;

namespace WrittingHelper
{
    class WowWin
    {
        public static WowWin mthis = null;

        public string mwinname = "World of Warcraft";
        public int mhwnd = 0;
        public IntPtr _hWnd {get; set;} =(IntPtr)null;
        public IntPtr GetHwnd() { return this._hWnd; }

        //public string mwinname = "Signal";
        bool mistargetfound = false;
        public Point mposition = new Point(0, 0);
        public WinApis.RECT mrect = new WinApis.RECT() { left = 0, right = 0, top = 0, bottom = 0 };
        public Point mclientcenter = new Point(0, 0);
        public Rectangle mclient = new Rectangle(0, 0, 0, 0);

        public WowWin()
        {
            mthis = this;
        }

        public bool FindWin()
        {
            IntPtr hwndtarget = (IntPtr)null;
            hwndtarget = WinApis.FindWindow(null, mwinname);

            if (hwndtarget != (IntPtr)null)
            {
                Lslog.log("target win handle=" + hwndtarget.ToString("x"));
                _hWnd = hwndtarget;
                mhwnd = (int)hwndtarget;
                mistargetfound = true;
            }
            else
                return false;

            return UpdateInfo();
        }
        bool UpdateInfo()
        {
            //_targetpos = new Point();
            mposition.X = 0; mposition.Y = 0;
            bool found = WinApis.ClientToScreen(_hWnd, ref mposition);
            if (found)
            {
                //WinApis.RECT _rect;
                WinApis.GetClientRect(_hWnd, out mrect);

                mclientcenter.X = (mrect.right - mrect.left) / 2;
                mclientcenter.Y = (mrect.bottom - mrect.top) / 2;

                mclient.X = mposition.X;
                mclient.Y = mposition.Y;
                mclient.Width = mrect.right - mrect.left;
                mclient.Height = mrect.bottom - mrect.top;

                Lslog.log(mposition.X.ToString() + "," + mposition.Y.ToString());

            }

            return found;
        }


        public void screenToClient(ref Point pt)
        {
            WinApis.ScreenToClient(_hWnd, ref pt);
            //return pt;
        }
        
    }
}
