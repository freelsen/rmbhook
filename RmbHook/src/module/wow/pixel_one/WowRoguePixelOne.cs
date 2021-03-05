using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WrittingHelper.libs;

namespace WrittingHelper.wow
{
    class WowRoguePixelOne
    {
        public Func<IntPtr> getHwndWow;
        public Func<IntPtr> getHwndDraw;

        public WowRoguePixelOne()
        {
            initData();
        }
        public int mdatanum = 100;// 15;
        public enum Status : int
        {
            phead,
            php0,   // health;
            php10,
            player_energy,  // energy;

            target_title,
            target_health,
            target_health10,
            target_energy,

            rogue1p,
            rogue3p,
            rogue5p,

            loot_win,
            loot1,
            loot2,
            loot3,
            loot4,


            outmeleerange = 51,
            roguehide = 52,
            auto = 53,
            slice = 54
        }


        static int mdlen = 100;
        public Point[,] mpositions = new Point[mdlen, 5];
        public Color[,] mcolors = new Color[mdlen, 5];
        //public List<Color>[] mcolorlist = new List<Color>[100];
        Color mcolornull = Color.FromArgb(0, 0, 0);
        public int mcolordiff = 50;

        public int mthlen = 100;

        public Point[,] mpositions0 = new Point[mdlen, 5];
        //public Color[,] mcolor0 = new Color[mdlen, 5];
        public int mhigh0 = 768;
        public int mhigh = 768;
        public int mwid = 1024;

        public int mthlen0;
        int mbasehigh0 = 98;
        int mgridhigh0 = 42;
        int mctlhigh0 = 30;
        int mctlwid0 = 32;

        public void changeSize(int high, int wid)
        {
            double scale = (double)high / (double)mhigh0;
            mhigh = high;
            mwid = wid;

            mthlen = Convert.ToInt32(mthlen0 * scale);
            //int basehigh = Convert.ToInt32(mbasehigh0 * scale);
            //int gridhigh = Convert.ToInt32(mgridhigh0 * scale);
            //int ctlhigh = Convert.ToInt32(mctlhigh0 * scale);
            //int ctlwid = Convert.ToInt32(mctlwid0 * scale);

            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mpositions[i, j].X = Convert.ToInt32(mpositions0[i, j].X * scale);
                    mpositions[i, j].Y = Convert.ToInt32(mpositions0[i, j].Y * scale);
                }

            }

            //int idx = 0;
            int x = 0; int y = 0;
            for (int i = 50; i < mdlen; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    x = Convert.ToInt32(mpositions0[i, j].X * scale);
                    y = Convert.ToInt32(mpositions0[i, j].Y * scale);
                    mpositions[i, j].X = mwid - x;
                    mpositions[i, j].Y = mhigh - y;
                }

