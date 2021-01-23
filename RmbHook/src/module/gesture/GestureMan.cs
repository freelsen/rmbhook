using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace RmbHook
{
    class GestureMan
    {
        public static GestureMan mthis = null;

        GestureParamter mGesPrm = new GestureParamter();

        int mgesIndex = 0;
        GestureDetectByDirectionOne mGesByDir = new GestureDetectByDirectionOne();
        GestureDirectionCommand mGesDirCmd = new GestureDirectionCommand();
        GestureDirection mGesDir = new GestureDirection();

        bool mrunning = false;
        public BackgroundWorker mBkgWorker = null;
        UltraHighAccurateTimer mUhaTimer = new UltraHighAccurateTimer();

        public GestureMan()
        {
            mthis = this;
            
        }
        public int init()
        {

            mGesPrm.mgesfun = mGesDirCmd;
            mGesPrm.mgesture = mGesByDir;
            mGesPrm.init();

            mGesByDir.mGestureDirection = mGesDir;
            mGesByDir.init();

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
            mGesDirCmd.onGesture(mgesIndex);      // 140716;
        }


        // callback by the timer thread;
        public void onTimerTick(int tm)
        {
            if (mGesByDir.onTimerTick(tm))
            {
                mgesIndex = mGesByDir.getDirectionIndex();
                Console.WriteLine(mGesDir.mdirect[mgesIndex]);
                //Console.WriteLine(mgesfun.mkeys[ma].ToString());

                mBkgWorker.ReportProgress(1);
            }
        }
        
    }
}
