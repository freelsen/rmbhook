using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WrittingHelper.libs;
using WrittingHelper.wow;

namespace WrittingHelper
{
    class WowEvent
    {
        public DrawFormMan mdfman =null;

        public WowCmd mwowcmd = null;
        public RogueTwo _wowroguetwo = null;
        public ColorGrids _colorgrids = null;
        public Dw3by3 mdw3by3 = null;
        public WowProc _botpump = null;

        void ToggleBot()
        {
            //mdfman.mdrawform.setTimer();
            this._botpump.Toggle();
        }
        // ----------------timer event;------------
        public void onTimer()
        {
            _wowroguetwo.DoMacro();
        }
        public void OnLoop()
        {
            this._wowroguetwo.DoMacro();
            Thread.Sleep(1000);
        }


        // -------------mouse event;------------
        public void onLmouseDown(Object sender, EventArgs e)
        {
            MouseArgsR er = (MouseArgsR)e;
            bool b = this.onLmouseDown(er.X,er.Y);
            if (b)
                er.ishandled = true;
        }
        public void onRmouseDown(Object sender, EventArgs e)
        {
            MouseArgsR er = (MouseArgsR)e;
            bool b = this.onRmouseDown(er.X, er.Y);
            if (b)
                er.ishandled = true;
        }
        public void onLmouseDouble(Object sender, EventArgs e)
        {
            MouseArgsR er = (MouseArgsR)e;
            this.onLDouble();
        }
        public void onRmouseDouble(Object sender, EventArgs e)
        {
            MouseArgsR er = (MouseArgsR)e;
            this.onRDouble();
        }


        // ----------------------------------------
        void onLDouble()
        {
            mwowcmd.doMap();
        }
        void onRDouble()
        {
            mwowcmd.doAutoRun();
        }


        bool onRmouseDown(int x, int y)
        {
            //onMove(x, y);
            return false;
        }
        bool misshowpos = false;

        bool onLmouseDown(int x, int y)
        {
            //int x = e.X;
            //int y = e.Y;

            bool ishandle = false;

            Point pt = new Point(x, y);
            WinApis.ScreenToClient(mdfman.mformhwnd, ref pt);

            int idx = mdw3by3.getGridIndex(pt.X, pt.Y);
            if (idx > -1 && idx < 9)
            {
                if (idx == 6)
                {
                    this.ToggleBot();
                    mdw3by3.setColor(idx);
                    mdfman._formclientevent.OnParint();

                    ishandle = true;
                }
                else if (idx == 7)
                {
                    //mdwgraph.changeColor();
                    misshowpos = !misshowpos;

                    ishandle = true;
                }
                else
                {
                    mwowcmd.doCmd(idx);
                    ishandle = true;
                }

                DbMsg.Msg("cmd=" + idx.ToString());

            }

            return ishandle;
        }

        
        
    }
}
