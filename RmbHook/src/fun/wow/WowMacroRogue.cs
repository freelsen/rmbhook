using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MouseKeyboardLibrary;
using System.Windows.Forms;
using System.Threading;

// 2021-02-08,
// f> to make the game automatic, by process key, mouse event,
namespace KeyMouseDo
{
    class WowMacroRogue
    {
        //public WowCmd mwowcmd = null;

        public WowMacroRogue()
        {
        }

        // rogue macro;
        public void doMacro()
        {
            bool isevi = false;
            bool issin = true;

            if (isTarget())
            {
                //DbMsg.Msg("target=true");
                if (!isTargetHealth((float)0.0))
                {
                    DbMsg.Msg("dead");
                    return;
                }

                if (isOutofRange())
                {
                    return;
                }

                if (isPoint5())
                {
                    DbMsg.Msg("point5=true");
                    isevi = true;
                }
                if (!isTargetHealth((float)0.4))// blood <30%;
                {
                    
                    DbMsg.Msg("blood < 0.4");
                    isevi = true;
                }

                // do 
                if (isevi)
                {
                    KeyHelper.SentKeyMof(Keys.Control, Keys.D3);
                }
                else if (issin)
                {
                    KeyHelper.SentKeyMof(Keys.Control, Keys.D2);
                }
            }
            else
            {
                //bool iseat = false;
                if (meatcnt == 0)
                {
                    if (!isphealth((float)0.5))
                    {
                        //iseat = true;
                        //}
                        //if (meatcnt == 0)
                        //{
                            // eat;
                            KeyHelper.SentKeyMof(Keys.Control, Keys.D0);
                            meatcnt = 30;
                        //}
                    } 
                }
                else if (meatcnt>0)
                {
                    if (meatcnt == 22)
                    {
                        KeyHelper.SentKeyMof(Keys.None, Keys.C);//hide;
                    }
                    else if (meatcnt == 3)
                    {
                        // stand up;
                        KeyHelper.SentKeyMof(Keys.None, Keys.Space);
                    }
                    else if (meatcnt == 1)
                    {
                        if (isphealth((float)0.8) && isHide())
                        {
                            KeyHelper.SentKeyMof(Keys.None, Keys.C);
                        }
                    }
                }
            }
            if (meatcnt > 0) meatcnt--;
        }



        // player,
        public Point mphpos = new Point(93, 76);
        public Point mpengerpos = new Point(93, 86);
        Color mpengercolor = Color.FromArgb(204, 203, 0);

        // target health;
        public Point mtpos = new Point(265, 60);
        Color mtcolor = Color.FromArgb(208, 205, 0); // neutral;
        Color mt2color = Color.FromArgb(208, 0, 0);  // enmegy;

        public Point mthpos = new Point(264, 76);
        Color mthealth = Color.FromArgb(0, 150, 0);

        //Point mthposr = new Point(377, 75);
        public int mthlen = 116;// mthposr.X - mthpos.X;
        Color getTHealth(float percent)
        {
            return getColorPercent(mthpos.X, mthpos.Y, percent, mthlen);
        }
        //Color mtcolor2 = new Color();
        bool isTargetHealth(float p)
        {
            Color color = getTHealth(p);
            //mtcolor2 = color;
            //DbMsg.Msg("target health color=" + 
            //    color.R.ToString() +
            //            "," + color.G.ToString() +
            //            "," + color.B.ToString());
            return isColorSame(color, mthealth);
        }

        bool isTarget()
        {

            Color color = getColor(mtpos.X, mtpos.Y);
            bool b = isColorSame(color, mt2color);
            if (b == false)
                return isColorSame(color, mtcolor);
            else
                return b;
        }

        // rogue point,
        public Point mroguep1pos = new Point(440, 42);
        Color mroguep1color = Color.FromArgb(154, 58, 56);
        public Point mroguep5pos = new Point(452, 83);
        Color mroguep5color = Color.FromArgb(154, 58, 56);

        public Point mroguehidepos = new Point(300, 773);
        Color mroguehidecolor = Color.FromArgb(255, 196, 141);

        

        bool isHide()
        {
            return isColorSame(mroguehidepos, mroguehidecolor);
        }

        int meatcnt = 0;
        bool isphealth(float p)
        {
            Color color = getColorPercent(mphpos.X, mphpos.Y,
                p, mthlen);
            //DbMsg.Msg("play health color="+
            //            color.R.ToString() +
            //            "," + color.G.ToString() +
            //            "," + color.B.ToString()); 
            return isColorSame(color, mthealth);
        }

        bool isEngerg(float p)
        {
            Color color = getColorPercent(mpengerpos.X, mpengerpos.Y,
                p, mthlen);
            return isColorSame(color, mpengercolor);
        }
        bool isPoint5()
        {
            //Color color = getColor(mroguep5pos.X, mroguep5pos.Y);
            //DbMsg.Msg(color.R.ToString() +
            //            "," + color.G.ToString() +
            //            "," + color.B.ToString());
            return isColorSame(mroguep5pos, mroguep5color);
        }
        bool isPoint1()
        {
            return isColorSame(mroguep1pos, mroguep1color);
        }

        // 
        public Point mc2pos = new Point(931, 809);
        Color mc2color = Color.FromArgb(255, 26, 26);

        bool isOutofRange()
        {
            //Color color = getColor(mc2pos.X, mc2pos.Y);
            //DbMsg.Msg(color.R.ToString() +
            //            "," + color.G.ToString() +
            //            "," + color.B.ToString());
            return isColorSame(mc2pos, mc2color);
        }

        // auto food



        // judge by color;
        int mcolordiff = 50;
        Color getColor(int x, int y)
        {
            Color c = FetchColor.getColor(DfTarget.mthis.mhwnd,
                x, y);
            return c;
        }
        Color getColorPercent(int x, int y, float xp, int xlen)
        {
            return getColor(Convert.ToInt32(x + xlen * xp), y);
        }
        bool isColorSame(Color color, Color cref)
        {
            int d = Math.Abs(color.R - cref.R) +
                Math.Abs(color.G - cref.G) + Math.Abs(color.B - cref.B);
            return (d < mcolordiff) ? true : false;
        }
        bool isColorSame(Point pt, Color cref)
        {
            Color color = getColor(pt.X, pt.Y);
            return isColorSame(color, cref);
        }
    }
}
