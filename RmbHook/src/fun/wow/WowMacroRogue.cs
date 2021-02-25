using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MouseKeyboardLibrary;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

// 2021-02-08,
// f> to make the game automatic, by process key, mouse event,
namespace KeyMouseDo
{
    class WowMacroRogue
    {
        //public WowCmd mwowcmd = null;
        public D2c md2c = null;

        public WowMacroRogue()
        {
            initKeys();
            initData();
        }

        
        int mhealcnt = 0;

        // rogue macro;
        public void doMacro()
        {
            bool isevi = false;
            bool issin = true;
            bool isslice = false;

            if (mhealcnt > 0) mhealcnt--;
            if (mslicecnt > 0) mslicecnt--;
            //if (mhealcnt > 0 || mslicecnt > 0)
            //    DbMsg.Msg("meatcnt=" + mhealcnt.ToString() + ",slicent=" + mslicecnt.ToString());

            int php = md2c.getVal(Wenum.phpcur);
            int phpm = md2c.getVal(Wenum.phpmax);

            int xpos = md2c.getVal(Wenum.xpos);
            int ypos = md2c.getVal(Wenum.ypos);

#if DEBUG
            DbMsg.Msg("pos=(" + xpos.ToString() + "," + ypos.ToString());
#endif

            if (md2c.getVal(Wenum.tname1) != 0)
            {
                int thp = md2c.getVal(Wenum.thpcur);
 
                if (thp > 0)
                {
                    int pt = md2c.getVal(Wenum.ppoint);

                    if (thp < 50)
                        isevi = true;
                    if (pt >= 5)
                        isevi = true;

                    if (md2c.getVal(Wenum.aauto) == 0)
                        doAction(Eaction.auto);

                    if (isevi && pt > 0)
                    {
                        doAction(Eaction.eviscerate);
                    }
                    else if (issin)
                    {
                        doAction(Eaction.sinister);
                    }
                }
            }
            else
            {
                if(md2c.getVal(Wenum.pcombat)==0)
                {
                    if (phpm <= 0) phpm = 1;
                    float p = (float)php / (float)phpm;
                    if (p <0.5 && mhealcnt==0)
                    {
                        doAction(Eaction.heal);
                        mhealcnt = 30;
                    }
                    if (mhealcnt > 0)
                        duringkHeal(p);
                }
            }
        }
        void duringkHeal(float p)
        {
            if (mhealcnt > 0)
            {
                //DbMsg.Msg("meatcnt=" + mhealcnt.ToString());
                if (mhealcnt > 3)
                    if (p>0.9)
                    {
                        mhealcnt = 3;
                    }

                if (mhealcnt == 22)
                {
                    //KeyHelper.SentKeyMof(Keys.None, Keys.C);//hide;
                    doAction(Eaction.hide);
                }
                else if (mhealcnt == 3)
                {
                    // stand up;
                    //KeyHelper.SentKeyMof(Keys.None, Keys.Space);
                    doAction(Eaction.jump);
                }
                else if (mhealcnt == 1)
                {

                    if (p>0.8 && (md2c.getVal(Wenum.asteal)==1))
                    {
                        //KeyHelper.SentKeyMof(Keys.None, Keys.C);
                        doAction(Eaction.hide);
                    }
                }
            }
        }
        

