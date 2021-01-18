using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace RmbHook
{
    /// 
    /// ManualTimer
    /// A simulated timer by loop 
    /// It creates a new thread in Thread Pool using ThreadPool class
    /// Nocky Tian @ 2008-3-16
    /// 
    /// The timer starts a new thread using  object,
    /// and the value of the property Priority is set to 
    /// so that the accuray could be kept 1ms around.
    /// 
    /// 
    /// 
    /// 
    public class UltraHighAccurateTimer
    {
        [StructLayout(LayoutKind.Explicit, Size = 8)]
        public struct LARGE_INTEGER
        {
            [FieldOffset(0)]
            public Int64 QuadPart;
            [FieldOffset(0)]
            public UInt32 LowPart;
            [FieldOffset(4)]
            public Int32 HighPart;
        }
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out LARGE_INTEGER lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out  LARGE_INTEGER lpFrequency);

        public delegate void ManualTimerEventHandler( int tm);
        public event ManualTimerEventHandler mtickevent;
        private object threadLock = new object();       // for thread safe
        
        private LARGE_INTEGER clockFrequency;            // result of QueryPerformanceFrequency() 
        private LARGE_INTEGER intevalTicks;
        private LARGE_INTEGER nextTriggerTime;               // the time when next task will be executed
        
        bool running = true;
        Thread mthread = null;

        public long mintervalMs;                     // interval in mimliseccond;

        public void stInterval( long value)         // ms;
        {
                mintervalMs = value;
                intevalTicks.QuadPart = (value * clockFrequency.QuadPart / 1000);
        }

        public void setEvent(ManualTimerEventHandler tickHandler)
        {
            mtickevent += new UltraHighAccurateTimer.ManualTimerEventHandler(tickHandler);
        }

        public UltraHighAccurateTimer()
        {
            if (QueryPerformanceFrequency(out clockFrequency) == false)
            {
                // Frequency not supported
                throw new Win32Exception("QueryPerformanceFrequency() function is not supported");
            }

            stInterval(10);                         // 10ms;
        }
        public void Start()
        {
            //mthread.Start();                      //  thread run;

            //int interval = Parameter.mthis.mtminterval;
            //stInterval(interval);

            running = true;
        }
        public void Stop()
        {
            running = false;
        }

        public void ThreadProc2()                   // 140715;
        {
            LARGE_INTEGER StartingTime,ElapsedMicroseconds;

            LARGE_INTEGER currTime;
            GetTick(out currTime);
            nextTriggerTime.QuadPart = currTime.QuadPart + intevalTicks.QuadPart;

            StartingTime.QuadPart = currTime.QuadPart;

            while (running)
            {
                //while (currTime.QuadPart < nextTriggerTime.QuadPart)
                {
                    GetTick(out currTime);
                }
                //nextTriggerTime.QuadPart = currTime.QuadPart + intevalTicks.QuadPart;

                ElapsedMicroseconds.QuadPart = currTime.QuadPart - StartingTime.QuadPart;
                StartingTime.QuadPart = currTime.QuadPart;

                ElapsedMicroseconds.QuadPart *= 1000;
                ElapsedMicroseconds.QuadPart /= clockFrequency.QuadPart;

                //Console.WriteLine(">curtime=" + currTime.QuadPart.ToString() );
                //Console.WriteLine(DateTime.Now.ToString("ss.ffff") + ",ElapsedMicroseconds=" + ElapsedMicroseconds.QuadPart.ToString());

                if (mtickevent != null)
                {
                    mtickevent((int)ElapsedMicroseconds.QuadPart);
                }
                Thread.Sleep((int)mintervalMs);                    // 140715;
            }
        }


        public bool GetTick(out LARGE_INTEGER currentTickCount)
        {
            if (QueryPerformanceCounter(out currentTickCount) == false)
                throw new Win32Exception("QueryPerformanceCounter() failed!");
            else
                return true;
        }


        public void createThread()
        {
            //mtickevent += new UltraHighAccurateTimer.ManualTimerEventHandler();

            mthread = new Thread(new ThreadStart(ThreadProc));
            mthread.Name = "HighAccuracyTimer";
            mthread.Priority = ThreadPriority.AboveNormal;
        }
        public void ThreadProc()
        {

            //LARGE_INTEGER StartingTime,ElapsedMicroseconds;

            LARGE_INTEGER currTime;
            GetTick(out currTime);
            nextTriggerTime.QuadPart = currTime.QuadPart + intevalTicks.QuadPart;
            
            //StartingTime.QuadPart = currTime.QuadPart;

            while (running)
            {
                while (currTime.QuadPart < nextTriggerTime.QuadPart)
                {
                    GetTick(out currTime);
                } 
                nextTriggerTime.QuadPart = currTime.QuadPart + intevalTicks.QuadPart;
                
                //ElapsedMicroseconds.QuadPart = currTime.QuadPart - StartingTime.QuadPart;
                //StartingTime.QuadPart = currTime.QuadPart;
                //ElapsedMicroseconds.QuadPart *= 1000;
                //ElapsedMicroseconds.QuadPart /= clockFrequency.QuadPart;

                //Console.WriteLine(">curtime=" + currTime.QuadPart.ToString() );
                //Console.WriteLine(DateTime.Now.ToString("ss.ffff") + ",ElapsedMicroseconds=" + ElapsedMicroseconds.QuadPart.ToString());
                
                if (mtickevent != null)
                {
                    mtickevent((int)this.mintervalMs);
                }
            }
        }

        protected void OnTick()
        {
            if (mtickevent != null)
            {
                mtickevent(0);
            }
        }



        ~UltraHighAccurateTimer()
        {
            running = false;
            if (mthread!=null)
                mthread.Abort();
        }
    }
}
