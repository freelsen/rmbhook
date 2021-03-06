using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper.wow
{
    class RogueTwo
    {
        public Func<int, int> getVal;
        public Action<int> doAction;

        //public WowCmd mwowcmd = null;
        //public D2c md2c = null;
        //WowRogueAction maction = new WowRogueAction();

        int mhealcnt = 0;
        int mslicecnt = 0;

        int GetVal(EgridRogue we)
        {
            return this.getVal((int)we);
        }
        void DoAction(EactionRogue idx)
        {
            this.doAction((int)idx);
        }

        // rogue macro;
        int _php;
        int _phpm;
        int _xpos;
        int _ypos;

        public void DoMacro()
        {
            if (mhealcnt > 0) mhealcnt--;
            if (mslicecnt > 0) mslicecnt--;
            //if (mhealcnt > 0 || mslicecnt > 0)
            //    DbMsg.Msg("meatcnt=" + mhealcnt.ToString() + ",slicent=" + mslicecnt.ToString());

            _php = GetVal(EgridRogue.phpcur);
            _phpm = GetVal(EgridRogue.phpmax);

            _xpos = GetVal(EgridRogue.xpos);
            _ypos = GetVal(EgridRogue.ypos);

#if DEBUG
            //DbMsg.Msg("pos=(" + _xpos.ToString() + "," + _ypos.ToString());
#endif

            if (GetVal(EgridRogue.tname1) != 0)
            {
                Combat();
            }
            else
            {
                OutofCombat();
            }
        }

        void Combat()
        {
            bool isevi = false;
            bool issin = true;
            bool isslice = false;

            int thp = GetVal(EgridRogue.thpcur);
#if DEBUG
            Lslog.log("thp=" + thp.ToString());
#endif

            if (thp > 0)
            {
                int pt = GetVal(EgridRogue.ppoint);
#if DEBUG
                Lslog.log("pt=" + pt.ToString() + "," + mslicecnt.ToString());
#endif

                if ((GetVal(EgridRogue.aslice) == 0))
                {
                    if ((thp > 70) && (pt < 3))
                    {
                        Lslog.log("slice=true");
                        isslice = true;
                    }
                }

                if (thp < 40)
                    isevi = true;
                if (pt >= 5)
                    isevi = true;

                if (GetVal(EgridRogue.aauto) == 0)
                    DoAction(EactionRogue.auto);

                if (pt > 0)
                {
                    if (isslice)
                    {
                        DoAction(EactionRogue.slice);
                        Lslog.log("slice");
                        //mslicecnt = 15;
                        issin = false;
                    }
                    else if (isevi)
                    {
                        DoAction(EactionRogue.eviscerate);
                        issin = false;
                    }
                }

                if (issin)
                {
                    DoAction(EactionRogue.sinister);
                }
            }
        }


        void OutofCombat()
        {
            if (GetVal(EgridRogue.pcombat) == 0)
            {
                if (_phpm <= 0) _phpm = 1;
                float p = (float)_php / (float)_phpm;
                if (p < 0.5 && mhealcnt == 0)
                {
                    DoAction(EactionRogue.heal);
                    mhealcnt = 30;
                }
                if (mhealcnt > 0)
                    DuringkHeal(p);
            }
        }


        void DuringkHeal(float p)
        {
            if (mhealcnt > 0)
            {
                //DbMsg.Msg("meatcnt=" + mhealcnt.ToString());
                if (mhealcnt > 3)
                    if (p > 0.9)
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

                    if (p > 0.8 && (GetVal(EgridRogue.asteal) == 1))
                    {
                        //KeyHelper.SentKeyMof(Keys.None, Keys.C);
                        DoAction(EactionRogue.hide);
                    }
                }
            }
        }
    }
}
