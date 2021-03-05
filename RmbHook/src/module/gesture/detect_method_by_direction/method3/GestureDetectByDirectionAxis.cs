using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WrittingHelper.libs;

// 2021-01-23, Sheng Li
// 02:24:07;
// id> 
// - the gesture need to start from a point, and come back to that point;
// - 
namespace WrittingHelper
{
    class GestureDetectByDirectionAxis : GestureDetectByDirection
    {

        public GestureDetectByDirectionAxis()
        {
        }

        public override void init()
        {
        }
        public void exit()
        {

        }
        bool misrun = false;

        public override void start()
        {
            if (misrun)
                return;
            else
                misrun = true;

            mtkspds.Reset();
            mtktims.Reset();

            initData();
            resetState();

            WinApis.GetCursorPos(ref mtickPos);

            mtextman.Open();
            //mtextman.WriteLine("aldkjfafd");
            //mtextman.Close();
            mtextman2.Open();
        }
        public override void stop()
        {
            mtextman.Close();
            mtextman2.Close();

            misrun = false;
        }

        public override bool onTimerTick(int ticktime)
        {
            bool isgesture = false;

            //Console.WriteLine("tick time=" + ticktime.ToString());
            // record status;
            mtickktime = ticktime;
            updatePosition();// update cursor position;
            updateAvgSpeed();   // update all the time;

            if (checkGestureTrigger())
            {                
                // update the tracking data base;
                enqueueData();// in queue;
                updateFiler();

                // check start;
                if (checkGestureStart())
                {
                    //Console.WriteLine("is start="+misgesStart.ToString());
                    // check stop;
                    if (checkGestureStop())
                    {
                        // check if it is a gesture;
                        if (checkGesture())
                        {
                             // determine the direction;
                            calGesture();
                            //Console.WriteLine("ges=" + mGestureDirection.ToString());
                            isgesture=true;
                        }

                        //
                        resetState();
                    }
                }
            }

            return isgesture;
        }

        // status;
        int mtickktime = 0;
        int mtqueue = 0;  // time duration starting from the first data;
        Point mtickPos = new Point(0, 0);
        Point mtickPosLast = new Point(0, 0);
        //int mxnow = 0; //location; x;
        //int mynow = 0;
        //int mxlast = 0;
        //int mylast = 0;
        int mdx = 0;    
        int mdy = 0;

        double mtickspeed;
        double mtickDis;

        
        // parameters for gesture judgement;
        int mgesDistanceMin = 40; // pixels;
        double mSpeedStatic = 0.4;
        double mGesTriggerSpeed = 1.0;
        double mgesSpeedStartMin = 0.2;//2.0; //  pixels/1ms;
        double mgesSpeedMin = 2.0; // 10 pixels/1ms;

        int mGesTimeMin = 50; // ms;
        int mgesTimeOver = 500; //ms;
        //int mGesTime

        // status for gesture;
        bool misGestureTriggered = false;
        bool misgesStart = false;
        int mgesdlen = 0;

        int[] mdSum;        // point to the maximum;
        int[] mdOneDirSum;
        int mContinuousIndex = 0;
        int mgesAxis = 0;   // 1=x; 2=y;
        int mgesDirSign = 1;    // 1, -1;
        int mgesPeakIdx = 0;

        Point mgesBeginPos;
        Point mgesPeakPos;
        Point mgesEndPos;
        double mgesPeakDis;
        double mgesPeakDisAxis;

        double mgesSpeedAvg = 0.0;
        double mgesSpeedSum = 0.0;


        void resetState()
        {
            misStatic = false;

            mdxpos.Reset();
            mdypos.Reset();
            mdtpos.Reset();
            mxpos.Reset();
            mypos.Reset();

            misGestureTriggered = false;
            misContinuous = true;
            misgesStart = false;
            //Console.WriteLine("onFirstData");
            mgesdlen = 0;
            mtqueue = 0;
            mgesSpeedSum = 0.0;

            misxcontinuous = true;
            misycontinuous = true;
            misgesBack = false;
        }
        