        public void doMacro2()
        {
            bool isevi = false;
            bool issin = true;
            bool isslice = false;

            if (mhealcnt > 0) mhealcnt--;
            if (mslicecnt > 0) mslicecnt--;
            if (mhealcnt>0 || mslicecnt>0)
                DbMsg.Msg("meatcnt=" + mhealcnt.ToString() +",slicent="+mslicecnt.ToString());

            //if(md2c.getVal(Wenum.tname1)!=0)
            if (isSameAll(Status.target_title))// isTarget())
            {
                //DbMsg.Msg("target=true");
                if (isSame(Status.target_health))// isTargetHealth((float)0.0))
                {
                    //DbMsg.Msg("dead");
                    //return;
                    if (!isSame(Status.outmeleerange))// isOutofRange())
                    {
                        if (isSame(Status.rogue5p))// isPoint5())
                        {
                            DbMsg.Msg("point5=true");
                            isevi = true;
                        }
                        if (!isSame(Status.target_health, (float)0.4))// isTargetHealth((float)0.4))// blood <30%;
                        {

                            DbMsg.Msg("target blood < 0.4");
                            isevi = true;
                        }
                        else if (isSame(Status.target_health, (float)0.6))
                        {
                            //if (!isSameAll(Status.slice))
                            if (mslicecnt == 0 )
                            {
                                isslice = true;                                
                            }
                        }

                        //isslice = false;

                        // do 
                        mhealcnt = 0;    // reset;

                        // start auto attack;
                        if (!isSameAll(Status.auto))
                        {
                            //KeyHelper.SentKeyMof(Keys.Control, Keys.D9);
                            doAction(Eaction.auto);
                        }

                        if (isSame(Status.rogue1p))
                        {
                            if (isevi)
                            {
                                doAction(Eaction.eviscerate);
                                issin = false;
                            }
                            else if (isslice && isSameAll(Status.player_energy,(float)0.3))
                            {
                                doAction(Eaction.slice);
                                issin = false;
                                mslicecnt = 15;
                            }
                        }

                        if (issin)
                        {
                            if (!isSame(Status.target_health, (float)0.2))
                            {
                                DbMsg.Msg("target-health<0.2");
                                //KeyHelper.SentKeyMof(Keys.Control, Keys.D2);
                                doAction(Eaction.sinister);
                            }
                            else
                            {
                                //showColor(Status.player_energy, (float)0.5);
                                if (isSameAll(Status.player_energy, (float)0.5))      // >60;
                                {
                                    DbMsg.Msg("energy>0.5");
                                    //KeyHelper.SentKeyMof(Keys.Control, Keys.D4);        // back;
                                    //KeyHelper.SentKeyMof(Keys.Control, Keys.D2);
                                    doAction(Eaction.sinister);
                                }
                            }
                        }
                        //showColor(Status.slice);

                    }
                    resetLoot();
                }
                else
                {
                    if (isLoot())
                    {
                        doLoot();
                    }
                    else
                        resetLoot();
                }
            }
            else    // non target;
            {
                //bool iseat = false;
                if (mhealcnt == 0)
                {
                    if (isSame(Status.phead))
                    {
                        if (!isSame(Status.php0, (float)0.5) && !isSame(Status.php10)) // isPHealth((float)0.5))
                        {
                            //KeyHelper.SentKeyMof(Keys.Control, Keys.D0);
                            doAction(Eaction.heal);
                            mhealcnt = 30;
                            
                        }
                    }
                }
                else if (mhealcnt>0)
                {
                    //DbMsg.Msg("meatcnt=" + mhealcnt.ToString());
                    if (mhealcnt > 3)
                        if (isSame(Status.php10))
                        {
                            mhealcnt = 3;
                        }

                    if (mhealcnt == 22)
                    {
                        //KeyHelper.SentKeyMof(Keys.None, Keys.C);//hide;
                        doAction(Eaction.hide);
                    }
                    else if (mhealcnt == 3)
                    {
                        // stand up;
                        //KeyHelper.SentKeyMof(Keys.None, Keys.Space);
                        doAction(Eaction.jump);
                    }
                    else if (mhealcnt == 1)
                    {
                        
                        if (isSame(Status.php0, (float)0.8) && isSameAll(Status.roguehide))// isPHealth((float)0.8) && isHide())
                        {
                            //KeyHelper.SentKeyMof(Keys.None, Keys.C);
                            doAction(Eaction.hide);
                        }
                    } 
                }

            }
            
        }
        int mslicecnt = 0;



        void doAction(Eaction action)
        {
            int idx = (int)action;
            KeyHelper.SentKeyMof(mactionkeys[idx, 0], mactionkeys[idx, 1]);
        }
        public enum Eaction : int
        {
            sinister,
            eviscerate,
            backstab,
            slice,
            auto,
            hide,
            heal,
            jump
        }
        Keys[,] mactionkeys = new Keys[100, 3];
        void initKeys()
        {
            for (int i=0;i<100;i++)
            {
                mactionkeys[i, 0] = Keys.None;
                mactionkeys[i, 1] = Keys.None;
            }

            int idx;
            idx = (int)Eaction.jump;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
            idx = (int)Eaction.hide;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D8;
            idx = (int)Eaction.sinister;    mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D2;
            idx = (int)Eaction.eviscerate;  mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D3;
            idx = (int)Eaction.auto;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D9;
            idx = (int)Eaction.backstab;    mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D4;
            idx = (int)Eaction.slice;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D5;
            idx = (int)Eaction.heal;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D0;
            idx = (int)Eaction.jump;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
            idx = (int)Eaction.jump;        mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
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

 
            outmeleerange=51,
            roguehide=52,
            auto=53,
            slice=54
        }


        static int mdlen = 100;
        public Point[,] mpositions = new Point[mdlen,5];
        public Color[,] mcolors = new Color[mdlen, 5];
        //public List<Color>[] mcolorlist = new List<Color>[100];
        Color mcolornull = Color.FromArgb(0, 0, 0);
        public int mcolordiff = 50;

        public int mthlen = 100;

