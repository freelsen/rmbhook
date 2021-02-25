using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyMouseDo
{
    class WowMan
    {
        public static WowMan mthis = null;

        public WowCmd mwowcmd = new WowCmd();
        public WowMacroRogue mwowmacro = new WowMacroRogue();

        public Dw3by3 mdw3by3 = new Dw3by3();
        public DwGraph mdwgraph = new DwGraph();

        public D2c md2c = new D2c();    // 2021-02-24;

        public DrawFormMan mdfman = null;

        public WowMan()
        {
            mthis = this;
        }

        public int init()
        {
            mwowmacro.md2c = md2c;
            return 0;
        }

        // ----------draw form event;----------------        
        Point mlastpos = new Point(0, 0);
        public void onParint(Graphics grap)
        {
            //mdwgraph.drawCircle(grap);
            mdw3by3.drawGraph(grap);

            // 
            Point pt = new Point(0, 0);
            if (misshowpos)
            {
                
                Color color;
                for (int i = 0; i < mwowmacro.mdatanum; i++)
                {
                    int j = 0;
                    //for (int j = 0; j < 5; j++)
                    {
                        if ((mwowmacro.mpositions[i, j].X == 0) && (mwowmacro.mpositions[i, j].X == 0))
                            continue;
                        //    break;

                        pt = mwowmacro.mpositions[i, j];
                        mdwgraph.drawRect(grap, pt);
                        color = mwowmacro.getColor(pt);
                        DbMsg.Msg(i.ToString() + "pos (" + pt.X.ToString() + "," + pt.Y.ToString() + "), color " +
                            "(" + color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString() + ")");
                    }
                }
            }

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

        bool misresize = false;
        public void onSize(Point point)
        {
            if (mlastpos.X == point.X && mlastpos.Y == point.Y)
                return;
            else
                mlastpos = point;

            int cx = point.X;
            int cy = point.Y;
            DbMsg.Msg("new size=" + (cx * 2).ToString() + "," + (cy * 2).ToString());

            mdwgraph.setRect(cx, cy);
            mdw3by3.setRects(cx, cy);

            // 2021-02-21,
            //int high = 2 * cy;
            mwowmacro.changeSize(2*cy, 2*cx);

            // 2021-02-24;
            //misresize = true;
            md2c.InitColor();
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
        bool misshowpos = false;

        public bool onLmouseDown(int x, int y)
        //public bool onLmouseDown(MouseEventArgs e)
        {
            //int x = e.X;
            //int y = e.Y;

            bool ishandle = false;

            Point pt = new Point(x,y);
            WinApis.ScreenToClient(mdfman.mhwnd, ref pt);

            int idx =mdw3by3.getGridIndex(pt.X,pt.Y);
            if (idx > -1 && idx<9)
            {
                //if (idx == 2)
                //{
                //    if (mwowmacro.isLoot())
                //    {
                //        //for (int i = 1; i < 4; i++)
                //        {
                            
                //            ishandle = true;
                //        }
                //    }
                //}
                //else 
                if (idx == 6)
                {
                    mdfman.setTimer();
                    mdw3by3.setColor(idx);
                    mdfman.onParint();
                    
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

                DbMsg.Msg("cmd="+idx.ToString());
                
            }

            return ishandle;
        }



        // ----------------timer event;------------
        public void onTimer2()
        {
            //if (misresize)
            //{
            //    misresize = false;
            //    md2c.InitColor();
            //}
            //else
            //{
                mwowmacro.doMacro();
            //}
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