        // check;

        bool checkGestureTrigger()
        {
            if (!misGestureTriggered)
            {
                if (checkStatic())
                {
                    if (mtickspeed > mGesTriggerSpeed)
                    {
                        #if DEBUG
                        Console.WriteLine("trigged," + mtickPos.X.ToString() + "," + mtickPos.Y.ToString() + "," + mtickspeed.ToString("f2"));
#endif
                        misGestureTriggered = true;
                    }
                }
            }
            return misGestureTriggered;
        }

        // trigger from an anverage speed;
        //double[] mspeeds = new double[mdatasize];
        static int mstaticsize = 100;
        IndexedQueue<double> mtkspds = new IndexedQueue<double>(mstaticsize);
        IndexedQueue<int> mtktims = new IndexedQueue<int>(mstaticsize);
        double mavgSpeedSum = 0.0;
        int mavgTime = 0;

        double mavgSpeed = 0.0;
        bool misStatic = false;


        bool checkStatic()
        {
            if (!misStatic)
            {
                if (mavgSpeed < 0.1)
                {
                    misStatic = true;
                }
            }
            return misStatic;
        }

        void updateAvgSpeed()
        {
            if (mtkspds.Count == mstaticsize)
            {
                Console.WriteLine("!!!! BUG !!!");
                //return 0.0;
            }

            if (mavgTime < 50)
            {
                mtkspds.Enqueue(mtickspeed);
                mtktims.Enqueue(mtickktime);

                if (mtkspds.Count == 1)
                {
                    mavgTime = mtickktime;
                    mavgSpeedSum = mtickspeed;
                }
                else
                {
                    mavgTime += mtickktime;
                    mavgSpeedSum += mtickspeed;
                }

                mavgSpeed = 100;    // wait;
            }
            else
            {
                mavgSpeedSum-=mtkspds.Dequeue();
                mavgTime -= mtktims.Dequeue();

                mavgSpeed = mavgSpeedSum / (double)mavgTime;
            }

            
            //return mavgSpeed;
        }



        bool checkGestureStart()
        {
            if (!misgesStart)
            {
                //
                if (!checkContinuous())     // continuous stoped;
                {
                    // check times;
                    if (mtqueue > 10)
                    {
                        //if (!misContinuous)
                        //{
                        //Console.WriteLine("continuous end,len="+mgesdlen.ToString());
                        // check the distance, speed;
                        updateMaxOneDirSumSet();

                        // +ls> a mistable consumed me a lot of time here;
                        int dis = Math.Abs(mdOneDirSum[mgesdlen - 1]);
                        if (dis > mgesDistanceMin)
                        {
                            //double speed = (double)dis / (double)mtqueue;
                            double speed=mgesSpeedAvg;
                            //Console.WriteLine("try start dis=" + dis.ToString()+",spd="+speed.ToString("f2"));
                            if ( speed> mgesSpeedStartMin)
                            {
                                misgesStart = true;

                                // record status;
                                mgesBeginPos = new Point(mxpos[0], mypos[0]);
                                mgesPeakPos = mtickPos;//new Point(mxnow, mynow);
                                mgesPeakDis = GestureCommon.calDistance(mgesBeginPos, mgesPeakPos);
                                mgesPeakDisAxis = (double)dis;
                                Console.WriteLine(">>ges start," + speed.ToString("f2") + ",(" + mgesBeginPos.X.ToString() + "," + mgesBeginPos.Y.ToString());
                            }
                            else
                                Console.WriteLine("..start check failed=speed .."+speed.ToString("f2"));
                        }
                        else
                            Console.WriteLine("..start check failed=distance ..");
                    }
                    else
                        Console.WriteLine("..start check failed=time..");

                    if (!misgesStart)
                    {
                        Console.WriteLine("<<x ges start failed.size="+mgesdlen.ToString());
                        resetState();
                    }
                }
            }
            return misgesStart;
        }

