using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace RmbHook
{
    public class Gesture
    {
        public static Gesture mthis = null;

        [StructLayout(LayoutKind.Sequential)]           //定义与API相兼容结构体
        public struct POINTAPI
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]              //获取鼠标坐标
        public static extern int GetCursorPos(ref POINTAPI lpPoint);

        POINTAPI mpt32 = new POINTAPI();

         Point mo = new Point(0, 0);
         Point mv = new Point();

        Point mpto = new Point(0,0);                 // origin point;
        Point mptnow = new Point(0,0);
        Point mptlast = new Point(0,0);

        //int mdismin = 20;
        //Point mlastmv = new Point();
        Point mptend = new Point();
        Point mptstart = new Point();
        Point mptup = new Point();
        Point mptdown = new Point();

        DateTime mdt = DateTime.Now;
        DateTime mdtnow = DateTime.Now;
        DateTime mdtlast = DateTime.Now;
        int mtres = 10;//ms;

        int mtinterval = 10; // ms;
        int mgapcnt = 60; // ms;
        int movcnt = 200; //ms;
        int mdvmin = 5;//2; // 40pixel/10ms;
        public void setTinterval(int t){ mtinterval=t;}

        double mdis;
        int mdx;
        int mdy;
        int ma;
        int mdismin = 40;                           // 140716;
        public int GetMa() { return ma; }

        public Rectangle mcenter;                   // = new Rectangle();
        int mcsize = 400;

        bool mbstart = false;
        int mstartcnt = 0;
        int mstopcnt = 0;

        int mmode = 0;                              // 0=single; 1=pair;
        GesPair mgespair = new GesPair();           // 140717;

        //int mstatictm = 0;
        //int mstatictmmin = 100; //ms;
        TimeSpan mts;
        DateTime mdtstart = DateTime.Now;
        //int mstartmax = 1000; //ms;

        //bool mbstatic = false;


        double [] mcos = new double[10];
        int[] marea = new int[10];
        string [] mdirect = new string [10];


        public Gesture()
        {
            mthis = this;
            //mtinterval = (int)mhtimer.mintervalMs;

            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;

            int sh = SystemInformation.WorkingArea.Height; // no taskbar;
            int sw = SystemInformation.WorkingArea.Width;

            int ch = sh / 2;
            int cw = sw / 2;
            
            mcenter = new Rectangle( cw-mcsize/2, ch-mcsize/2, mcsize, mcsize);

            mcos[0] = 0.9239;
            mcos[1] = 0.3827;
            mcos[2] = -0.3827;
            mcos[3] = -0.9239;
            mcos[4] = -1;

            marea[0] = 0; 
            marea[1] = 1; 
            marea[2] = 2; 
            marea[3] = 3; 
            marea[4] = 4; 
            marea[5] = 4; 
            marea[6] = 5; 
            marea[7] = 6; 
            marea[8] = 7; 
            marea[9] = 0;

            mdirect[0] = "右";
            mdirect[1] = "右上";
            mdirect[2] = "上";
            mdirect[3] = "左上";
            mdirect[4] = "左";
            //mdirect[5] = "左";
            mdirect[5] = "左下";
            mdirect[6] = "下";
            mdirect[7] = "右下";
            //mdirect[9] = "右";
        }

        public void linit()
        {
            mgespair.linit();

            //mbkworker = HookForm.gthis.getWorker();
        }
        public void start()
        {
            Parameter prm = Parameter.mthis;

            mdvmin = prm.mvmin;                         // 140715;
            mgapcnt = prm.mgaptm;
            movcnt = prm.movertime;
           
            mdismin = prm.mdismin;

            mmode = prm.mpair;

            mgespair.start();                           // 140717;

           
        }
        public void stop()
        {
            
        }

        public void onHtimerTick(int tm)
        {
            //Console.Out.WriteLine(">hi timer tick,"+tm.ToString());
            mtinterval = tm;                            // 140716;

            GetCursorPos(ref mpt32);
            onMove3(mpt32.X, mpt32.Y);
        }
        
        

        public void onMove3(int x, int y)
        {
            mptnow.X = x; mptnow.Y = y;

            double ds = cDis(mptnow, mptlast);
            // update;
            mptlast = mptnow;
            mdtlast = mdtnow;

            // cal;
            if (mtinterval <= 0)                        // 140715;
                return;
            double dv = ds / mtinterval;

            //Console.Out.WriteLine(">dv=" + dv.ToString());
            onDv(dv);

        }
        public void onDv(double dv)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (mmode == 1)
            {
                if (mgespair.ckOvtime(mtinterval))
                {
                    mstartcnt = 0;
                    mstopcnt = 0;
                    return;
                }

            }
            if (dv < mdvmin)    // 140715;Parameter.mthis.mvmin )                // 140713;
            {
                //Console.WriteLine(">  dv<mdvmin," + dv.ToString());
                if (mstopcnt < 5000) // ms;
                    mstopcnt += mtinterval;

                if (mbstart) // stop when no moving;
                {
                    mbstart = false;
                    Console.Out.WriteLine(">>stop no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    if (!onStop())
                    {
                        mstopcnt = mgapcnt;
                    }
                }

                mstartcnt = 0;
            }
            else
            {
                //Console.WriteLine(">      dv>=mdvmin," + dv.ToString());
                //if (!mbstart && (mstopcnt >= mgapcnt) && mcenter.Contains(mptnow))
                if (!mbstart && (mstopcnt >= gtStopGap()) )  // 140716; mgapcnt) )
                {

                    mbstart = true;
                    Console.Out.WriteLine(">>start, stopcnt=" + (mstopcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    onStart();
                }
                mstopcnt = 0;

                if (mstartcnt < 5000)
                    mstartcnt += mtinterval;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (mbstart && (mstartcnt > movcnt)) // stop when moving=overtime;
                {
                    mbstart = false;
                    mstartcnt = 0;
                    Console.Out.WriteLine(">> overtime, startcnt=" + (mstartcnt).ToString());
                    onStop();
                }
            }
        }
        int gtStopGap()                             // 140716;
        {
            if (mmode == 0)
            {
                return mgapcnt;
            }
            else
            {
                int gapcnt = mgespair.gtStopGap();
                if (gapcnt > 0)
                    return gapcnt;
                else
                    return mgapcnt;
            }
        }
        void onStart()
        {
            mptstart = mptnow;

            if (mmode == 1)
                mgespair.onStart();
        }
        bool onStop()
        {
            mptend = mptnow;

            // cal dis, tan;
            mdis = cDis(mptend, mptstart);
            if (mmode == 0)
            {
                if (mdis > mdismin)
                {
                    onGesture();
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (mgespair.onStop(mdis, mdismin))
                {
                    onGesture();
                    return true;
                }
                else
                    return false;
            }

        }

        private bool mreport = false;
        public bool GetReport() { return mreport; }
        public void ResetReport() { mreport = false; }

        public void onGesture()
        {
            mdx = cDx(mptend, mptstart);
            mdy = cDy(mptend, mptstart);
            double cos = mdx / mdis;
            //
            ma = ckArea(cos, mdy >= 0);
            Console.WriteLine(mdirect[ma] + "(dx,dy,dis,cos)=" + mdx.ToString() + "," + mdy.ToString()
                + "," + mdis.ToString("{.00}") + "," + cos.ToString("{0.0000}"));
            // cal up, down point;
//            cUpDown();

            /*
            if (mmode == 1)
            {
                int a = mgespair.ckPair(ma);
                if (a < 0)
                {
                    Console.WriteLine("gesture.onGesture()> ckpair failed.");
                    return;
                }
                ma = a;
            }
             */

            // report;
            mreport = true;

            // cal dis, tan;
            //double dis = cDis(mptend, mptstart);
            //double tan = cTan(mptend, mptstart);

        }
        int ckArea(double cos, bool up)
        {
            int a = 0;

            if (up)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cos >= mcos[i])
                    {
                        a = marea[i];
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cos >= mcos[i])
                    {
                        a = marea[9-i];
                        break;
                    }
                }
            }

            return a;
        }
        public void onMove2(int x, int y)
        {
            mdtnow = DateTime.Now;
            mts = mdtnow.Subtract(mdtlast);
            int dt = (int)mts.TotalMilliseconds;    // time difference;
            if (dt > mtres)
            {
                mptnow.X = x; mptnow.Y = y;
                double ds = cDis(mptnow, mptlast);  // distance;

                // update;
                mptlast = mptnow;
                mdtlast = mdtnow;

                // cal;
                double dv = ds / dt;                // velosity;

                //Console.Out.WriteLine(">dv=" + dv.ToString());
                onDv(dv);

            }
        }
        public void onDv2(double dv)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (dv < mdvmin)        // 140715;Parameter.mthis.mvmin)//mdvmin) // 
            {
                if (mstartcnt > 0)
                {
                    mstartcnt--;
                    //mstopcnt = 0;
                }
                if (mstartcnt == 0 && mbstart)
                {
                    mbstart = false;
                    //Console.Out.WriteLine(">> no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    onStop();
                    //mstopcnt = 0;
                }
                if (mstopcnt < 5000) // ms;
                    mstopcnt += mtinterval;

                //if (mbstart  ) // stop when no moving;
                //{
                //    mbstart = false;
                //    Console.Out.WriteLine(">>stop no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                //    onStop();
                //}

                //                mstartcnt = 0;
            }
            else
            {
                if (!mbstart && (mstopcnt > mgapcnt))
                {
                    mbstart = true;
                    //Console.Out.WriteLine(">>start, stopcnt=" + (mstopcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    onStart();
                }
                mstopcnt = 0;

                if (mstartcnt < 5000)
                    mstartcnt += mtinterval;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (mbstart && (mstartcnt > movcnt)) // stop when moving=overtime;
                {
                    mbstart = false;
                    mstartcnt = 0;
                    //Console.Out.WriteLine(">> overtime, startcnt=" + (mstartcnt).ToString());
                    onStop();
                }
            }
        }
        void cUpDown()
        {
            double cosup = 1, cosdown = 1;

            if (ma == 0)
            {
                cosup = 0.9239;
                cosdown = 0.9239;
            }
            else if (ma == 1)
            {
                cosup = 0.3827;
                cosdown = 0.9239;
            }
            else if (ma == 2)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (ma == 3)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (ma == 4)
            {
                cosup = -0.9239;
                cosdown = -0.9239;
            }
            else if (ma == 5)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (ma == 6)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (ma == 7)
            {
                cosup = 0.9239;
                cosdown = 0.3827;
            }
            double disup = mdis;// mdx / cosup;
            double dxup = Math.Abs(disup * cosup);//Math.Sqrt(disup * disup - mdx * mdx);
            double dyup = Math.Sqrt(disup * disup - dxup * dxup);

            double disdown = mdis;// mdx / cosdown;
            double dxdown = Math.Abs(disdown * cosdown);// Math.Sqrt(disdown * disdown - mdx * mdx);
            double dydown = Math.Sqrt(disdown * disdown - dxdown * dxdown);
            //Console.WriteLine("=>a,dir=" + ma.ToString() + "," + mdirect[ma]);
            if (ma == 0)
            {
                mptup.X = mptstart.X + (int)dxup;
                mptup.Y = mptstart.Y - (int)dyup;
                mptdown.X = mptstart.X + (int)dxdown;
                mptdown.Y = mptstart.Y + (int)dydown;
            }
            else if (ma == 1)
            {
                mptup.X = mptstart.X + (int)dxup;
                mptup.Y = mptstart.Y - (int)dyup;
                mptdown.X = mptstart.X + (int)dxdown;
                mptdown.Y = mptstart.Y - (int)dydown;
            }
            if (ma == 2)
            {
                mptup.X = mptstart.X - (int)dxup;
                mptup.Y = mptstart.Y - (int)dyup;
                mptdown.X = mptstart.X + (int)dxdown;
                mptdown.Y = mptstart.Y - (int)dydown;
            }
            if (ma == 3)
            {
                mptup.X = mptstart.X - (int)dxup;
                mptup.Y = mptstart.Y - (int)dyup;
                mptdown.X = mptstart.X - (int)dxdown;
                mptdown.Y = mptstart.Y - (int)dydown;
            }
            if (ma == 4)
            {
                mptup.X = mptstart.X - (int)dxup;
                mptup.Y = mptstart.Y + (int)dyup;
                mptdown.X = mptstart.X - (int)dxdown;
                mptdown.Y = mptstart.Y - (int)dydown;
            }
            if (ma == 5)
            {
                mptup.X = mptstart.X - (int)dxup;
                mptup.Y = mptstart.Y + (int)dyup;
                mptdown.X = mptstart.X - (int)dxdown;
                mptdown.Y = mptstart.Y + (int)dydown;
            }
            if (ma == 6)
            {
                mptup.X = mptstart.X + (int)dxup;
                mptup.Y = mptstart.Y + (int)dyup;
                mptdown.X = mptstart.X - (int)dxdown;
                mptdown.Y = mptstart.Y + (int)dydown;
            }
            if (ma == 7)
            {
                mptup.X = mptstart.X + (int)dxup;
                mptup.Y = mptstart.Y + (int)dyup;
                mptdown.X = mptstart.X + (int)dxdown;
                mptdown.Y = mptstart.Y + (int)dydown;
            }

        }
        public  void ts_dis(MouseEventArgs e)
        {
            mv.X = e.X;
            mv.Y = e.Y;

            int dis = (int)Gesture.cDis(mv, mo);
            int dis2 = Gesture.cDis2(mv, mo);
            int dx = Gesture.cDx(mv, mo);
            int dy = Gesture.cDy(mv, mo);
            double tan = Gesture.cTan(mv, mo);
            Console.Out.WriteLine(" (dis,dis2,dx,dy,tan)=("
                 + dis.ToString() + ","
                 + dis2.ToString() + ","
                 + dx.ToString() + ","
                 + dy.ToString() + ","
                 + tan.ToString("{0.00}") + ")");
        }
        public static double cDis(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;

            double f = dx*dx + dy*dy;//Math.Pow(dx) + Math.Pow(dy);
            f = Math.Sqrt(f);

            return f;
        }
        public static int cDis2(Point a, Point b)                  // 四边形逼近；
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y + b.Y);
        }
        public static int cDx(Point a, Point b)
        {
            return a.X - b.X;
        }
        
        public static int cDy(Point a, Point b)
        {
            return -(a.Y - b.Y);        // 140740; y轴方向转换；
        }
        public static double cTan(Point a, Point b)
        {
            double dy = cDy(a, b);     
            double dx = cDx(a, b);
            if (dx == 0)
            {
                if (dy > 0)
                    return 9999;
                else if (dy < 0)
                    return -9999;
                else
                    return 0;
            }
            else
            {
                return (dy / dx);
            }
        }

        public void gtMouse()
        {
            GetCursorPos(ref mpt32);
        }
    }
}
