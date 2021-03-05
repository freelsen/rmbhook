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
namespace WrittingHelper.wow
{
    class WowRogueOne
    {
        public Action<int> doAction;
        //public WowCmd mwowcmd = null;
        //public D2c md2c = null;
        WowRoguePixelOne mpixelone = new WowRoguePixelOne();
        RogueAction maction = new RogueAction();

        
        int mhealcnt = 0;
        int mslicecnt = 0;

        void DoAction(EactionRogue ea)
        {
            this.doAction((int)ea);
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
            if (mpixelone.isSameAll(WowRoguePixelOne.Status.target_title))// isTarget())
            {
                //DbMsg.Msg("target=true");
                if (mpixelone.isSame(WowRoguePixelOne.Status.target_health))// isTargetHealth((float)0.0))
                {
                    //DbMsg.Msg("dead");
                    //return;
                    if (!mpixelone.isSame(WowRoguePixelOne.Status.outmeleerange))// isOutofRange())
                    {
                        if (mpixelone.isSame(WowRoguePixelOne.Status.rogue5p))// isPoint5())
                        {
                            DbMsg.Msg("point5=true");
                            isevi = true;
                        }
                        if (!mpixelone.isSame(WowRoguePixelOne.Status.target_health, (float)0.4))// isTargetHealth((float)0.4))// blood <30%;
                        {

                            DbMsg.Msg("target blood < 0.4");
                            isevi = true;
                        }
                        else if (mpixelone.isSame(WowRoguePixelOne.Status.target_health, (float)0.6))
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
                        if (!mpixelone.isSameAll(WowRoguePixelOne.Status.auto))
                        {
                            //KeyHelper.SentKeyMof(Keys.Control, Keys.D9);
                            DoAction(EactionRogue.auto);
                        }

                        if (mpixelone.isSame(WowRoguePixelOne.Status.rogue1p))
                        {
                            if (isevi)
                            {
                                DoAction(EactionRogue.eviscerate);
                                issin = false;
                            }
                            else if (isslice && mpixelone.isSameAll(WowRoguePixelOne.Status.player_energy,(float)0.3))
                            {
                                DoAction(EactionRogue.slice);
                                issin = false;
                                mslicecnt = 15;
                            }
                        }

                        if (issin)
                        {
                            if (!mpixelone.isSame(WowRoguePixelOne.Status.target_health, (float)0.2))
                            {
                                DbMsg.Msg("target-health<0.2");
                                //KeyHelper.SentKeyMof(Keys.Control, Keys.D2);
                                DoAction(EactionRogue.sinister);
                            }
                            else
                            {
                                //showColor(Status.player_energy, (float)0.5);
                                if (mpixelone.isSameAll(WowRoguePixelOne.Status.player_energy, (float)0.5))      // >60;
                                {
                                    DbMsg.Msg("energy>0.5");
                                    //KeyHelper.SentKeyMof(Keys.Control, Keys.D4);        // back;
                                    //KeyHelper.SentKeyMof(Keys.Control, Keys.D2);
                                    DoAction(EactionRogue.sinister);
                                }
                            }
                        }
                        //showColor(Status.slice);

                    }
                    mpixelone.resetLoot();
                }
                else
                {
                    if (mpixelone.isLoot())
                    {
                        mpixelone.doLoot();
                    }
                    else
                        mpixelone.resetLoot();
                }
            }
            else    // non target;
            {
                //bool iseat = false;
                if (mhealcnt == 0)
                {
                    if (mpixelone.isSame(WowRoguePixelOne.Status.phead))
                    {
                        if (!mpixelone.isSame(WowRoguePixelOne.Status.php0, (float)0.5) && !mpixelone.isSame(WowRoguePixelOne.Status.php10)) // isPHealth((float)0.5))
                        {
                            //KeyHelper.SentKeyMof(Keys.Control, Keys.D0);
                            DoAction(EactionRogue.heal);
                            mhealcnt = 30;
                            
                        }
                    }
                }
                else if (mhealcnt>0)
                {
                    //DbMsg.Msg("meatcnt=" + mhealcnt.ToString());
                    if (mhealcnt > 3)
                        if (mpixelone.isSame(WowRoguePixelOne.Status.php10))
                        {
                            mhealcnt = 3;
                        }

                    if (mhealcnt == 22)
                    {
                        //KeyHelper.SentKeyMof(Keys.None, Keys.C);//hide;
                        DoAction(EactionRogue.hide);
                    }
                    else if (mhealcnt == 3)
                    {
                        // stand up;
                        //KeyHelper.SentKeyMof(Keys.None, Keys.Space);
                        DoAction(EactionRogue.jump);
                    }
                    else if (mhealcnt == 1)
                    {
                        
                        if (mpixelone.isSame(WowRoguePixelOne.Status.php0, (float)0.8) && mpixelone.isSameAll(WowRoguePixelOne.Status.roguehide))// isPHealth((float)0.8) && isHide())
                        {
                            //KeyHelper.SentKeyMof(Keys.None, Keys.C);
                            DoAction(EactionRogue.hide);
                        }
                    } 
                }

            }
            
        }

        public void ShowPos()
        {
            //if (misshowpos)
            {

                Color color;
                for (int i = 0; i < mpixelone.mdatanum; i++)
                {
                    int j = 0;
                    //for (int j = 0; j < 5; j++)
                    {
                        if ((mpixelone.mpositions[i, j].X == 0) && (mpixelone.mpositions[i, j].X == 0))
                            continue;
                        //    break;

                        Point pt = mpixelone.mpositions[i, j];
                        //mdwgraph.drawRect(grap, pt);

                        color = mpixelone.getColor(pt);
                        DbMsg.Msg(i.ToString() + "pos (" + pt.X.ToString() + "," + pt.Y.ToString() + "), color " +
                            "(" + color.R.ToString() + "," + color.G.ToString() + "," + color.B.ToString() + ")");
                    }
                }
            }
        }

    }
}