        public Point[,] mpositions0 = new Point[mdlen,5];
        //public Color[,] mcolor0 = new Color[mdlen, 5];
        public int mhigh0=768;
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

            for (int i=0;i<50;i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    mpositions[i,j].X = Convert.ToInt32(mpositions0[i,j].X * scale);
                    mpositions[i,j].Y = Convert.ToInt32(mpositions0[i,j].Y * scale);
                }
               
            }

            //int idx = 0;
            int x = 0; int y = 0;
            for (int i=50;i<mdlen;i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    x = Convert.ToInt32(mpositions0[i,j].X * scale);
                    y = Convert.ToInt32(mpositions0[i,j].Y * scale);
                    mpositions[i,j].X = mwid - x;
                    mpositions[i,j].Y = mhigh - y;
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
            idx = (int)Status.target_health10; mpositions0[idx, 0] = new Point(tx+d, py); mcolors[idx, 0] = Color.FromArgb(0, 150, 0);
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
            idx = (int)Status.outmeleerange;     mpositions0[idx, 0] = new Point(gridhigh+26,  basehigh+gridhigh * 10+30); mcolors[idx, 0] = Color.FromArgb(255, 26, 26);
                                                mpositions0[idx, 1] = new Point(gridhigh + 26, basehigh + gridhigh * 10 + 31); 
            //idx = (int)Status.auto;             mpositions[idx] = new Point(315, 784);      mcolors[idx, 0] = Color.FromArgb(173, 171, 173);
            idx = (int)Status.auto;     mpositions0[idx, 0] = new Point(gridhigh+42,  basehigh+gridhigh * 3+20); mcolors[idx, 0] = Color.FromArgb(216,197,103);// mcolors[idx, 1] = Color.FromArgb(0, 0, 0);// 152, 164, 194);
            idx = (int)Status.roguehide; mpositions0[idx, 0] = new Point(gridhigh+42, basehigh + gridhigh*4+20); mcolors[idx, 0] = Color.FromArgb(222, 201, 104); //mcolors[idx, 1] = Color.FromArgb(0, 0, 0);// 215, 160);
            idx = (int)Status.slice; mpositions0[idx, 0] = new Point(gridhigh, basehigh); mcolors[idx, 0] = Color.FromArgb(175, 128, 4); mcolors[idx, 1] = Color.FromArgb(110, 90, 10); mcolors[idx, 2] = Color.FromArgb(86, 69, 8);


            changeSize(mhigh0,mwid);
        }

       
        public bool isLoot()
        {
            bool b= isSame(Status.loot_win);

            return b;
        }
        void resetLoot()
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
                    WinApis.ClientToScreen(DrawFormMan._this.mhwnd, ref pt);

                    MouseHelper.Click(1, pt.X, pt.Y);
                    DbMsg.Msg("mouse-click=" + pt.X.ToString() + "," + pt.Y.ToString());
                }
                //Thread.Sleep(10);
            }
            //MouseHelper.setPos(curpos);


            //return pt;
        }


        bool isanyColor(Color color,int idx)
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
        bool isSameAll(Status index)
        {
            return isSameAll(index, (float)0.0);
        }
        bool isSameAll(Status index, float p)
        {
            int idx = (int)index;

            bool same = false;
            for (int j = 0; j < 5; j++)
            {
                if ((mpositions[idx, j].X == 0) && (mpositions[idx, j].X == 0))
                    break;

                int x = mpositions[idx,j ].X + Convert.ToInt32(p * mthlen);
                Color color = getColor(x, mpositions[idx,j].Y);
                
                if (isanyColor(color,idx))
                {
                    same = true;
                    break;
                }
            }
            return same;
        }
        bool isSame(Status index)
        {
            return isSame(index, (float)0.0);
        }
        void showColor(Status index)
        {
            showColor(index, (float)0.0);
        }
        void showColor(Status index, float p)
        {
            int idx = (int)index;
            int x = mpositions[idx,0].X + Convert.ToInt32(p * mthlen);
            Color color = getColor(x, mpositions[idx,0].Y);
            DbMsg.Msg("color=" +
                        color.R.ToString() +
                        "," + color.G.ToString() +
                        "," + color.B.ToString());
        }
        bool isSame(Status index, float p)
        {

            int idx = (int)index;

            bool same = false;
            for (int j = 0; j < 5; j++)
            {
                if ((mpositions[idx, j].X == 0) && (mpositions[idx, j].X == 0))
                    continue;

                int x = mpositions[idx,j].X + Convert.ToInt32(p * mthlen);
                Color color = getColor(x, mpositions[idx,j].Y);
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
            Color c = FetchColor.getColorClient(DfTarget.mthis.mhwnd, x, y);
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
