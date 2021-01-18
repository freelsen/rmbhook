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

        public Gesture mgesture = null;
        public GesFun mgesfun = null;

        UltraHighAccurateTimer mhtimer = new UltraHighAccurateTimer();
        public BackgroundWorker mbkworker = null;

        public GestureMan()
        {
            mthis = this;
            
        }
        public int init()
        {
            mhtimer.setEvent(onHtimerTick);

            mgesture.setTinterval((int)mhtimer.mintervalMs);

            mbkworker = HookForm.gthis.getWorker();

            return 0;
        }


        public void Start()
        {
            mgesture.start();               // 140712;
            mhtimer.Start();

            //stMouseRun(true);
            mbkworker.RunWorkerAsync();
        }
        public void Stop()
        {
            //stMouseRun(false);
            mgesture.stop();
            mhtimer.Stop();
        }

        // callback by the timer thread;
        public void onHtimerTick(int tm)
        {
            mgesture.ResetReport();

            mgesture.onHtimerTick(tm);

            if (mgesture.GetReport())
            {
                //GestureMan mgesman = GestureMan.mthis;
                //mgesman.mptstart = mptstart;
                //mgesman.mptend = mptend;
                //mgesman.mdirect = mdirect[ma];
                //mgesman.ma = ma;
                //mgesman.mptup = mptup;
                //mgesman.mptdown = mptdown;
                //mgesman.mdis = mdis;

                ma = mgesture.GetMa();

                mbkworker.ReportProgress(1);

                mgesture.ResetReport();
            }
        }

        // callback by the background worker thread;
        public void Work()
        {
            // Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;      // 140715; may be not work;
            mgesfun.start();                          // 140717;
            //mgesture.work();
            mhtimer.ThreadProc2(); 
        }
        public void Progress()
        {
            mgesfun.onGesture(ma);      // 140716;
        }

        public int ma = 0;
    }
}