        bool misxcontinuous = false;
        bool misycontinuous = false;
        int mgesXPeakIdx = 0;
        int mgesYPeakIdx = 0;

        bool checkSign(int a, int b)
        {
            return (((a > 0) && (b > 0)) || ((a < 0) && (b < 0)));  //strict;
            //return ((a >= 0 && b >= 0) || (a <= 0 && b <= 0));
        }
        bool checkContinuous()
        {
            if (mgesdlen > 1)
            {
                if (misxcontinuous)
                {
                    if (!checkSign(mdxpos[mgesdlen - 1], mdxpos[mgesdlen - 2]))
                    {
                        //Console.WriteLine("(" + mdxpos[mgesdlen - 1].ToString() + "),(" + mdxpos[mgesdlen - 1].ToString()+")");
                        misxcontinuous = false;
                        mgesXPeakIdx = mgesdlen - 1;
                        mgesAxis = 1;       // record who stops at last;
                    }
                }

                if (misycontinuous)
                {
                    if (!checkSign(mdypos[mgesdlen - 1], mdypos[mgesdlen - 2]))
                    {
                        misycontinuous = false;
                        mgesYPeakIdx = mgesdlen - 1;
                        mgesAxis = 2;
                    }
                }

                if (!misxcontinuous && !misycontinuous)
                {
                    Console.WriteLine("discontinued,size=" + mgesdlen.ToString());
                    misContinuous = false;
                }
            }
            return misContinuous;
        }

        bool checkContinuous2()
        {
            if (misxcontinuous)
            {
                double d = mdx * mdxDirSign;
                //Console.WriteLine("mdx=" + mdx.ToString()+",sign="+mdxDirSign.ToString());
                if (d <= 0)
                {
                    
                }
            }
            if (misycontinuous)
            {
                double d = mdy * mdyDirSign;
                if (d <= 0)
                {
                    mgesYPeakIdx = mgesdlen - 1;
                    misycontinuous = false;
                    mgesAxis = 2;
                }
            }

            if (!misxcontinuous && !misycontinuous)
            {
#if DEBUG
                Console.WriteLine("discontinued,data=" + mgesdlen.ToString());
#endif
                misContinuous = false;
            }
            return misContinuous;
        }