                //mpositions[i].X = wid - ctlwid;
                //mpositions[i].Y = high - basehigh - gridhigh*idx -ctlhigh;

            }

            //idx = 0;
            //int wid1 = wid - gridhigh;
            //for (int i = 70; i < 82; i++)
            //{
            //    mpositions[i].X = wid1 - ctlwid;
            //    mpositions[i].Y = high - basehigh - gridhigh * idx - ctlhigh;
            //    idx = idx + 1;
            //}
        }


        void initData()
        {
            for (int i = 0; i < mdlen; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mcolors[i, j] = Color.FromArgb(0, 0, 0);

                    mpositions[i, j] = new Point(0, 0);
                    mpositions0[i, j] = new Point(0, 0);
                }
            }

            int idx;
            int d = 0;
            /*
            // 800x600;
            mthlen = 85;
            
            idx = (int)Status.php0;             mpositions[idx] = new Point(73, 39);        mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            d = Convert.ToInt32(mthlen * 1.0);
            idx = (int)Status.php10;            mpositions[idx] = new Point(73+d, 39); mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            idx = (int)Status.player_energy;    mpositions[idx] = new Point(73, 48);        mcolors[idx, 0] = Color.FromArgb(150, 150, 0);

            idx = (int)Status.target_title;     mpositions[idx] = new Point(204, 28);       mcolors[idx, 0] = Color.FromArgb(100, 100, 0); mcolors[idx, 1] = Color.FromArgb(200, 0, 0); mcolors[idx, 2] = Color.FromArgb(200, 200, 0);
            idx = (int)Status.target_health;    mpositions[idx] = new Point(202, 39);       mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            idx = (int)Status.target_energy;    mpositions[idx] = new Point(202, 48);       mcolors[idx, 0] = Color.FromArgb(150,150, 0);

            idx = (int)Status.rogue1p;          mpositions[idx] = new Point(337, 13);        mcolors[idx, 0] = Color.FromArgb(134, 18, 19);
            idx = (int)Status.rogue3p;          mpositions[idx] = new Point(340, 30);       mcolors[idx, 0] = Color.FromArgb(134, 18, 19);
            idx = (int)Status.rogue5p;          mpositions[idx] = new Point(247, 43);        mcolors[idx, 0] = Color.FromArgb(154, 58, 56);
            idx = (int)Status.roguehide;        mpositions[idx] = new Point(38, 503);        mcolors[idx, 0] = Color.FromArgb(255, 215, 160);
            idx = (int)Status.ctl2;             mpositions[idx] = new Point(523, 531);      mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            */

            /* 1600x900;
            mthlen = 117;
            int px = 98;    int py = 53;
            int py2 = 64;

            int tx = 273;
            int tx1 = 275;  int ty1 = 36;

            idx = (int)Status.phead;            mpositions[idx] = new Point(90, 28);        mcolors[idx, 0] = Color.FromArgb(115, 119, 115);
            idx = (int)Status.php0;             mpositions[idx] = new Point(px, py);        mcolors[idx, 0] = Color.FromArgb(0, 160, 0);
            d = Convert.ToInt32(mthlen * 1.0);
            idx = (int)Status.php10;            mpositions[idx] = new Point(px + d, py);    mcolors[idx, 0] = Color.FromArgb(0, 160, 0);
            idx = (int)Status.player_energy;    mpositions[idx] = new Point(px, py2);       mcolors[idx, 0] = Color.FromArgb(160, 160, 0); mcolors[idx, 1] = Color.FromArgb(200, 200, 0);

            idx = (int)Status.target_title;     mpositions[idx] = new Point(tx1, ty1);      mcolors[idx, 0] = Color.FromArgb(100, 100, 0); mcolors[idx, 1] = Color.FromArgb(200, 0, 0); mcolors[idx, 2] = Color.FromArgb(200, 200, 0);
            idx = (int)Status.target_health;    mpositions[idx] = new Point(tx, py);        mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            idx = (int)Status.target_energy;    mpositions[idx] = new Point(tx, py2);       mcolors[idx, 0] = Color.FromArgb(150, 150, 0);

            idx = (int)Status.rogue1p;          mpositions[idx] = new Point(456, 18);       mcolors[idx, 0] = Color.FromArgb(210, 61, 62);
            idx = (int)Status.rogue5p;          mpositions[idx] = new Point(469, 60);       mcolors[idx, 0] = Color.FromArgb(174, 58, 56);
            // monitor;
            //idx = (int)Status.roguehide; mpositions[idx] = new Point(311, 770); mcolors[idx, 0] = Color.FromArgb(255, 215, 160);
            //idx = (int)Status.ctl2; mpositions[idx] = new Point(968, 808); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            // t460s;
            idx = (int)Status.roguehide;        mpositions[idx] = new Point(311, 770);      mcolors[idx, 0] = Color.FromArgb(255, 200, 150);// 215, 160);
            //idx = (int)Status.ctl2;             mpositions[idx] = new Point(968, 808-20); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            idx = (int)Status.ctl2;             mpositions[idx] = new Point(1539-2, 311+10);     mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            //idx = (int)Status.auto;             mpositions[idx] = new Point(315, 784);      mcolors[idx, 0] = Color.FromArgb(173, 171, 173);
            idx = (int)Status.auto;             mpositions[idx] = new Point(1511, 611+25);     mcolors[idx, 0] = Color.FromArgb(30,30,30);// 152, 164, 194);
            idx = (int)Status.slice;            mpositions[idx] = new Point(1388, 35);      mcolors[idx, 0] = Color.FromArgb(175, 128, 4); mcolors[idx, 1] = Color.FromArgb(110,90,10); mcolors[idx, 2] = Color.FromArgb(86, 69, 8);
            */

            // set for 1024x768; default scale (no scale);
            mthlen0 = 111;
            int px = 93; int py = 50;
            int py2 = 61;

            int tx = 258;
            int tx1 = 260; int ty1 = 35;

            idx = (int)Status.phead; mpositions0[idx, 0] = new Point(85, 29); mcolors[idx, 0] = Color.FromArgb(120, 120, 120);
            idx = (int)Status.php0; mpositions0[idx, 0] = new Point(px, py); mcolors[idx, 0] = Color.FromArgb(0, 160, 0);
            d = Convert.ToInt32(mthlen0 * 1.0);
            idx = (int)Status.php10; mpositions0[idx, 0] = new Point(px + d, py); mcolors[idx, 0] = Color.FromArgb(0, 180, 0);
            idx = (int)Status.player_energy; mpositions0[idx, 0] = new Point(px, py2); mcolors[idx, 0] = Color.FromArgb(160, 160, 0); mcolors[idx, 1] = Color.FromArgb(200, 200, 0);

            idx = (int)Status.target_title; mpositions0[idx, 0] = new Point(tx1, ty1); mcolors[idx, 0] = Color.FromArgb(100, 100, 0); mcolors[idx, 1] = Color.FromArgb(200, 0, 0); mcolors[idx, 2] = Color.FromArgb(200, 200, 0);
            idx = (int)Status.target_health; mpositions0[idx, 0] = new Point(tx, py); mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            d = Convert.ToInt32(mthlen0 * 1.0);
            idx = (int)Status.target_health10; mpositions0[idx, 0] = new Point(tx + d, py); mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
            idx = (int)Status.target_energy; mpositions0[idx, 0] = new Point(tx, py2); mcolors[idx, 0] = Color.FromArgb(150, 150, 0);

            idx = (int)Status.rogue1p; mpositions0[idx, 0] = new Point(433, 18); mcolors[idx, 0] = Color.FromArgb(170, 61, 62);
            idx = (int)Status.rogue5p; mpositions0[idx, 0] = new Point(445, 59); mcolors[idx, 0] = Color.FromArgb(174, 58, 56);

            idx = (int)Status.loot_win; mpositions0[idx, 0] = new Point(69, 122); mcolors[idx, 0] = Color.FromArgb(140, 140, 140);
            idx = (int)Status.loot1; mpositions0[idx, 0] = new Point(39, 210); mcolors[idx, 0] = Color.FromArgb(20, 20, 20);
            idx = (int)Status.loot2; mpositions0[idx, 0] = new Point(39, 250); mcolors[idx, 0] = Color.FromArgb(20, 20, 20);
            idx = (int)Status.loot3; mpositions0[idx, 0] = new Point(39, 290); mcolors[idx, 0] = Color.FromArgb(20, 20, 20);
            idx = (int)Status.loot4; mpositions0[idx, 0] = new Point(39, 330); mcolors[idx, 0] = Color.FromArgb(20, 20, 20);
            // monitor;
            //idx = (int)Status.roguehide; mpositions[idx] = new Point(311, 770); mcolors[idx, 0] = Color.FromArgb(255, 215, 160);
            //idx = (int)Status.ctl2; mpositions[idx] = new Point(968, 808); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            // t460s;
            int hig = mhigh0;
            int wid = mwid;
            int basehigh = 98;
            int gridhigh = 42;
            int ctlhigh = 30;
            int ctlwid = 32;
            //int exphigh = 8;
            //idx = (int)Status.ctl2;             mpositions[idx] = new Point(968, 808-20); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            idx = (int)Status.outmeleerange; mpositions0[idx, 0] = new Point(gridhigh + 26, basehigh + gridhigh * 10 + 30); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
            mpositions0[idx, 1] = new Point(gridhigh + 26, basehigh + gridhigh * 10 + 31);
            //idx = (int)Status.auto;             mpositions[idx] = new Point(315, 784);      mcolors[idx, 0] = Color.FromArgb(173, 171, 173);
            idx = (int)Status.auto; mpositions0[idx, 0] = new Point(gridhigh + 42, basehigh + gridhigh * 3 + 20); mcolors[idx, 0] = Color.FromArgb(216, 197, 103);// mcolors[idx, 1] = Color.FromArgb(0, 0, 0);// 152, 164, 194);
            idx = (int)Status.roguehide; mpositions0[idx, 0] = new Point(gridhigh + 42, basehigh + gridhigh * 4 + 20); mcolors[idx, 0] = Color.FromArgb(222, 201, 104); //mcolors[idx, 1] = Color.FromArgb(0, 0, 0);// 215, 160);
            idx = (int)Status.slice; mpositions0[idx, 0] = new Point(gridhigh, basehigh); mcolors[idx, 0] = Color.FromArgb(175, 128, 4); mcolors[idx, 1] = Color.FromArgb(110, 90, 10); mcolors[idx, 2] = Color.FromArgb(86, 69, 8);


            changeSize(mhigh0, mwid);
        }

        

        public bool isLoot()
        {
            bool b = isSame(Status.loot_win);

            return b;
        }
        public void resetLoot()
        {
            mlootidx = 0;
        }
        int mlootidx = 0;
        public void doLoot()
        {
            //int idx = (int)Status.loot1+mlootidx;

            DbMsg.Msg("do loot");
            //Point pt = mpositions[idx, mlootidx];
            mlootidx++;
            if (mlootidx >= 4)
                mlootidx = 0;

            //remember current position;
            Point curpos = new Point(0, 0);
            WinApis.GetCursorPos(ref curpos);

            for (int i = 0; i < 4; i++)
            {
                int idx = (int)Status.loot1 + i;// mlootidx;
                //pt = mwowmacro.getLootPos();
                //DbMsg.Msg("pos=" + pt.X.ToString() + "," + pt.Y.ToString());
                //if (j>0)
                {
                    Point pt = new Point(0, 0);// = mpositions[idx, 1];
                    //pt.X = mpositions[idx, 0].X;
                    //pt.Y = mpositions[idx, 0].Y;
                    pt = mpositions[idx, 0];
                    WinApis.ClientToScreen(this.getHwndDraw(),ref pt); // DrawFormMan._this.mformhwnd, ref pt);

                    MouseHelper.Click(1, pt.X, pt.Y);
                    DbMsg.Msg("mouse-click=" + pt.X.ToString() + "," + pt.Y.ToString());
                }
                //Thread.Sleep(10);
            }
            //MouseHelper.setPos(curpos);


            //return pt;
        }


        public bool isanyColor(Color color, int idx)
        {
            Color color2;

            bool b = false;
            for (int i = 0; i < 5; i++)
            {
                color2 = mcolors[idx, i];
                if (Color.Equals(color2, mcolornull))
                    break;
                else
                {
                    b = isColorSame(color, color2);
                    if (b)
                        break;
                }
            }
            return b;
        }
        public bool isSameAll(Status index)
        {
            return isSameAll(index, (float)0.0);
        }
        public bool isSameAll(Status index, float p)
        {
            int idx = (int)index;

            bool same = false;
            for (int j = 0; j < 5; j++)
            {
                if ((mpositions[idx, j].X == 0) && (mpositions[idx, j].X == 0))
                    break;

                int x = mpositions[idx, j].X + Convert.ToInt32(p * mthlen);
                Color color = getColor(x, mpositions[idx, j].Y);

                if (isanyColor(color, idx))
                {
                    same = true;
                    break;
                }
            }
            return same;
        }
        public bool isSame(Status index)
        {
            return isSame(index, (float)0.0);
        }
        void showColor(Status index)
        {
            showColor(index, (float)0.0);
        }
        public void showColor(Status index, float p)
        {
            int idx = (int)index;
            int x = mpositions[idx, 0].X + Convert.ToInt32(p * mthlen);
            Color color = getColor(x, mpositions[idx, 0].Y);
            DbMsg.Msg("color=" +
                        color.R.ToString() +
                        "," + color.G.ToString() +
                        "," + color.B.ToString());
        }
        public bool isSame(Status index, float p)
        {

            int idx = (int)index;

            bool same = false;
            for (int j = 0; j < 5; j++)
            {
                if ((mpositions[idx, j].X == 0) && (mpositions[idx, j].X == 0))
                    continue;

                int x = mpositions[idx, j].X + Convert.ToInt32(p * mthlen);
                Color color = getColor(x, mpositions[idx, j].Y);
                //DbMsg.Msg("play health color="+
                //            color.R.ToString() +
                //            "," + color.G.ToString() +
                //            "," + color.B.ToString()); 
                if (isColorSame(color, mcolors[idx, 0]))
                {
                    same = true;
                    break;
                }
            }
            return same;
        }

        // judge by color;

        public Color getColor(Point pt)
        {
            return getColor(pt.X, pt.Y);
        }
        public Color getColor(int x, int y)
        {
            Color c = FetchColor.getColorClient((int)this.getHwndWow(),x,y);// WowWin.mthis.mhwnd, x, y);
            return c;
        }
        Color getColorPercent(int x, int y, float xp, int xlen)
        {
            return getColor(Convert.ToInt32(x + xlen * xp), y);
        }
        public bool isColorSame(Color color, Color cref)
        {
            int d = Math.Abs(color.R - cref.R) +
                Math.Abs(color.G - cref.G) + Math.Abs(color.B - cref.B);
            return (d < mcolordiff) ? true : false;
        }
        public bool isColorSame(Point pt, Color cref)
        {
            Color color = getColor(pt.X, pt.Y);
            return isColorSame(color, cref);
        }


    }
}
