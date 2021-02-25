using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    // index start from 0,
    public enum Wenum : int
    {
        phpmax = 10,
        phpcur = 11,
        pmanamax = 12,
        pmanacur = 13,
        plevel = 14,
        ppoint = 54,
        pcombat=42,
        xpos=1,
        ypos=2,

        ttype = 55,
        trange = 15,
        thpmax = 18,
        thpcur = 19,
        tname1 = 16,
        tname2 = 17,

        ashoot = 34,
        asinster = 35,
        aauto=36,
        asteal=53

    }
    // 2021-02-24,g
    class D2c
    {
        public static int mgridrow = 2;
        public static int mgridcol = 100;
        Color[,] mcolors = new Color[mgridrow, mgridcol];
        Point[,] mgridpos = new Point[mgridrow, mgridcol];


        public int getVal(Wenum we)
        {
            int idx = (int)we;
            return getVal(idx);
        }
        public int getVal(int idx)
        {
            Point pt = mgridpos[1, idx];
            Color c = FetchColor.getColorClient(DfTarget.mthis.mhwnd, pt.X, pt.Y);
            int d = Color2Int(c);
            //DbMsg.Msg("idx=" + idx.ToString() +
            //    ",pos=(" + pt.X.ToString() + "," + pt.Y.ToString()
            //    + "),val=" + d.ToString());
            return d;
        }
        public void InitColor()
        {
            DbMsg.Msg("init color");
            // get a large area;
            int dx = 3;
            int dy = 3;
            int xsize = mgridcol*dx*2;
            int ysize = mgridrow * dy * 2;
            Color[,] colors = new Color[ysize, xsize];
            int[,] cdata = new int[ysize, xsize];

            
            for (int iy = 0; iy < ysize; iy++)
            {
                for (int jx = 0; jx < xsize; jx++)
                {
                    Color c=FetchColor.getColorClient(DfTarget.mthis.mhwnd, jx, iy);
                    //DbMsg.Msg("(" + iy.ToString() + "," + jx.ToString()
                    //    +")=(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString());
                    colors[iy, jx] = c;
                    cdata[iy, jx] = Color2Int(colors[iy,jx]);
                }
            }

            // align;
            

            int d = 0;
            int idx = 0;
            for (int i=0; i<xsize;i++)
            {
                if (cdata[0,i]==d)
                {
                    mgridpos[0,idx].X = i+1;
                    mgridpos[1, idx].X = i+1;
                    idx++;
                    d++;
                }
                if (idx == mgridcol)
                    break;
            }
            int xnum = idx;
            d = 0;
            idx = 0;
            for (int i=0;i<ysize;i++)
            {
                if(cdata[i,0]==d)
                {
                    for (int j=0;j<xnum;j++)
                    {
                        mgridpos[0, j].Y = i + 1;
                        mgridpos[1, j].Y = i + 1;
                    }
                    idx++;
                    d++;
                }
                if (idx == mgridrow)
                    break;
            }

            // align;
            DbMsg.Msg("init color done");
        }
        public int Color2Int(Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            int val = b + g * 256 + r * 256 * 256;
            return val;
        }
    }
}