        bool misgesBack = false;        // is continuos to check the gesture?
        bool checkGestureStop()
        {
            bool isstop = false;

            // overtime;
            //Console.WriteLine("tm=" + mtqueue.ToString());
            if (mtqueue > mgesTimeOver)
            {
                Console.WriteLine("stop: overtime");
                //record();
                return true;
            }
            // is come back;
            if (!misgesBack)
            {

                // method 1
                int dlen = mgesdlen - 1;
                bool ispass=!checkSign(mdOneDirSum[mgesPeakIdx], mdOneDirSum[dlen]);
                bool isclose=(Math.Abs(mdOneDirSum[0]-mdOneDirSum[dlen])<20)?true:false;
                //if (ispass || isclose)

                // method 2;
                //Console.WriteLine("check back=" + d.ToString());
                //int d = Math.Abs(mdSum[mgesdlen - 1]);
                //if (d > 20)//mgesDistanceMin)

                // method 3;
                //int d = mgesDirSign * (mdOneDirSum[mgesPeakIdx] - mdOneDirSum[dlen]);
                int d = mgesDirSign * (mdPos[mgesPeakIdx]-mdPos[dlen]);
#if DEBUG
                Console.WriteLine("check back=" + d.ToString());
#endif
                if (d>20)//mgesDistanceMin)                
                {
                    misgesBack = true;
                }
            }
            else
            {
#if DEBUG
                Console.WriteLine("stop check spd=" + mgesSpeedAvg.ToString("f2"));
#endif
                if (mgesSpeedAvg < mSpeedStatic)
                // come to static;
                //double spda = mgesPeakDis / (double)mtqueue;
                //Console.WriteLine("stop check spd=" + spda.ToString());
                //if (spda < mSpeedStatic)
                {
                    //misgesBack = true;
                    Console.WriteLine("-->stop: gesture static,spd=" + mavgSpeed.ToString());
                    return true;
                }
            }
            if (mgesSpeedAvg < 0.1)
            // come to static;
            //double spda = mgesPeakDis / (double)mtqueue;
            //Console.WriteLine("stop check spd=" + spda.ToString());
            //if (spda < mSpeedStatic)
            {
                //misgesBack = true;
                Console.WriteLine("-->stop: static,spd=" + mavgSpeed.ToString());
                return true;
            }


            return isstop;
        }

     
        bool checkGesture()
        {
            bool isges = false;

            Console.WriteLine(mgesBeginPos.X.ToString() + "," + mgesBeginPos.Y.ToString());
            Console.WriteLine(mgesPeakPos.X.ToString() + "," + mgesPeakPos.Y.ToString());
            Console.WriteLine(mtickPos.X.ToString() + "," + mtickPos.Y.ToString());
            record();

            if (mtqueue > mgesTimeOver)
            {
                Console.WriteLine("check: overflow");
                return false;
            }
            if (mtqueue < mGesTimeMin)
            {
                Console.WriteLine("check: too short");
                return false;
            }

            if (misgesBack)
            {
                return true;
            }
            else
            {
                Console.WriteLine("check: no come back,d=");
                return false;
            }
            

            //// is come back;
            //if (!misgesBack)
            //{
            //    Console.WriteLine("check: no come back");
            //    return false;
            //}

            // come back;
            if (false)
            {
                //int d = mgesDirSign * (mdOneDirSum[mgesPeakIdx] - mdOneDirSum[dlen]);
                int d = 0;
                if (mgesAxis == 1) //x
                    d = mgesPeakPos.X - mtickPos.X;//mgesBeginPos.X;
                else
                    d = mgesPeakPos.Y - mtickPos.Y;//mgesBeginPos.Y;
                //d = (d > 0) ? d : -d;
                if (mgesDirSign * d > 20)
                {
                    isges = true;
                }
                else
                {
                    int x = mxpos[0];
                    int dx = mdxpos[0];
                    int dlen = mgesdlen - 1;
                    Console.WriteLine("check: no come back,d=" + d.ToString());

                    Console.WriteLine(mgesPeakPos.X.ToString() + "," + mgesPeakPos.Y.ToString());
                    Console.WriteLine(mtickPos.X.ToString() + "," + mtickPos.Y.ToString());
                }
            }

            //double dis = GestureCommon.calDistance(new Point(mxnow, mynow), mgesBeginPos);
            //double dis2 = mgesPeakDisAxis-mgesPeakDisAxis / 3;
            //Console.WriteLine("check comeback," + d.ToString() + "," + dis2.ToString("f2"));
            //if (d < dis2)
            ////if (dis<mgesPeakDis/3)
            //{
            //    Console.WriteLine("stop: comeback");
            //    misgesBack = true;
            //    return true;
            //}
            

            //double dis = GestureCommon.calDistance(mgesBeginPos, mgesPeakPos);
            //double speed = mgesPeakDis / (double)mtqueue;

            //Console.WriteLine("ges check spd=" + speed.ToString("f2"));
            //if (speed > mgesSpeedMin)
            //{
            //    //Console.WriteLine("ges checked!!!");
            //    isges = true;
            //}
            

            return isges;
        }

        void calGesture()
        {
            mdirectionIndex = mGestureDirection.CalAreaIndex(mgesBeginPos, mgesPeakPos);
        }

        //



