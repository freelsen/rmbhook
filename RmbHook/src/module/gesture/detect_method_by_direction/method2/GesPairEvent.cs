using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    public class GesPairEvent
    {
        delegate void EventFun( );
        Queue<EventFun> mevents = new Queue<EventFun>();

        bool mrun = false;

        int mpaircnt = 0;
        int mpairgap = 20;
        int mpairwant = -1;
        int mpair = -1;
        int mpairout = -1;

        int movtime = 400;
        int mtmcnt = 0;
        bool misovertime = false;
        
        double mdismin = 20;
        double mdis = 0;
        bool misvalid = false;

        public int linit() { 
            return 0; 
        }

        public void start()
        {
            mpaircnt = 0;
        }

        
        // ----- interface --------------------------------------------
        public void onStart()
        {
            incPaircnt();
        }
        public bool onStop(double dis)
        {
            setDistance(dis);
            return misvalid;
        }
        public bool ckOvtime(int tm)
        {
            setOvertime(tm);
            return misovertime;
        }
        public int ckPair(int a)
        {
            setPair(a);
            return mpairout;
        }

        // --- change state, not trig event ------------------
        void reset()
        {
            mpaircnt = 0;
            mpairwant = -1;
            mtmcnt = 0;

            mrun = false;
        }

        void incPaircnt() { mpaircnt++; }

        // --- event trigger; --------------------------------
        // trig when state(variable) changed;
        void setOvertime(int tm)
        {
            mtmcnt += tm;
            mevents.Enqueue(new EventFun(onOvertime));
            loopEvent();
        }
        void setDistance(double dis)
        {
            mdis = dis;
            mevents.Enqueue(new EventFun(onDistance));
            loopEvent();
        }
        void setPair(int a)
        {
            mpair = a;
            mevents.Enqueue(new EventFun(onPair));
            loopEvent();
        }

        // -- event loop; --------------------------------------
        void loopEvent()
        {
            while (mevents.Count > 0)
            {
                EventFun ef = mevents.Dequeue();
                ef();
            }
        }

        // --- event handler; -------------------------------------------
        void onOvertime()
        {
            if (mtmcnt >= movtime)
            {
                misovertime = true;
                reset();
            }
            else
                misovertime = false;
        }
        void onDistance()
        {
            if (mpaircnt == 0)
                misvalid = false;
            else if (mpaircnt == 1 && mdis < mdismin)
                misvalid = false;
            else
                misvalid = true;
        }
        void onPair()
        {
            if (mpaircnt == 1)
            {
                mpairwant = mpair;
                mpairout = -1;
            }
            else
            {
                if (mpairwant < 4 && (mpair == mpairwant + 4))
                {
                    mpairout = mpairwant;
                    return;
                }
                else if (mpairwant >= 4 && (mpair == mpairwant - 4))
                {
                    mpairout = mpairwant;
                    return;
                }
                else
                    mpairout = -1;
            }
            
        }

        
    }
}
