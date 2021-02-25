using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyMouseDo
{
    class DrawFormMan
    {
        public static DrawFormMan _this = null;
        public DrawForm mdrawform = new DrawForm();
        public DfTarget mdftarget = new DfTarget();

        public WowMan mwowman = null;

        public IntPtr mhwnd = (IntPtr)null;

        public int init()
        {
            _this = this;

            mhwnd = mdrawform.Handle;

            mdrawform.mdfman = this;
            mdrawform.init();

            //Bound();

            return 0;
        }
        public void Bound()
        {
            if (mdftarget.findTarget())
            {
                DbMsg.Msg("target win found");
                WinApis.MoveWindow(mdrawform.Handle, 
                    mdftarget.mposition.X, mdftarget.mposition.Y
                    ,mdftarget.mrect.right- mdftarget.mrect.left, 
                    mdftarget.mrect.bottom - mdftarget.mrect.top,
                    true);
            }
        }

        public void onParint()
        {
            Graphics grap = mdrawform.CreateGraphics();
            grap.Clear(Color.Transparent);
            //mdrawform.Invalidate();
            onParint(grap);
        }
        public void onParint(Graphics grap)
        {
            Console.WriteLine("onparint");
            mwowman.onParint(grap);

            // 
            
        }

        public void doTest()
        {
            if (mdrawform.Visible == false)
            {
                mdrawform.Show();
                
                Bound();
                
                // reset background;
                Graphics grap = mdrawform.CreateGraphics();
                grap.Clear(Color.Transparent);

                // notify graphs,
                mwowman.onSize(mdftarget.mclientcenter);

                onParint(grap);
            }
            else
            {
                mdrawform.Visible = false;
            }
        }
        
        public void setTimer()
        {
            mdrawform.setTimer();
        }
        public void setTimer(bool b)
        {
            mdrawform.setTimer(b);
        }
        public void onTimer()
        {
            mwowman.onTimer2();
        }
    }
}
