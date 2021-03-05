using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace WrittingHelper
{
    class GestureDetectByDirectionFromMouse : GestureDetectByDirection
    {
        // tick;
        int mtickTime = 0; // ms;
        double mtickDistance = 0.0;
        Point mtickPositionNow = new Point(0, 0);
        Point mtickPositionLast = new Point(0, 0);
        double mtickSpeed = 0.0;

        // gesture cycle;
        bool misGesStart = false;
        int mgesStartedTime = 0;
        int mgesStopedTime = 0;
        Point mgesEndPos = new Point();
        Point mgesBeginPos = new Point();
        double mgesDistance = 0.0;

        // parameters;
        double mSpeedStatic = 0.5; // 2021-01-20;
        public int mSpeedMin = 5;//2; // 40pixel/10ms;
        public int mgesDistanceMin = 40;                           // 140716;
        public int mgesGapTime = 60; // ms;
        public int mgesDurationTime = 300; //ms;


        // stratagey 2: 
        DateTime mdt = DateTime.Now;
        DateTime mdtnow = DateTime.Now;
        DateTime mdtlast = DateTime.Now;
        int mtres = 10;//ms;
        TimeSpan mts;

        public override void init()
        {
            mgespair.linit();
        }
        public override void start()
        {

            mgespair.start();                           // 140717;
        }

        public void onMove2(int x, int y)
        {
            mdtnow = DateTime.Now;
            mts = mdtnow.Subtract(mdtlast);
            int dt = (int)mts.TotalMilliseconds;    // time difference;
            if (dt > mtres)
            {
                mtickPositionNow.X = x; mtickPositionNow.Y = y;
                double ds = GestureCommon.calDistance(mtickPositionNow, mtickPositionLast);  // distance;

                // update;
                mtickPositionLast = mtickPositionNow;
                mdtlast = mdtnow;

                // cal;
                double dv = ds / dt;                // velosity;

                //Console.Out.WriteLine(">dv=" + dv.ToString());
                onVelocity(dv);
            }
        }

        public void onVelocity(double velocity)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (mmode == 1)
            {
                if (mgespair.ckOvtime(mtickTime))
                {
                    mgesStartedTime = 0;
                    mgesStopedTime = 0;
                    return;
                }

            }

            if (velocity < mSpeedMin)    // 140715;Parameter.mthis.mvmin )                // 140713;
            {
                //Console.WriteLine(">  dv<mdvmin," + dv.ToString());
                if (mgesStopedTime < 5000) // ms;
                    mgesStopedTime += mtickTime;

                if (misGesStart) // stop when no moving;
                {
                    misGesStart = false;
                    //Console.Out.WriteLine(">>stop no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    if (!checkDistance())
                    {
                        mgesStopedTime = mgesGapTime;
                    }
                }

                mgesStartedTime = 0;
            }
            else
            {
                //Console.WriteLine(">      dv>=mdvmin," + dv.ToString());
                //if (!mbstart && (mstopcnt >= mgapcnt) && mcenter.Contains(mptnow))
                if (!misGesStart && (mgesStopedTime >= gtStopGap()))  // 140716; mgapcnt) )
                {

                    misGesStart = true;
                    Console.Out.WriteLine(">>start, stopcnt=" + (mgesStopedTime).ToString() + "," + mtickPositionNow.X.ToString() + "," + mtickPositionNow.Y.ToString());
                    onStart();
                }
                mgesStopedTime = 0;

                if (mgesStartedTime < 5000)
                    mgesStartedTime += mtickTime;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (misGesStart && (mgesStartedTime > mgesDurationTime)) // stop when moving=overtime;
                {
                    misGesStart = false;
                    mgesStartedTime = 0;
                    Console.Out.WriteLine(">> overtime, startcnt=" + (mgesStartedTime).ToString());
                    checkDistance();
                }
            }
        }

        public void onDv2(double dv)
        {
            //Console.WriteLine(">dv=" + dv.ToString());
            if (dv < mSpeedMin)        // 140715;Parameter.mthis.mvmin)//mdvmin) // 
            {
                if (mgesStartedTime > 0)
                {
                    mgesStartedTime--;
                    //mstopcnt = 0;
                }
                if (mgesStartedTime == 0 && misGesStart)
                {
                    misGesStart = false;
                    //Console.Out.WriteLine(">> no moving, startcnt=" + (mstartcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    checkDistance();
                    //mstopcnt = 0;
                }
                if (mgesStopedTime < 5000) // ms;
                    mgesStopedTime += mtickTime;

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
                if (!misGesStart && (mgesStopedTime > mgesGapTime))
                {
                    misGesStart = true;
                    //Console.Out.WriteLine(">>start, stopcnt=" + (mstopcnt).ToString() + "," + mptnow.X.ToString() + "," + mptnow.Y.ToString());
                    onStart();
                }
                mgesStopedTime = 0;

                if (mgesStartedTime < 5000)
                    mgesStartedTime += mtickTime;
                //Console.Out.WriteLine(">>mstartcnt=" + mstartcnt.ToString());

                if (misGesStart && (mgesStartedTime > mgesDurationTime)) // stop when moving=overtime;
                {
                    misGesStart = false;
                    mgesStartedTime = 0;
                    //Console.Out.WriteLine(">> overtime, startcnt=" + (mstartcnt).ToString());
                    checkDistance();
                }
            }
        }


        int mmode = 0;                              // 0=single; 1=pair;
        GesPair mgespair = new GesPair();           // 140717;

        int gtStopGap()                             // 140716;
        {
            if (mmode == 0)
            {
                return mgesGapTime;
            }
            else
            {
                int gapcnt = mgespair.gtStopGap();
                if (gapcnt > 0)
                    return gapcnt;
                else
                    return mgesGapTime;
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
                return (mgesDistance > mgesDistanceMin);
            }
            else
            {
                return (mgespair.onStop(mgesDistance, mgesDistanceMin));
            }
        }

        void CalGesture()
        {
            mdirectionIndex = mGestureDirection.CalAreaIndex(mgesBeginPos, mgesEndPos);

            // cal up, down point;
            // cUpDown();

            if (mmode == 1)
            {
                int a = mgespair.ckPair(mdirectionIndex);
                if (a < 0)
                {
                    Console.WriteLine("gesture.onGesture()> ckpair failed.");
                    return;
                }
                mdirectionIndex = a;
            }
        }


        Point mo = new Point(0, 0);
        Point mv = new Point();
        Point mptup = new Point();
        Point mptdown = new Point();

        void cUpDown()
        {
            double cosup = 1, cosdown = 1;

            if (mdirectionIndex == 0)
            {
                cosup = 0.9239;
                cosdown = 0.9239;
            }
            else if (mdirectionIndex == 1)
            {
                cosup = 0.3827;
                cosdown = 0.9239;
            }
            else if (mdirectionIndex == 2)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (mdirectionIndex == 3)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (mdirectionIndex == 4)
            {
                cosup = -0.9239;
                cosdown = -0.9239;
            }
            else if (mdirectionIndex == 5)
            {
                cosup = -0.9239;
                cosdown = -0.3827;
            }
            else if (mdirectionIndex == 6)
            {
                cosup = -0.3827;
                cosdown = 0.3827;
            }
            else if (mdirectionIndex == 7)
            {
                cosup = 0.9239;
                cosdown = 0.3827;
            }
            double disup = mgesDistance;// mdx / cosup;
            double dxup = Math.Abs(disup * cosup);//Math.Sqrt(disup * disup - mdx * mdx);
            double dyup = Math.Sqrt(disup * disup - dxup * dxup);

            double disdown = mgesDistance;// mdx / cosdown;
            double dxdown = Math.Abs(disdown * cosdown);// Math.Sqrt(disdown * disdown - mdx * mdx);
            double dydown = Math.Sqrt(disdown * disdown - dxdown * dxdown);
            //Console.WriteLine("=>a,dir=" + ma.ToString() + "," + mdirect[ma]);
            if (mdirectionIndex == 0)
            {
                mptup.X = mgesBeginPos.X + (int)dxup;
                mptup.Y = mgesBeginPos.Y - (int)dyup;
                mptdown.X = mgesBeginPos.X + (int)dxdown;
                mptdown.Y = mgesBeginPos.Y + (int)dydown;
            }
            else if (mdirectionIndex == 1)
            {
                mptup.X = mgesBeginPos.X + (int)dxup;
                mptup.Y = mgesBeginPos.Y - (int)dyup;
                mptdown.X = mgesBeginPos.X + (int)dxdown;
                mptdown.Y = mgesBeginPos.Y - (int)dydown;
            }
            if (mdirectionIndex == 2)
            {
                mptup.X = mgesBeginPos.X - (int)dxup;
                mptup.Y = mgesBeginPos.Y - (int)dyup;
                mptdown.X = mgesBeginPos.X + (int)dxdown;
                mptdown.Y = mgesBeginPos.Y - (int)dydown;
            }
            if (mdirectionIndex == 3)
            {
                mptup.X = mgesBeginPos.X - (int)dxup;
                mptup.Y = mgesBeginPos.Y - (int)dyup;
                mptdown.X = mgesBeginPos.X - (int)dxdown;
                mptdown.Y = mgesBeginPos.Y - (int)dydown;
            }
            if (mdirectionIndex == 4)
            {
                mptup.X = mgesBeginPos.X - (int)dxup;
                mptup.Y = mgesBeginPos.Y + (int)dyup;
                mptdown.X = mgesBeginPos.X - (int)dxdown;
                mptdown.Y = mgesBeginPos.Y - (int)dydown;
            }
            if (mdirectionIndex == 5)
            {
                mptup.X = mgesBeginPos.X - (int)dxup;
                mptup.Y = mgesBeginPos.Y + (int)dyup;
                mptdown.X = mgesBeginPos.X - (int)dxdown;
                mptdown.Y = mgesBeginPos.Y + (int)dydown;
            }
            if (mdirectionIndex == 6)
            {
                mptup.X = mgesBeginPos.X + (int)dxup;
                mptup.Y = mgesBeginPos.Y + (int)dyup;
                mptdown.X = mgesBeginPos.X - (int)dxdown;
                mptdown.Y = mgesBeginPos.Y + (int)dydown;
            }
            if (mdirectionIndex == 7)
            {
                mptup.X = mgesBeginPos.X + (int)dxup;
                mptup.Y = mgesBeginPos.Y + (int)dyup;
                mptdown.X = mgesBeginPos.X + (int)dxdown;
                mptdown.Y = mgesBeginPos.Y + (int)dydown;
            }

        }
        public void ts_dis(MouseEventArgs e)
        {
            mv.X = e.X;
            mv.Y = e.Y;

            int dis = (int)GestureCommon.calDistance(mv, mo);
            int dis2 = GestureCommon.cDis2(mv, mo);
            int dx = GestureCommon.cDx(mv, mo);
            int dy = GestureCommon.cDy(mv, mo);
            double tan = GestureCommon.cTan(mv, mo);
            Console.Out.WriteLine(" (dis,dis2,dx,dy,tan)=("
                 + dis.ToString() + ","
                 + dis2.ToString() + ","
                 + dx.ToString() + ","
                 + dy.ToString() + ","
                 + tan.ToString("{0.00}") + ")");
        }
    }
}
