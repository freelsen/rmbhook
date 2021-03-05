using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper.wow
{
    class DrawClient
    {
        public ColorGrids _colorgrids = null;

        public Dw3by3 mdw3by3 = null; // new Dw3by3();
        public DwGraph mdwgraph = null; // new DwGraph();

        public void OnSizeChanged(Rectangle rc)
        {
            int cx = rc.Width / 2;
            int cy = rc.Height / 2;

            mdwgraph.setRect(cx, cy);
            mdw3by3.setRects(cx, cy);

            // 2021-02-21,
            //int high = 2 * cy;
            //_wowroguetwo.changeSize(2*cy, 2*cx);

            // 2021-02-24;
            //misresize = true;
            _colorgrids.AlignGrids();
        }
        public void OnPaint(Graphics grap)
        {
            //mdwgraph.drawCircle(grap);
            mdw3by3.drawGraph(grap);
        }
    }
}
