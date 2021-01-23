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
    class GestureDetectByDirectionOne : GestureDetectByDirection
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


        public GestureDetectByDirectionOne()
        {
        }

        public override void init()
        {
            InitStartArea();
        }
        public override void start()
        {
            mtextman.Open();    // 2021-01-22;
        }
        public override void stop()
        {
            mtextman.Close();
        }

        public override bool onTimerTick(int tm)
        {
            bool ges = false;
            //Console.Out.WriteLine(">hi timer tick,"+tm.ToString());
            mtickTime = tm;                            // 140716;

            // get mouse location;
            WinAPIs.POINTAPI mpt32 = new WinAPIs.POINTAPI();
            WinAPIs.GetCursorPos(ref mpt32);

            ges = onMouseMove3(mpt32.X, mpt32.Y);
            return ges;
        }


        //-----------------------------------------------------------------
        
        bool onMouseMove3(int x, int y)
        {
            bool ges = false;

            TickUpdate(x,y);

            if (!misGesStart)
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

        void StartGesture()
        {
            misGesStart = true;
            mgesStartedTime = 0;
            mgesStopedTime = 0;
            mdismax = 0.0;  // 2021-01-18;

            mgestrystopcnt = 0;
            mgesturestate = 1;

            mgesBeginPos = mtickPositionNow;

            mgesisback = false;// 2021-01-20;

            Console.WriteLine(">>>ges start.");
        }
        void StopGesture()
        {
            misGesStart = false;
            mgesStopedTime = 0;

            //Console.WriteLine(mgesturestate.ToString());

            mgesEndPos = mtickPositionNow; // final position;
        }




        static string mfilename = "d:\\temp\\rmbhook\\mouse.txt";
        TextMan mtextman = new TextMan(mfilename);

        void TickUpdate(int x, int y)
        {
            mtickPositionLast = mtickPositionNow;
            mtickPositionNow.X = x; mtickPositionNow.Y = y;

            mtickDistance = GestureCommon.cDis(mtickPositionNow, mtickPositionLast);
            mtickSpeed = mtickDistance / mtickTime;

            // save; 2021-01-22;
            //mtextman.WriteLine(x.ToString() + "," + y.ToString() + "," + mtickSpeed.ToString("f4"));
        }




        // this state is used to record how the mouse goes.
        int mgesturestate = 0;

        bool CheckGesStart()
        {
            bool start = false;

            // 2021-01-20; have to remain static for a while;
            if (mtickSpeed < mSpeedStatic)
            {
                if (mgesStopedTime < 5000) mgesStopedTime += mtickTime;

                if (mgesStopedTime < mgesGapTime)//gtStopGap())
                    return false;
            }

            // this check is not used for poor result;
            //if (!CheckStartArea(mtickptnow))
            //{
            //    return false;
            //}

            if ((mtickSpeed > mSpeedMin))
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
            mgesStartedTime += mtickTime;

            if (mgesStartedTime > mgesDurationTime) // stop when moving overtime;
            {
                Console.WriteLine("stop:overtime");
                return true;
            }

            mgesdisnow = GestureCommon.cDis(mgesBeginPos, mtickPositionNow);
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
            double avgnow = mdismax / mgesStartedTime;
            if (avgnow < mSpeedStatic)//2.0)
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

// start test;

        // start test: one condition;
        // start area approach;
        // id> mouse must start from the center area;
        // could be a rectangular or a circle;

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
                    mptdismax = mtickPositionNow;
                }
                else
                    mgesisback = true;
            }
        }

        // 2021-01-29, gesture direction;
        // use direction to determin if it comes back,
        // record the max distance in the same direction;
        Point mgesDirection = new Point(0, 0);



// start & stop gesture;

        // 2021-01-18; n times to stop;
        int mgestrystopcnt = 0;
        public int mgestrystopmax = 3;


        bool CheckGesture()
        {
            mgesDistance = GestureCommon.cDis(mgesEndPos, mgesBeginPos);

            if (mgesDistance < mdismax)  // 2021-01-17, use the maximum point;
            {
                mgesDistance = mdismax;
                mgesEndPos = mptdismax;
            }


            bool b1 = (mgesDistance > mgesDistanceMin);//checkDistance();
            bool b2= (mgesturestate > 1);
            Console.WriteLine(mdismax.ToString() + "," + mgesturestate.ToString());
            return b1 && b2;
            //return (checkDistance()); //   { mgesstopcnt = mgesgap;  }
        }




        void CalGesture()
        {
            mdirectionIndex = mGestureDirection.CalAreaIndex(mgesBeginPos, mgesEndPos);
        }

    }
}
