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
    public class GestureRec
    {
        public static GestureRec mthis = null;



        public GestureRec()
        {
            mthis = this;
            //mtinterval = (int)mhtimer.mintervalMs;


            InitDirection();
        }

        public void linit()
        {
            InitStartArea();

            mgespair.linit();
        }
        public void start()
        {
            mgespair.start();                           // 140717;  
        }
        public void stop()
        {
            
        }



        // tick;
        int mticktime = 10; // ms;
        //public void setTinterval(int t) { mticktime = t; }
        Point mtickptnow = new Point(0, 0);
        Point mtickptlast = new Point(0, 0);
        double mtickvelocity = 0.0;

        // gesture cycle;
        public int mvelocitymin = 5;//2; // 40pixel/10ms;
        double mvelocitystatic = 0.5; // 2021-01-20;

        bool mgesstart = false;
        int mgesstartcnt = 0;
        int mgesstopcnt = 0;
        Point mgesptend = new Point();
        Point mgesptstart = new Point();

        double mgesdis = 0.0;
        public int mdistancemin = 40;                           // 140716;
        public int mgesgap = 60; // ms;
        public int mgesduration = 300; //ms;

        // 2021-01-29, gesture direction;
        // use direction to determin if it comes back,
        // record the max distance in the same direction;
        Point mgesdirection = new Point(0, 0);

        public bool onHtimerTick(int tm)
        {
            bool ges = false;
            //Console.Out.WriteLine(">hi timer tick,"+tm.ToString());
            mticktime = tm;                            // 140716;

            WinAPIs.POINTAPI mpt32 = new WinAPIs.POINTAPI();
            WinAPIs.GetCursorPos(ref mpt32);

            ges = onMove3(mpt32.X, mpt32.Y);
            return ges;
        }
        // stratagy 1: accurate timer;

        // start area;
        //public int[] mstartarea = new int[4];
        Rectangle mcenterrect;                   // = new Rectangle();
        int mcsize = 400;
        Point mcenterpt;
        int mradius = 100;

        void InitStartArea()
        {
            int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = Screen.PrimaryScreen.Bounds.Width;

            int sh = SystemInformation.WorkingArea.Height; // no taskbar;
            int sw = SystemInformation.WorkingArea.Width;

            int ch = sh / 2;
            int cw = sw / 2;

            mcenterrect = new Rectangle(cw - (cw/8), ch - (ch/8), cw/4, ch/4);

            mcenterpt = new Point(cw, ch);
        }

        bool CheckStartArea()
        {
            bool inside = false;

            // rectangular;
            //if (mcenterrect.Contains(pt))
            //    inside = true;
            if (mgesdisnow < mradius)
                inside = true;

            return inside;
        }

        double mtickdis = 0.0;

        public bool onMove3(int x, int y)
        {
            bool ges = false;

            mtickptlast = mtickptnow;
            mtickptnow.X = x; mtickptnow.Y = y;

            mtickdis = cDis(mtickptnow, mtickptlast);
            mtickvelocity = mtickdis / mticktime;

            if (!mgesstart)
            {
                if (CheckGesStart())
                    StartGesture();
            }
            else
            {
                if (CheckGesStop())
                {
                    StopGesture();
                    if (CheckGesture())
                    {
                        CalGesture();
                        //Console.WriteLine(mdirect[mareaidx]);
                        //Console.WriteLine(mgesstartcnt.ToString()+","+mgesdis.ToString());
                        ges = true;
                    }
                }
            }

            return ges;
        }

        int mgesturestate = 0;

        bool CheckGesStart()
        {
            bool start = false;
            
            // 2021-01-20; have to remain static for a while;
            if (mtickvelocity < mvelocitystatic)
            {
                if (mgesstopcnt < 5000) mgesstopcnt += mticktime;

                if (mgesstopcnt < gtStopGap())
                    return false;
            }

            //if (!CheckStartArea(mtickptnow))
            //{
            //    return false;
            //}

            if ((mtickvelocity > mvelocitymin))
            {
                //mgesturestate = 1;
                //Console.WriteLine(mgesturestate.ToString());
                start = true; 
            }
            return start;
        }
        bool CheckGesStop()
        {
            bool stop = false;
            mgesstartcnt += mticktime;

            if (mgesstartcnt > mgesduration) // stop when moving overtime;
            {
                Console.WriteLine("stop:overtime");
                return true;
            }

            mgesdisnow = cDis(mgesptstart, mtickptnow);
            RecordMaxDis();

            bool inside = CheckStartArea();
            //Console.WriteLine(inside.ToString());
            if (!inside && (mgesturestate == 1))
            {
                mgesturestate = 2;
                //Console.WriteLine(mgesturestate.ToString());
            }
            if (inside && (mgesturestate == 2))
            {
                mgesturestate = 3;
                //Console.WriteLine("stop:back");
                //stop = true;
            }

            //double disnow = cDis(mgesptstart, mtickptnow);
            double avgnow = mdismax / mgesstartcnt;
            if (avgnow < mvelocitystatic)//2.0)
            {
                Console.WriteLine(avgnow.ToString());
                stop = true;
            }
            //if (mtickvelocity < mvelocitymin) // stop when moving slow;
            //{
            //    // 2021-01-18; double check?
            //    if (++mgestrystopcnt >= mgestrystopmax)
            //        stop = true;
            //}

            return stop;
        }

        // 2021-01-18;
        
        //double mgesdisavg = 0.0;
        double mgesdisnow = 0.0;
        double mdismax = 0.0;
        Point mptdismax;
        // check is the distance decresed; 
        bool mgesisback = false;   // 2021-01-20;
        
        void RecordMaxDis()    // 2021-01-18;
        {
            // 2021-01-20, check the direction first; (!not done yet)
            // 2021-01-20, use the distance to check if it comes back;
            if (!mgesisback)
            {
                if (mgesdisnow > mdismax)
                {
                    mdismax = mgesdisnow;
                    mptdismax = mtickptnow;
                }
                else
                    mgesisback = true;
            }
        }

        /* 2021-01-18;
        public bool CheckGesture()
        {
            bool gesdone = false;

            if (mgesstart)//(mtickvelocity < mvelocitymin)
            {
                mgesstartcnt += mticktime;
                CalMaxDis();
            }
            else
            {
                if (mgesstopcnt < 5000) mgesstopcnt += mticktime;
            }

            if ((mtickvelocity > mvelocitymin) && (!mgesstart) && (mgesstopcnt >= gtStopGap()))
            {
                StartGesture();
            }
            else if (mgesstart && (mtickvelocity < mvelocitymin)) // stop when moving slow;
            {
                // 2021-01-18; double check?
                if (++mgestrystopcnt >= mgestrystopmax)
                    gesdone = StopGesture();
            }
            else if (mgesstart && (mgesstartcnt > mgesduration)) // stop when moving overtime;
                gesdone = StopGesture();

            return gesdone;
        }
         */

        // 2021-01-18; n times to stop;
        int mgestrystopcnt = 0;
        public int mgestrystopmax = 3;

        void StartGesture()
        {
            mgesstart = true;
            mgesstartcnt = 0;
            mgesstopcnt = 0;
            mdismax = 0.0;  // 2021-01-18;

            mgestrystopcnt = 0;
            mgesturestate = 1;

            mgesptstart = mtickptnow;

            mgesisback = false;// 2021-01-20;

            onStart(); // for gespair;

            Console.WriteLine(">>>ges start.");
        }
        void StopGesture()
        {
            mgesstart = false;
            mgesstopcnt = 0;

            //Console.WriteLine(mgesturestate.ToString());

            mgesptend = mtickptnow; // final position;

            //Console.WriteLine("ges stoped");
        }
        bool CheckGesture()
        {
            mgesdis = cDis(mgesptend, mgesptstart);

            if (mgesdis < mdismax)  // 2021-01-17, use the maximum point;
            {
                mgesdis = mdismax;
                mgesptend = mptdismax;
            }
            
            
            bool b1 = checkDistance();
            bool b2= (mgesturestate > 1);
            Console.WriteLine(mdismax.ToString() + "," + mgesturestate.ToString());
            return b1 && b2;
            //return (checkDistance()); //   { mgesstopcnt = mgesgap;  }
        }



        int mmode = 0;                              // 0=single; 1=pair;
        GesPair mgespair = new GesPair();           // 140717;

        int gtStopGap()                             // 140716;
        {
            if (mmode == 0)
            {
                return mgesgap;
            }
            else
            {
                int gapcnt = mgespair.gtStopGap();
                if (gapcnt > 0)
                    return gapcnt;
                else
                    return mgesgap;
            }
        }
        void onStart()
        {
            if (mmode == 1)
                mgespair.onStart();
        }
        bool checkDistance()
        {
            if (mmode == 0)
            {
                return (mgesdis > mdistancemin);
            }
            else
            {
                return (mgespair.onStop(mgesdis, mdistancemin));
            }
        }


        int mareaidx;
        public int GetAreaIndex() { return mareaidx; }

        public void CalGesture()
        {
            mareaidx = CalAreaIndex(mgesptstart, mgesptend);
            
            // cal up, down point;
            // cUpDown();

            if (mmode == 1)
            {
                int a = mgespair.ckPair(mareaidx);
                if (a < 0)
                {
                    Console.WriteLine("gesture.onGesture()> ckpair failed.");
                    return;
                }
                mareaidx = a;
            }
        }

        double[] mcos = new double[10];
        int[] marea = new int[10];
        public string[] mdirect = new string[10];

        void InitDirection()
        {
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

        int CalAreaIndex(Point p0, Point p1)
        {
            //pbm> the distance is in pixel unit.
            // should be in metric unit;

            double dx = cDx(p1, p0);
            double dy = cDy(p1, p0);
            double dis = cDis(p1, p0);
            double cos = dx /dis;

            //Console.WriteLine("(dx,dy,dis,cos)=" + dx.ToString() + "," + dy.ToString()
            //    + "," + dis.ToString("{.00}") + "," + cos.ToString("{0.0000}"));

            return ckArea(cos, dy >= 0);
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
                        a = marea[9 - i];
                        break;
                    }
                }
            }

            return a;
        }












        public void onVelocity(double velocity)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (mmode == 1)
            {
                if (mgespair.ckOvtime(mticktime))
                {
                    mgesstartcnt = 0;
                    mgesstopcnt = 0;
                    return;
                }

            }
            if (velocity < mvelocitymin)    // 140715;Parameter.mthis.mvmin )                // 140713;
            {
                //Console.WriteLine(">  dv<mdvmin," + dv.ToString());
                if (mgesstopcnt < 5000) // ms;
                    mgesstopcnt += mticktime;

                if (mgesstart) // stop when no moving;
                {
                    mgesstart = false;
                    //Console.Out.WriteLine(">>stop no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    if (!checkDistance())
                    {
                        mgesstopcnt = mgesgap;
                    }
                }

                mgesstartcnt = 0;
            }
            else
            {
                //Console.WriteLine(">      dv>=mdvmin," + dv.ToString());
                //if (!mbstart && (mstopcnt >= mgapcnt) && mcenter.Contains(mptnow))
                if (!mgesstart && (mgesstopcnt >= gtStopGap()) )  // 140716; mgapcnt) )
                {

                    mgesstart = true;
                    Console.Out.WriteLine(">>start, stopcnt=" + (mgesstopcnt).ToString() + "," + mtickptnow.X.ToString() + "," + mtickptnow.Y.ToString());
                    onStart();
                }
                mgesstopcnt = 0;

                if (mgesstartcnt < 5000)
                    mgesstartcnt += mticktime;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (mgesstart && (mgesstartcnt > mgesduration)) // stop when moving=overtime;
                {
                    mgesstart = false;
                    mgesstartcnt = 0;
                    Console.Out.WriteLine(">> overtime, startcnt=" + (mgesstartcnt).ToString());
                    checkDistance();
                }
            }
        }
        public void onDv2(double dv)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (dv < mvelocitymin)        // 140715;Parameter.mthis.mvmin)//mdvmin) // 
            {
                if (mgesstartcnt > 0)
                {
                    mgesstartcnt--;
                    //mstopcnt = 0;
                }
                if (mgesstartcnt == 0 && mgesstart)
                {
                    mgesstart = false;
                    //Console.Out.WriteLine(">> no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    checkDistance();
                    //mstopcnt = 0;
                }
                if (mgesstopcnt < 5000) // ms;
                    mgesstopcnt += mticktime;

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
                if (!mgesstart && (mgesstopcnt > mgesgap))
                {
                    mgesstart = true;
                    //Console.Out.WriteLine(">>start, stopcnt=" + (mstopcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    onStart();
                }
                mgesstopcnt = 0;

                if (mgesstartcnt < 5000)
                    mgesstartcnt += mticktime;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (mgesstart && (mgesstartcnt > mgesduration)) // stop when moving=overtime;
                {
                    mgesstart = false;
                    mgesstartcnt = 0;
                    //Console.Out.WriteLine(">> overtime, startcnt=" + (mstartcnt).ToString());
                    checkDistance();
                }
            }
        }


      


        // stratagey 2: 
         DateTime mdt = DateTime.Now;
        DateTime mdtnow = DateTime.Now;
        DateTime mdtlast = DateTime.Now;
        int mtres = 10;//ms;
        TimeSpan mts;

       public void onMove2(int x, int y)
        {
            mdtnow = DateTime.Now;
            mts = mdtnow.Subtract(mdtlast);
            int dt = (int)mts.TotalMilliseconds;    // time difference;
            if (dt > mtres)
            {
                mtickptnow.X = x; mtickptnow.Y = y;
                double ds = cDis(mtickptnow, mtickptlast);  // distance;

                // update;
                mtickptlast = mtickptnow;
                mdtlast = mdtnow;

                // cal;
                double dv = ds / dt;                // velosity;

                //Console.Out.WriteLine(">dv=" + dv.ToString());
                onVelocity(dv);
            }
        }





       Point mo = new Point(0, 0);
       Point mv = new Point();
       Point mptup = new Point();
       Point mptdown = new Point();

        void cUpDown()
        {
            double cosup = 1, cosdown = 1;

            if (mareaidx == 0)
            {
                cosup = 0.9239;
                cosdown = 0.9239;
            }
            else if (mareaidx == 1)
            {
                cosup = 0.3827;
                cosdown = 0.9239;
            }
            else if (mareaidx == 2)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (mareaidx == 3)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (mareaidx == 4)
            {
                cosup = -0.9239;
                cosdown = -0.9239;
            }
            else if (mareaidx == 5)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (mareaidx == 6)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (mareaidx == 7)
            {
                cosup = 0.9239;
                cosdown = 0.3827;
            }
            double disup = mgesdis;// mdx / cosup;
            double dxup = Math.Abs(disup * cosup);//Math.Sqrt(disup * disup - mdx * mdx);
            double dyup = Math.Sqrt(disup * disup - dxup * dxup);

            double disdown = mgesdis;// mdx / cosdown;
            double dxdown = Math.Abs(disdown * cosdown);// Math.Sqrt(disdown * disdown - mdx * mdx);
            double dydown = Math.Sqrt(disdown * disdown - dxdown * dxdown);
            //Console.WriteLine("=>a,dir=" + ma.ToString() + "," + mdirect[ma]);
            if (mareaidx == 0)
            {
                mptup.X = mgesptstart.X + (int)dxup;
                mptup.Y = mgesptstart.Y - (int)dyup;
                mptdown.X = mgesptstart.X + (int)dxdown;
                mptdown.Y = mgesptstart.Y + (int)dydown;
            }
            else if (mareaidx == 1)
            {
                mptup.X = mgesptstart.X + (int)dxup;
                mptup.Y = mgesptstart.Y - (int)dyup;
                mptdown.X = mgesptstart.X + (int)dxdown;
                mptdown.Y = mgesptstart.Y - (int)dydown;
            }
            if (mareaidx == 2)
            {
                mptup.X = mgesptstart.X - (int)dxup;
                mptup.Y = mgesptstart.Y - (int)dyup;
                mptdown.X = mgesptstart.X + (int)dxdown;
                mptdown.Y = mgesptstart.Y - (int)dydown;
            }
            if (mareaidx == 3)
            {
                mptup.X = mgesptstart.X - (int)dxup;
                mptup.Y = mgesptstart.Y - (int)dyup;
                mptdown.X = mgesptstart.X - (int)dxdown;
                mptdown.Y = mgesptstart.Y - (int)dydown;
            }
            if (mareaidx == 4)
            {
                mptup.X = mgesptstart.X - (int)dxup;
                mptup.Y = mgesptstart.Y + (int)dyup;
                mptdown.X = mgesptstart.X - (int)dxdown;
                mptdown.Y = mgesptstart.Y - (int)dydown;
            }
            if (mareaidx == 5)
            {
                mptup.X = mgesptstart.X - (int)dxup;
                mptup.Y = mgesptstart.Y + (int)dyup;
                mptdown.X = mgesptstart.X - (int)dxdown;
                mptdown.Y = mgesptstart.Y + (int)dydown;
            }
            if (mareaidx == 6)
            {
                mptup.X = mgesptstart.X + (int)dxup;
                mptup.Y = mgesptstart.Y + (int)dyup;
                mptdown.X = mgesptstart.X - (int)dxdown;
                mptdown.Y = mgesptstart.Y + (int)dydown;
            }
            if (mareaidx == 7)
            {
                mptup.X = mgesptstart.X + (int)dxup;
                mptup.Y = mgesptstart.Y + (int)dyup;
                mptdown.X = mgesptstart.X + (int)dxdown;
                mptdown.Y = mgesptstart.Y + (int)dydown;
            }

        }
        public  void ts_dis(MouseEventArgs e)
        {
            mv.X = e.X;
            mv.Y = e.Y;

            int dis = (int)GestureRec.cDis(mv, mo);
            int dis2 = GestureRec.cDis2(mv, mo);
            int dx = GestureRec.cDx(mv, mo);
            int dy = GestureRec.cDy(mv, mo);
            double tan = GestureRec.cTan(mv, mo);
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
            WinAPIs.POINTAPI mpt32 = new WinAPIs.POINTAPI();
            WinAPIs.GetCursorPos(ref mpt32);
        }
    }
}
