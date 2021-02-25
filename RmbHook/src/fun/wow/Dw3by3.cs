using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

// draw a grid for commands;

namespace KeyMouseDo
{
    class Dw3by3
    {
        Rectangle[] _rectwow = new Rectangle[10];

        int _gridw = 50;
        int _gridh = 50;
        static int _gridrow = 3;
        static int _gridcol = 3;
        static int _gridnum = _gridrow * _gridcol;
        int[] mwid = new int[_gridcol];
        int[] mhig = new int[_gridrow];
        int mwidth = 0;
        int mhight = 0;

        Pen[] mpens = new Pen[_gridnum];

        public Dw3by3()
        {
            mwid[0] = 150;
            mwid[1] = 150;
            mwid[2] = 150;
            
             
            mhig[0] = 150;
            mhig[1] = 150;
            mhig[2] = 150;

            for (int i=0;i<3;i++)
            {
                mwid[i] = _gridw;
                mhig[i] = _gridh;
                mwidth += mwid[i];
                mhight += mhig[i];
            }
            for (int i=0; i<_gridnum; i++)
            {
                mpens[i] = Pens.Black;
            }
        }

        public void setColor(int idx)
        {
            if (mpens[idx] == Pens.Black)
            {
                mpens[idx] = Pens.DarkRed;
            }
            else
            {
                mpens[idx] = Pens.Black;
            }
        }


        public int getGridIndex(int x, int y)
        {
            //x -= _point.X;
            //y -= _point.Y;
            for (int i = 0; i < _gridnum; i++)
            {
                if (_rectwow[i].Contains(x, y))
                    return i;
            }
            return -1;
        }

        public void setRects(int cx, int cy)
        {
            // 3x3 grids;
            //int left = cx - Convert.ToInt32((_gridcol * _gridw) / 2);
            //int top = cy - Convert.ToInt32((_gridrow * _gridh) / 2);
            int left = cx - Convert.ToInt32(mwidth / 2);
            int top = cy - Convert.ToInt32(mhight/ 2);


            // 
            int idx = 0;
            int x0 = left;// - Convert.ToInt32(_gridw*1.5);
            int y0 = cy+ Convert.ToInt32(_gridh * 0.5);

            int x = 0;
            int y = y0;

            for (int i = 0; i < _gridrow; i++)
            {
                x = x0;
                for (int j = 0; j < _gridcol; j++)
                {
                    _rectwow[idx].X = x;
                    _rectwow[idx].Y = y;
                    _rectwow[idx].Width = mwid[j];// _gridw;
                    _rectwow[idx].Height = mhig[i];// _gridh;

                    idx++;
                    x += mwid[j];// _gridw;
                }

                y += mhig[i];// _gridh;
            }
        }
        

        public void drawGraph(Graphics grap)
        {
            for (int i = 0; i < _gridnum; i++)
            {
                //grap.DrawEllipse(Pens.Black, _rectwow[i]);
                grap.DrawRectangle(mpens[i], _rectwow[i]);
            }
        }
    }
}
