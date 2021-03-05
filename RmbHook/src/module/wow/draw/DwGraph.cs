using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WrittingHelper.wow
{
    class DwGraph
    {
        SolidBrush _myBrush = new SolidBrush(Color.Red);

        public Rectangle _prect = new Rectangle(0, 0, 100, 100);
        Point mcenter = new Point(0, 0);

        public void setRect(int cx, int cy)
        {
            mcenter.X = cx;mcenter.Y = cy;

            _prect.X = cx - _prect.Width / 2;
            _prect.Y = cy - _prect.Height / 2;
        }

        public Pen mpen = Pens.Red;
        public void changeColor()
        {
            if (mpen == Pens.Red)
                mpen = Pens.Black;
            else
                mpen = Pens.Red;
        }

        public void drawRect(Graphics grap)
        {
            //Rectangle rect = new Rectangle(_point.X, _point.Y, 50, 50);

            //e.Graphics.DrawRectangle(Pens.Red, rect);
            //SolidBrush myBrush = new SolidBrush(Color.Red);
            grap.FillRectangle(_myBrush, _prect);

        }
        
        public void drawCircle(Graphics grap)
        {
            //grap.DrawEllipse(Pens.Red, _prect);
            float astart = (float)22.5;
            float astep = 45;
            //float aend = (float)22.5+astep;
            
            for (int i = 0; i < 8; i++)
            {
                grap.DrawPie(Pens.Red, _prect, astart, astep);
                astart += astep;
                //aend += astep;
            }
        }

        public void drawCircle(Graphics grap, Point pt)
        {
            int r = 5;
            int a = pt.X - r;
            int c = pt.Y - r;

            Rectangle rect = new Rectangle(a,c,r,r);
            
            grap.DrawEllipse(Pens.Red, rect);
        }
        public void drawRect(Graphics grap, Point pt)
        {
            Rectangle rect = new Rectangle(pt.X - 1, pt.Y - 1, 2, 2);
            grap.DrawRectangle(mpen, rect);
        }
    }
}
