using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittingHelper.libs;
using WrittingHelper.wow;

namespace WrittingHelper
{
    class FormClientEvent
    {
        public Action<Rectangle> onSizeChanged;
        public Action<Graphics> onPaint;
        //public 

        public DrawForm mdrawform = null;


        //Point mlastpos = new Point(0, 0);
        Rectangle mclientlast = new Rectangle(0, 0, 0, 0);
        public void OnSize(Rectangle rc)
        {
            //Point point = new Point(rc.Width / 2, rc.Height / 2);

            if (mclientlast.X == rc.X && mclientlast.Y == rc.Y)
                return;
            else
                mclientlast = rc;

            Lslog.log("new size=" + rc.Width.ToString() + "," + rc.Height.ToString());

            this.onSizeChanged(rc);
        }

        // ----------draw form event;----------------        
        
        public void OnParint()
        {
            Graphics grap = mdrawform.CreateGraphics();
            grap.Clear(Color.Transparent);
            //mdrawform.Invalidate();
            OnParint(grap);
        }
        public void OnParint(Graphics grap)
        {
            this.onPaint(grap);
            //mdwgraph.drawCircle(grap);
            //mdw3by3.drawGraph(grap);

            // 
            Point pt = new Point(0, 0);
            if (DrawForm.mthis != null)
            {
                pt.X = 0; pt.Y = 0;
                WinApis.ClientToScreen(DrawForm.mthis.Handle, ref pt);
                Lslog.log("drawform=" + pt.X.ToString() + "," + pt.Y.ToString());

                pt.X = 0; pt.Y = 0;
                WinApis.ClientToScreen(WowWin.mthis._hWnd, ref pt);
                Lslog.log("target=" + pt.X.ToString() + "," + pt.Y.ToString());
            }

        }
        //void onParint2(Graphics grap)
        //{
        //    Console.WriteLine("onparint");
        //    mwowman._formevent.onParint(grap);
        //}
    }
}