        void updatePosition()
        {
            WinApis.GetCursorPos(ref mtickPos);
            mdx = mtickPos.X - mtickPosLast.X;
            mdy = mtickPos.Y - mtickPosLast.Y;

            mtickDis = GestureCommon.calDistance(mtickPos, mtickPosLast);
            mtickspeed = mtickDis / (double)mtickktime;

            mtickPosLast = mtickPos;
        }

        // the data buffer, design;
        // - work as a queue: fifo;
        // - track data as long as `timemax` duration;
        // - record the summation at any duration;

        static int mdatasize=1000;
        // location;
        IndexedQueue<int> mxpos = new IndexedQueue<int>(mdatasize * 10);
        IndexedQueue<int> mypos = new IndexedQueue<int>(mdatasize * 10);
        // derivative;
        IndexedQueue<int> mdxpos = new IndexedQueue<int>(mdatasize*10);
        IndexedQueue<int> mdypos = new IndexedQueue<int>(mdatasize*10);
        IndexedQueue<int> mdtpos = new IndexedQueue<int>(mdatasize * 10);
        // integral;
        int[] mdxsum = new int[mdatasize];
        int[] mdysum = new int[mdatasize];
        int[] mdtsum = new int[mdatasize];
        // integral on the same direction;
        // 2021-01-24, this filter is used to identify the starting of the gesture;
        // it gives integral of data in the same direction `continuously`.
        int[] mdxOneDirSum = new int[mdatasize];
        int[] mdyOneDirSum = new int[mdatasize];
        int mdxDirSign = 1;     // sign of the direction;
        int mdyDirSign = 1;
        bool misContinuous = false;
        // 

        void initData()
        {
            for (int i = 0; i < mdxsum.Length; i++)
            {
                mdxsum[i] = 0;
                mdysum[i] = 0;
                mdtsum[i] = 0;

                mdxOneDirSum[i] = 0;
                mdyOneDirSum[i] = 0;
            }
            mdxpos.Reset();
            mdypos.Reset();
            mdtpos.Reset();
            mxpos.Reset();
            mypos.Reset();

            mtqueue = 0;
        }

        void enqueueData()
        {
            if (mxpos.Count > 300)     // something wrong happened here;
            {
                Console.WriteLine("-!!!!!!!!!! <bugs> mxpos.Count>=");
                resetState();
            }

            //Console.WriteLine("enq=" + mtickPos.X.ToString());
            mxpos.Enqueue(mtickPos.X); mypos.Enqueue(mtickPos.Y);
            mdxpos.Enqueue(mdx); mdypos.Enqueue(mdy);
            mdtpos.Enqueue(mtickktime);

            mgesdlen++;
            mtqueue += mtickktime;

            mgesSpeedSum += mtickspeed;
            mgesSpeedAvg = mgesSpeedSum / (double)mtqueue;

            int dlen = mdxpos.Count - 1;
            if (dlen == 0)
            {
                mdxsum[0] = mdx;
                mdysum[0] = mdy;
                mdtsum[0] = mtickktime;
            }
            else
            {
                mdxsum[dlen] = mdxsum[dlen - 1] + mdx;
                mdysum[dlen] = mdysum[dlen - 1] + mdy;
                mdtsum[dlen] = mdtsum[dlen - 1] + mtickktime;
            }
        }

        void updateFiler()
        {
            // continuous-sign filter;
            int dlen = mdxpos.Count - 1;
            if (dlen == 0)
            {
                mdxOneDirSum[0] = mdx;
                mdyOneDirSum[0] = mdy;

                mdxDirSign = (mdx > 0) ? 1 : -1;    // what if =0?
                mdyDirSign = (mdy > 0) ? 1 : -1;
            }
            else
            {
                mdxOneDirSum[dlen] = mdxOneDirSum[dlen - 1] +mdx;
                mdyOneDirSum[dlen] = mdyOneDirSum[dlen - 1] +mdy;
            }
        }


