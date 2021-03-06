﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WrittingHelper
{
    class GestureCommon
    {
        public static double calDistance(int x1,int y1, int x2,int y2)
        {
            int dx = x1-x2;
            int dy = y1-y2;

            double f = dx * dx + dy * dy;//Math.Pow(dx) + Math.Pow(dy);
            f = Math.Sqrt(f);

            return f;
        }
        public static double calDistance(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;

            double f = dx * dx + dy * dy;//Math.Pow(dx) + Math.Pow(dy);
            f = Math.Sqrt(f);

            return f;
        }
        public static int cDis2(Point a, Point b)                  // 四边形逼近；
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y + b.Y);
        }
        public static int cDx(Point a, Point b)
        {
            return a.X - b.X;
        }

        public static int cDy(Point a, Point b)
        {
            return -(a.Y - b.Y);        // 140740; y轴方向转换；
        }
        public static double cTan(Point a, Point b)
        {
            double dy = cDy(a, b);
            double dx = cDx(a, b);
            if (dx == 0)
            {
                if (dy > 0)
                    return 9999;
                else if (dy < 0)
                    return -9999;
                else
                    return 0;
            }
            else
            {
                return (dy / dx);
            }
        }
    }
}
