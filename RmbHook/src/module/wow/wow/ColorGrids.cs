using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper.wow
{
    class ColorGrids
    {
        public Func<int, int, Color> getColorClient;
        public IntPtr targetHwnd { get; set; }

 
        public static int mgridrow = 2;
        public static int mgridcol = 100;
        Color[,] mcolors = new Color[mgridrow, mgridcol];
        Point[,] mgridpos = new Point[mgridrow, mgridcol];


        public int GetVal(int idx)
        {
            Point pt = mgridpos[1, idx];
            //Color c = FetchColor.getColorClient(DfTarget.mthis.mhwnd, pt.X, pt.Y);
            Color c = this.getColorClient(pt.X, pt.Y);
            int d = Color2Int(c);
            //DbMsg.Msg("idx=" + idx.ToString() +
            //    ",pos=(" + pt.X.ToString() + "," + pt.Y.ToString()
            //    + "),val=" + d.ToString());
            return d;
        }

        public void AlignGrids()
        {
            Lslog.log("align grids");
            // 
            int dx = 3;
            int dy = 3;
            int xsize = mgridcol * dx * 2;
            int ysize = mgridrow * dy * 2;
            int[,] cdata = new int[ysize, xsize];

            this.fetchColors(cdata, xsize, ysize);

            // align;
            int xnum = this.alignColors(cdata, xsize);

            // set position;
            this.setGridPos(cdata, xnum, ysize);


            // align;
            Lslog.log("align grids done");
        }



        public Point GetGridPos(int idx)
        {
            if ((idx >= 0) && (idx < mgridcol))
                return mgridpos[1, idx];
            else
                return new Point(-1,-1);
        }

        #region private;
        void fetchColors(int[,] cdata, int xsize, int ysize)
        {
            // get a large area;
            Color[,] colors = new Color[ysize, xsize];


            for (int iy = 0; iy < ysize; iy++)
            {
                for (int jx = 0; jx < xsize; jx++)
                {
                    //Color c = FetchColor.getColorClient(DfTarget.mthis.mhwnd, jx, iy);
                    Color c = this.getColorClient(jx, iy);
                    //DbMsg.Msg("(" + iy.ToString() + "," + jx.ToString()
                    //    +")=(" + c.R.ToString() + "," + c.G.ToString() + "," + c.B.ToString());
                    colors[iy, jx] = c;
                    cdata[iy, jx] = Color2Int(colors[iy, jx]);
                }
            }
        }
        int alignColors(int[,] cdata, int xsize)
        {
            int d = 0;
            int idx = 0;
            for (int i = 0; i < xsize; i++)
            {
                if (cdata[0, i] == d)
                {
                    mgridpos[0, idx].X = i + 1;
                    mgridpos[1, idx].X = i + 1;
                    idx++;
                    d++;
                }
                if (idx == mgridcol)
                    break;
            }

            return idx;
        }
        void setGridPos(int[,] cdata, int xnum, int ysize)
        {
            int d = 0;
            int idx = 0;
            for (int i = 0; i < ysize; i++)
            {
                if (cdata[i, 0] == d)
                {
                    for (int j = 0; j < xnum; j++)
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
        }

        int Color2Int(Color color)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            int val = b + g * 256 + r * 256 * 256;
            return val;
        }
        #endregion

    }
}
