using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace RmbHook
{
    public delegate bool CallBack(int hwnd, int lParam);
    
    public class WinMon
    {
        public const int SE_SHUTDOWN_PRIVILEGE = 0x13;
        public const int HWND_TOPMOST = -1;
        public const int HWND_TOP = 0;
        public const int HWND_NOTOPMOST = -2;
        public const int SWP_NOMOVE = 0x0002;
        public const int SWP_NOSIZE = 0x0001;

        [StructLayout(LayoutKind.Sequential)]           //定义与API相兼容结构体
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [DllImport("User32.dll")]
        public static extern bool EnumWindows(CallBack x, int y);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(int hwnd, int nCmdShow);
        [DllImport("User32.dll")]
        public static extern long GetWindowLong(int hwnd, int nIndex);
        [DllImport("User32.dll")]
        public static extern int GetWindowText(int hwnd, StringBuilder lpstr, int nMaxCount);
        [DllImport("User32.dll")]
        public static extern int GetForegroundWindow( );
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(int hwnd);
        [DllImport("User32.dll")]
        public static extern bool  IsIconic(int hWnd);
        [DllImport("User32.dll")]
        public static extern bool IsZoomed(int hWnd);


        [DllImport("user32.dll")]
        public static extern int FindWindow(StringBuilder lpClassName, StringBuilder lpWindowName);
        [DllImport("user32.dll")]
        public static extern int SetParent(int hWndChild, int hWndNewParent);
        [DllImport("user32.dll")]
        public static extern int GetParent(int hWndChild);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int X, int Y, int cx,
            int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]              //获取鼠标坐标
        public static extern int GetCursorPos(  ref POINTAPI lpPoint  );
        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]           //指定坐标处窗体句柄
        public static extern int WindowFromPoint(int xPoint, int yPoint );

        [DllImport("user32.dll")]
        public static extern int SetFocus(int hwnd);
        [DllImport("user32.dll")]
        public static extern int SetActiveWindow(int hwnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(int hwnd, out int ID);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);


        CallBack mcallback = null;

        public static WinMon mthis = null;

        Parameter mprm = null;
        int mkeeptime = 30;
        int mkeepnum = 2;
        int mstorenum = 20;
        int mrestorenum = 2;

        ListBox mlist = null;

        int mhwnd = 0;

        //static ArrayList mwins = new ArrayList();
         Hashtable mwins = new Hashtable();
        // 存储：句柄，加入时间；
        // 条件：在desk上的窗口；
        // 作用：定时检查其加入时间与当前时间差，若大于阈值，则将其min;
        // 要求：快速查找key；

         SortedList msortlist = new SortedList(new SortedList());
        // 存储：本次待min之窗口，已经按在desk上显示时间降序排列；
        // 要求：排序，降序，允许重复key；

         StringBuilder msb = new StringBuilder(512);
         LinkedList<int> mlink = new LinkedList<int>();
        // 存储：被lsmon最小化的窗口，用于restore；
        // 要求：后进先出， 先进先出；

         Stack<int> mminwins = new Stack<int>();
         //LinkedList<int> mminwins = new LinkedList<int>();
        // 存储：最小化的窗口，但不是因为lsmon；
        // 要求：后进先出, 包含查找；

        //static ArrayList<int> mremoves = new ArrayList<int>();
         //Queue<int> mminwins = new Queue<int>();          


        public bool mrun = false;

        static DateTime mdt = DateTime.Now;

        public WinMon()
        {
            mthis = this;
            mcallback = new CallBack(WinMon.s_report); 
        }

        public int init()
        {
            mprm = ObjMan.gthis.mparameter;

            mkeeptime = mprm.getKeepTime();
            mkeepnum = mprm.getKeepNum();
            mstorenum = mprm.getStorenum();
            mrestorenum = mprm.getRestorenum();

            return 1;
        }
        public void setRun(bool b) 
        {
            if (b == false)
            {
                restore(true);
            }
            else
            {
                mwins.Clear();
                mlink.Clear();
                mminwins.Clear();                   // 140710;
            }
            mrun = b; 
        }
        public static bool s_report(int hwnd, int lParam)
        {
            bool b = false;

            if (WinMon.mthis != null)
                b = WinMon.mthis.report(hwnd, lParam);

            return b;
        }

        // min top window now, and don't put in collections;
        // just like min menu( click min button);
        public void minNow()
        {
            int hwnd = GetForegroundWindow();
            GetWindowText(hwnd, msb, msb.Capacity);

            if (msb.Length > 0)                         // only deal these have titles;
            {
                if (!IsIconic(hwnd))
                {
                    // SW_SHOW=5
                    ShowWindow(hwnd, 6);               // SW_MINIMIZE
                }
            }
        }
        public void stBottom()
        {
            int hwnd = GetForegroundWindow();
            GetWindowText(hwnd, msb, msb.Capacity);

            if (msb.Length > 0)                         // only deal these have titles;
            {
                //if (!IsIconic(hwnd))
                {
                    //ShowWindow(hwnd, 6);               // SW_MINIMIZE
                    // HWND_BOTTOM
                    SetWindowPos( hwnd, 1, 0, 0, 0, 0, SE_SHUTDOWN_PRIVILEGE);
                }
                //

                int pid;
                int fhwnd = hwnd;
                Thread.Sleep(100);

                int fid = GetWindowThreadProcessId(hwnd, out pid);
                setWinUnderMouseForeground(fid);

                //SetFocus(hwnd);

                /*
                while (true)
                {
                    if (hwnd == 0)
                        break;

                    GetWindowText(hwnd, msb, msb.Capacity);
                    Console.Out.WriteLine(" show win> " + hwnd.ToString() + "=" + msb);
                    if (mwins.Contains(hwnd))
                    {
                        ShowWindow(hwnd, 1);    //SW_SHOWNORMAL=1                // SW_SHOW=5;
                        break;
                    }
                    else
                    {
                        hwnd = GetParent(hwnd);
                        continue;
                    }
                }
               */

               
            }
        }
        public int setWinUnderMouseForeground(int attachId)
        {
            int hwnd = getWinUnderMouse();
            int d= GetWindowText(hwnd, msb, msb.Capacity);
            //Console.Out.WriteLine(" getwindowtext: " + d.ToString());
            Console.Out.WriteLine(" show win (" + hwnd.ToString() + "): " + msb);
            //SetActiveWindow(hwnd);

            int pid=0;
            int cid = GetWindowThreadProcessId(hwnd, out pid);
            AttachThreadInput(attachId, cid, true);

            ShowWindow(hwnd, 1);    //SW_SHOWNORMAL=1                // SW_SHOW=5;
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            SetForegroundWindow(hwnd);

            return hwnd;
        }
        void stTop(int hwnd)
        {
            //setwindowpos(me.hwnd,XXX_TOPMOST,0,0,0,0,XX_NOMOVE|XXX_NOSIZE)
            //HWND_TOP=0
            // SWP_SHOWWINDOW=0x0040;
            //SWP_NOSIZE=0x0001;
            //SWP_NOMOVE=0x0002;
            SetWindowPos(hwnd, 0, 0, 0, 0, 0, 0x0001 | 0x0002);
        }
        public void enumWin(ListBox list)
        {
            mlist = list;
            if (mlist != null)
            {
                if (mlist.Items.Count > 0)
                {
                    mlist.Items.Clear();
                }
            }

            //
            //mrvque.Clear();
            msortlist.Clear();                                  // 140709;
            mdt = DateTime.Now;

            mhwnd = GetForegroundWindow();
            removeTop(mhwnd);
            EnumWindows(mcallback, 0);
            minWins();

        }
        
        public bool report(int hwnd, int lParam)
        {
            addListBox(hwnd);

            int d = 0;
            //int dif = 0;
            if (mhwnd != hwnd)
            {
                long style = GetWindowLong(hwnd, -16);
                if (((style & 0x00020000L) == 0x00020000L) && ((style & 0x10000000L) == 0x10000000L))
                {
                    GetWindowText(hwnd, msb, msb.Capacity);

                    if (msb.Length > 0)                         // only deal these have titles;
                    {
                        if (!IsIconic(hwnd))
                        {
                            if (mwins.Contains(hwnd))           // check if in table;
                            {
                                DateTime dt = (DateTime)mwins[hwnd];
                                TimeSpan ts = mdt.Subtract(dt);
                                d = (int) ts.TotalSeconds;      // 140709;

                                if ( d >= mkeeptime)
                                {
                                    //mrvque.Enqueue(hwnd);
                                    //dif = d - mprm.mkeeptime;
                                    msortlist.Add( d,hwnd);
                                }
                            }
                            else// if (msb.Length > 0) // has title;
                            {
                                mwins.Add(hwnd, mdt);
                                Console.WriteLine("wins add> " + hwnd.ToString() + "=" + msb);
                            }
                        }
                        else // iconic wins;
                        {
                            
                            if (mwins.Contains(hwnd))
                            {
                                mwins.Remove(hwnd);
                                Console.WriteLine("wins remove> " + hwnd.ToString() + "=" + msb);
                            }

                            if (!mlink.Contains(hwnd) && !mminwins.Contains(hwnd))       // 140710;
                            {
                                mminwins.Push(hwnd);
                                Console.WriteLine(" minwins.push> " + hwnd.ToString() + "=" + msb);
                            }
                        }
                    }
                }
            }
            return true;
        }
        void minWins()
        {
            int hwnd = 0;
            int kpnum = mkeepnum;

            //SortedList monthList = new SortedList();
            //monthList.GetByIndex()

            int k;//, v;
            //msortlist.
            int d = msortlist.Count;
            //for (int i = 0; i < d;  i++)
            //{
            //    k = (int)msortlist.GetKey(i);
            //    v = (int)msortlist.GetByIndex(i);
            //}
            //d = mrvque.Count;
            if ( d > kpnum)
            {
                d = d - kpnum;
                for (int i = 0; i < d; i++)
                {
                    //hwnd = mrvque.Dequeue();
                    k = (int)msortlist.GetKey(i);
                    hwnd = (int)msortlist.GetByIndex(i);

                    ShowWindow(hwnd, 6);                    // SW_MINIMIZE
                    mwins.Remove(hwnd);

                    GetWindowText(hwnd, msb, msb.Capacity);
                    Console.WriteLine("wins remove> " + k.ToString() + "," +hwnd.ToString()+ "=" + msb);

                    mlink.AddLast(hwnd);

                    Console.WriteLine("  restore enqueue> " + hwnd.ToString() + "=" + msb);
                    if (mlink.Count > mstorenum)
                    {
                        mlink.RemoveFirst();
                        Console.WriteLine("  restore dequeue> " + hwnd.ToString() + "=" + msb);
                    }
                }
            }
        }

        public  void restore() {     restore(false);      }
        public  void restore(bool all)
        {
            if (!mrun)                              // 140717;
                return;

            int hwnd;

            //gtWinMouse();       // test; 140711;

            int cnt = mrestorenum;             // 140708;
            int lcnt = mlink.Count;
            if (all)
            {
                cnt = lcnt;
            }
            else
            {
                if (cnt > lcnt)
                    cnt = lcnt;
            }

            for (int i = 0; i < cnt; i++)
            {
                hwnd = mlink.Last.Value;
                mlink.RemoveLast();
                

                GetWindowText(hwnd, msb, msb.Capacity);
                if (msb.Length > 0)                     // empty if hwnd not exist; 140709;
                {
                    Console.WriteLine("  restore > " + hwnd.ToString() + "=" + msb);
                    ShowWindow(hwnd, 9);                // SW_RESTORE
                }
            }

            if (cnt <= 0)                               // 140710; restore in mminwins;
            {
                while (mminwins.Count > 0)
                {
                    hwnd = mminwins.Pop();

                    GetWindowText(hwnd, msb, msb.Capacity);
                    Console.WriteLine("  minwins.pop> " + hwnd.ToString() + "=" + msb);

                    if (mwins.Contains(hwnd))           // if win's already restored; then it must bin in mwins;
                        continue;

                    if (msb.Length > 0)                     // empty if hwnd not exist; 140709;
                    {
                        Console.WriteLine("  minwins restore> " + hwnd.ToString() + "=" + msb);
                        ShowWindow(hwnd, 9);                // SW_RESTORE
                        break;
                    }
                    else
                        continue;
                }
            }
        }

        void addListBox(int hwnd)
        {
            if (mlist != null)
            {
                StringBuilder s = new StringBuilder(512);
                int d = GetWindowText(hwnd, s, s.Capacity);

                mlist.Items.Add(s);
            }
        }    
        void removeTop(int hwnd)
        {
            if (mwins.Contains(hwnd))
            {
                GetWindowText(hwnd, msb, msb.Capacity);
                mwins.Remove(hwnd);
                Console.WriteLine("wins remove top hwnd> " + hwnd.ToString() + "=" + msb);
            }
        }

        public static int getWinUnderMouse()
         {
             POINTAPI point = new POINTAPI();                   //必须用与之相兼容的结构体，类也可以

             GetCursorPos(ref point);                           //获取当前鼠标坐标
             int hwnd = WindowFromPoint(point.X, point.Y);      //获取指定坐标处窗口的句柄
             Console.Out.WriteLine(" hwnd=" + hwnd.ToString());

             return hwnd;

             //this.label1.Text = point.X.ToString() + ":" + point.Y.ToString() + "-" + hwnd.ToString();//显示效果，此时窗口已经嵌入桌面了
         }

    }
}
