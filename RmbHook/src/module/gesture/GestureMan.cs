using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace KeyMouseDo
{
    class GestureMan
    {
        public static GestureMan mthis = null;

        GestureParamter mGesPrm = new GestureParamter();

        int mgesIndex = 0;
        GestureDetectByDirectionOne mGesByDirOne = new GestureDetectByDirectionOne();
        GestureDetectByDirectionAxis mGesByDirAxis = new GestureDetectByDirectionAxis();
        GestureDetectByDirection mGesByDir = null;
        GestureDirectionCommand mGesDirCmd = new GestureDirectionCommand();
        GestureDirection mGesDir = new GestureDirection();

        bool mrunning = false;
        public BackgroundWorker mBkgWorker = null;
        UltraHighAccurateTimer mUhaTimer = new UltraHighAccurateTimer();
        public WowMan mwowman = null;


        public GestureMan()
        {
            mthis = this;
            
        }
        public int init()
        {

            mGesPrm.mgesfun = mGesDirCmd;
            mGesPrm.mgesture = mGesByDirOne;
            mGesPrm.init();

            //
            mGesByDirOne.mGestureDirection = mGesDir;
            mGesByDirOne.init();

            mGesByDirAxis.mGestureDirection = mGesDir;
            mGesByDirAxis.init();
            mGesByDir = mGesByDirAxis;

            mGesDirCmd.init();

            mGesDirCmd.mwinmon = WinMon.mthis;// 2021-01-22;

            mUhaTimer.setEvent(onTimerTick);

            //mBkgWorker = HookForm.gthis.getWorker();

            return 0;
        }

        public void Start()
        {
            if (mrunning) return;
            mrunning = true;

            mGesByDir.start();               // 140712;
            mUhaTimer.Start();

            //stMouseRun(true);
            mBkgWorker.RunWorkerAsync();
        }
        public void Stop()
        {
            //stMouseRun(false);
            mGesByDir.stop();
            mUhaTimer.Stop();
            mrunning = false;
        }

        // callback by the background worker thread;
        public void Work()
        {
            // Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;      // 140715; may be not work;
            //mGesDirCmd.start();                          // 140717;
            //mgesture.work();
            mUhaTimer.ThreadProc2();
        }

        public void Progress()
        {
            //mwowman.onMouseStop();      // 2021-02-13;
            //return;

            mGesDirCmd.onGesture(mgesIndex);      // 140716;
        }
        public void Progress(int idx)
        {
            mGesDirCmd.onGesture(idx);      // 140716;
        }


        // callback by the timer thread;
        //volatile int mtime = 0;
        //volatile bool misbusy = false;
        public void onTimerTick(int tm)
        {
            //if(mwowman.onTimerTick(tm))
            //{
            //    mBkgWorker.ReportProgress(1);
            //}
            //return;

            if (mGesByDir.onTimerTick(tm))
            {
                mgesIndex = mGesByDir.getDirectionIndex();
                string str=mGesDir.mdirect[mgesIndex];
                Console.WriteLine(str);
                //Console.WriteLine(mgesfun.mkeys[ma].ToString());

                mBkgWorker.ReportProgress(1);
            }
        }


        void test()
        {
            // Code in this region can be aborted without affecting
            // other tasks.
            //
            Thread.BeginCriticalRegion();

            //
            // The host might decide to unload the application domain
            // if a failure occurs in this code region.
            //
            Thread.EndCriticalRegion();
            //
            // Code in this region can be aborted without affecting
            // other tasks.
        }
        
    }
}