        void dequeueData()
        {
            // out queue;

            if ((mtqueue > mgesTimeOver))
            {
                mgesdlen--;

                int dlen = mxpos.Count - 1;

                //return;
                mxpos.Dequeue(); mypos.Dequeue();

                int dx = mdxpos.Dequeue();
                int dy = mdypos.Dequeue();
                int dt = mdtpos.Dequeue();

                mtqueue -= dt;

                for (int i = 0; i < dlen; i++)
                {
                    mdxsum[i] = mdxsum[i + 1] - dx; //mdxsum[i] + mdxpos[i] - dx;
                    mdysum[i] = mdysum[i + 1] - dy; // mdysum[i] + mdypos[i] - dy;
                    mdtsum[i] = mdtsum[i + 1] - dt; // mdtsum[i] + mdtpos[i] - dt;
                    // <!> while here I found the default queue's limitation;
                    // you cannot get the data by their position;

                    mdxOneDirSum[i] = mdxOneDirSum[i + 1];
                    mdyOneDirSum[i] = mdyOneDirSum[i + 1];

                    //
                    if (dx > 0)
                    {
                        mdxOneDirSum[i] -= dx; //-=dx;
                    }
                    if (dy > 0) mdyOneDirSum[i] -= dy;//-= dy;
                }
            }

        }

        IndexedQueue<int> mdPos;
        void updateMaxOneDirSumSet()
        {
            // used to record the gesture started on which axis direction;
            // rule> the maximum value of continuous integral;
            if (mgesAxis == 1)
            {
                mdPos = mxpos;
                mdOneDirSum = mdxOneDirSum;
                //Console.WriteLine(mdxPositiveSum[dlen].ToString());
                mdSum = mdxsum;
                mgesPeakIdx = mgesXPeakIdx;
                mgesDirSign = mdxDirSign;
            }
            else if (mgesAxis == 2)
            {
                mdPos = mypos;
                mdOneDirSum = mdyOneDirSum;
                mdSum = mdysum;
                mgesPeakIdx = mgesYPeakIdx;
                mgesDirSign = mdyDirSign;
            }
        }

        
        static string mfilename = "d:\\temp\\rmbhook\\d1.txt";
        TextMan mtextman = new TextMan(mfilename);
        static string mfilename2 = "d:\\temp\\rmbhook\\d2.txt";
        TextMan mtextman2 = new TextMan(mfilename2);

        void record()
        {
            Console.WriteLine("------------record------------");

            mtextman.WriteLine("x position");
            for (int i = 0; i < mxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mxpos[i].ToString() + "\t");
            }
            mtextman.WriteLine("");

            mtextman.WriteLine("dx sum one dir");
            //mtextman.WriteLine(mdxDirSign.ToString());
            for (int i = 0; i < mdxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mdxOneDirSum[i].ToString() + "\t");
            }
            mtextman.WriteLine("");

            mtextman.WriteLine("dx sum");
            //mtextman.WriteLine(mdxDirSign.ToString());
            for (int i = 0; i < mdxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mdxsum[i].ToString() + "\t");
            }
            mtextman.WriteLine("");

            mtextman.WriteLine("y position");
            for (int i = 0; i < mxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mypos[i].ToString() + "\t");
            }
            mtextman.WriteLine("");

            mtextman.WriteLine("dy sum one dir");
            //mtextman.WriteLine(mdxDirSign.ToString());
            for (int i = 0; i < mdxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mdyOneDirSum[i].ToString() + "\t");
            }
            mtextman.WriteLine("");

            mtextman.WriteLine("dy sum");
            //mtextman.WriteLine(mdxDirSign.ToString());
            for (int i = 0; i < mdxpos.Count; i++)
            {
                //mtextman.wri
                mtextman.Write(mdysum[i].ToString() + "\t");
            }
            mtextman.WriteLine("");
        }

    }
}
