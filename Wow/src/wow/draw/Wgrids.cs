using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ls.libs;

namespace WoW.wow
{
    class Wgrids
    {

        // 2D grids,
        public static Point[,] GetGridCenter(Rectangle[,] grids, int row, int col)
        {
            Point[,] centpos = new Point[row, col];
            for (int r = 0; r < row; r++)
                for (int c = 0; c < col; c++)
                {
                    Rectangle rc = grids[r, c];
                    centpos[r, c] = new Point(rc.X + rc.Width / 2, rc.Y + rc.Height / 2);
                    Lslog.log($"grid center({r.ToString()},{c.ToString()})=({centpos[r, c].X.ToString()},{centpos[r, c].Y.ToString()})");
                }

            return centpos;
        }
        public static void SetGridsCenter(Rectangle[,] grids, int cx, int cy, int dx, int dy, int row, int col)
        {
            int wid =dx * col;
            int hig = dy * row;
            int x = cx - Convert.ToInt32( ((float)wid) / 2);
            int y = cy - Convert.ToInt32( ((float)hig) / 2);
            SetGrids(grids, x, y, dx, dy, row, col);
                
        }
        public static void SetGrids(Rectangle[,] grids,int x, int y, int dx, int dy, int row, int col)
        {
            // 
            for (int r=0; r<row; r++)
                for (int c=0; c<col; c++)
                {
                    grids[r, c].X = x+ (c) * dx;
                    grids[r, c].Y = y+ (r) * dy;
                    grids[r, c].Width = dx;
                    grids[r, c].Height = dy;
                }
        }
        public static void InitGrids(Rectangle[,] grids, int row, int col)
        {
            for (int r = 0; r < row; r++)
                for (int c = 0; c < col; c++)
                {
                    grids[r, c] = new Rectangle(0, 0, 0, 0);
                }
        }

        // 1D grids,
        public static Point GetGridCenter(Rectangle[] grids, int idx)
        {
            //int r = (idx / col);
            //int c = idx - (r * col);
            //Rectangle rc = grids[r, c];
            Rectangle rc = grids[idx];
            Point point = new Point(rc.X + rc.Width / 2, +rc.Y + rc.Height / 2);
            return point;
        }
        public static Point GetGridCenter(Rectangle[] grids, int r, int c, int row, int col)
        {
            int idx = r * col + c;
            return GetGridCenter(grids, idx);
        }
    }
}
