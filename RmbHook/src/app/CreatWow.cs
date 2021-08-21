using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper
{
    class CreatWow
    {
        // 2021-08-21, purpose: to split wow module easily from the project,
        // q1> what if the WowMan class is not exist?
        // all code should not refer to it, so we need another class to represet it,
        // this agent class should exist all the time,

        //public WowMan mwowman = new WowMan();
        public int init()
        {
            //mwowman.mdfman = mdrawFormMan;
            //mwowman.mhookhandler = this.mHookEventHandler;
            //mwowman.init();     // 2021-02-13,

            //mdrawFormMan.mwowman = mwowman;
            //mGestureMan.mwowman = mwowman;  //2021-02-13;

            return 0;
        }

        public IntPtr mformhwnd;

        public void OnParint()
        {

        }


        // WowEvent;
        public void onLmouseDown(Object sender, EventArgs e)
        {

        }
        public void onRmouseDown(Object sender, EventArgs e)
        {

        }
        public void onLmouseDouble(Object sender, EventArgs e)
        {

        }
        public void onRmouseDouble(Object sender, EventArgs e)
        {

        }

        // WowMan
        public void OnSizeChanged(Rectangle rc)
        {

        }
        public void OnPaint(Graphics grap)
        {

        }
        //WowMan, worker;
        public void doWork()
        {

        }
        public void reportWork()
        {

        }
        public void doneWork()
        {

        }
        public void runWorker()
        {

        }
    } 
}
