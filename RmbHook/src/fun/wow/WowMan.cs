using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class WowMan
    {
        public static WowMan mthis = null;

        public WowCmd mwowcmd = new WowCmd();
        public WowMacroRogue mwowmacro = new WowMacroRogue();

        public Dw3by3 mdw3by3 = new Dw3by3();
        public DwGraph mdwgraph = new DwGraph();

        public DrawFormMan mdfman = null;

        public WowMan()
        {
            mthis = this;
        }

        public int init()
        {
            return 0;
        }

        // ----------draw form event;----------------        

        public void onParint(Graphics grap)
        {
            //mdwgraph.drawCircle(grap);
            mdw3by3.drawGraph(grap);

            // 
            Point pt = new Point(0, 0);
            pt=mwowmacro.mphpos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mphpos;
            pt.X += Convert.ToInt32(0.5 * mwowmacro.mthlen);
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mphpos;
            pt.X += Convert.ToInt32(1.0 * mwowmacro.mthlen);
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mtpos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mthpos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mthpos;
            pt.X += Convert.ToInt32(1.0 * mwowmacro.mthlen);
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mroguep1pos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mroguep5pos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mroguehidepos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mpengerpos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            pt = mwowmacro.mc2pos;
            DfTarget.mthis.screenToClient(ref pt);
            mdwgraph.drawCircle(grap, pt);

            if (DrawForm.mthis != null)
            {
                pt.X = 0; pt.Y = 0;
                WinApis.ClientToScreen(DrawForm.mthis.Handle, ref pt);
                DbMsg.Msg("drawform=" + pt.X.ToString() + "," + pt.Y.ToString());

                pt.X = 0; pt.Y = 0;
                WinApis.ClientToScreen(DfTarget.mthis.mhWnd, ref pt);
                DbMsg.Msg("target=" + pt.X.ToString() + "," + pt.Y.ToString());
            }

        }
        public void onSize(Point point)
        {
            int cx = point.X;
            int cy = point.Y;

            mdwgraph.setRect(cx, cy);
            mdw3by3.setRects(cx, cy);
        }

        


        // -------------mouse event;------------
        public void onLDouble()
        {
            mwowcmd.doMap();
        }
        public void onRDouble()
        {
            mwowcmd.doAutoRun();
        }
        public bool onRmouseDown(int x,int y)
        {
            //onMove(x, y);
            return false;
        }

        public bool onLmouseDown(int x, int y)
        {
            bool ishandle = false;

            
            int idx =mdw3by3.getGridIndex(x,y);
            if (idx > -1 && idx<9)
            {
                if (idx == 5)
                {
                    mdfman.setTimer();
                    mdw3by3.setColor(idx);
                    mdfman.onParint();
                }
                else
                    mwowcmd.doCmd(idx);
                
                DbMsg.Msg("cmd="+idx.ToString());
                ishandle = true;
            }

            return ishandle;
        }



        // ----------------timer event;------------
        public void onTimer2()
        {
            mwowmacro.doMacro();
        }





        int mxlast = 0;
        int mylast = 0;
        int mtlast = 0;
        Point mpt = new Point(0, 0);
        int mdx = 0;
        int mdy = 0;
        int mcnt = 0;

        public bool onTimerTick(int ticktime)
        {
            bool isstop = false;
            WinApis.GetCursorPos(ref mpt);

            //double dr=Math.Sqrt()
            mdx = Math.Abs(mpt.X - mxlast);
            mdy = Math.Abs(mpt.Y - mylast);
            mxlast = mpt.X;
            mylast = mpt.Y;


            int dmax = (mdx > mdy) ? mdx : mdy;
            if (dmax < 10)
                mcnt++;
            else
                mcnt = 0;
            if (mcnt == 5)
            {
                //mcnt = 0;
                isstop = true;
            }

            return isstop;
        }
        public void onMouseStop()
        {
            onMove(mxlast, mylast);
        }


        public void onMove(int x, int y)
        {

            int cx = x - DfTarget.mthis.mposition.X;
            int cy = y - DfTarget.mthis.mposition.Y;

            onSize(new Point(cx, cy));

            mdfman.onParint();

            Console.WriteLine("move");
        }
    }
}
