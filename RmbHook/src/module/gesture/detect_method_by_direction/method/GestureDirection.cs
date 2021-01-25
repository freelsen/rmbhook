using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

// 2021-01-22; Sheng Li, Calgary,CA.
// This class define the division of a circle (360 degree);
// Each direction has a gesture related.

namespace KeyMouseDo
{
    class GestureDirection
    {
        double[] mcos = new double[10];
        int[] marea = new int[10];
        public string[] mdirect = new string[10];

        public GestureDirection()
        {
            InitDirection();
        }

        void InitDirection()
        {
            mcos[0] = 0.9239;
            mcos[1] = 0.3827;
            mcos[2] = -0.3827;
            mcos[3] = -0.9239;
            mcos[4] = -1;

            marea[0] = 0;
            marea[1] = 1;
            marea[2] = 2;
            marea[3] = 3;
            marea[4] = 4;
            marea[5] = 4;
            marea[6] = 5;
            marea[7] = 6;
            marea[8] = 7;
            marea[9] = 0;

            mdirect[0] = "右";
            mdirect[1] = "右上";
            mdirect[2] = "上";
            mdirect[3] = "左上";
            mdirect[4] = "左";
            //mdirect[5] = "左";
            mdirect[5] = "左下";
            mdirect[6] = "下";
            mdirect[7] = "右下";
            //mdirect[9] = "右";
        }

        public int CalAreaIndex(Point p0, Point p1)
        {
            //pbm> the distance is in pixel unit.
            // should be in metric unit;

            double dx = GestureCommon.cDx(p1, p0);
            double dy = GestureCommon.cDy(p1, p0);
            double dis = GestureCommon.calDistance(p1, p0);
            double cos = dx / dis;

            //Console.WriteLine("(dx,dy,dis,cos)=" + dx.ToString() + "," + dy.ToString()
            //    + "," + dis.ToString("{.00}") + "," + cos.ToString("{0.0000}"));

            return ckArea(cos, dy >= 0);
        }


        public int ckArea(double cos, bool up)
        {
            int a = 0;

            if (up)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cos >= mcos[i])
                    {
                        a = marea[i];
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cos >= mcos[i])
                    {
                        a = marea[9 - i];
                        break;
                    }
                }
            }

            return a;
        }
    }
}
