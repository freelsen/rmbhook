using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Collections;

namespace RmbHook
{
    public class KeyCmd
    {
        protected MapKey mapkey = new MapKey();
        public virtual int init()
        {
            mapkey.init();

            return 0;
        }
        public virtual int onKey(Keys key)
        {
            int eatkey = 0;
            Keys mkey = Keys.A;           
            if (mapkey.getMapKey(key, ref mkey))
            {
                sendKey(mkey);
                eatkey = 1;
            } 

            return eatkey;
        }
        protected void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
    }    
    public class MapKey
    {
        public Hashtable mkeymap = new Hashtable();
        public int init()
        {
            mkeymap.Add(Keys.I, Keys.Up);
            mkeymap.Add(Keys.K, Keys.Down);
            mkeymap.Add(Keys.J, Keys.Left);
            mkeymap.Add(Keys.L, Keys.Right);
            mkeymap.Add(Keys.U, Keys.Home);
            mkeymap.Add(Keys.O, Keys.End);

            return 0;
        }
        public bool getMapKey( Keys key, ref Keys mkey)
        {

            bool contain = false;
            if (mkeymap.ContainsKey(key))
            {
                mkey = (Keys)mkeymap[key];
                contain = true;
            }

            return contain;
        }
    }

    // --- +repeat key; ---
    public class RepeatKey
    {
        private int mkeynum = 1;
        public int getKeyNum() { return mkeynum;  }

        private Hashtable mkeynumtable = new Hashtable();
        public void init()
        {
            mkeynumtable.Add(Keys.F, (int)1);
            mkeynumtable.Add(Keys.D, (int)2);
            mkeynumtable.Add(Keys.S, (int)5);
            mkeynumtable.Add(Keys.A, (int)10);
        }
        public int setKeyNum(Keys key)
        {
            if (mkeynumtable.Contains(key))
            {
                mkeynum = (int)mkeynumtable[key];
                return mkeynum;
            }
            else
                return -1;
        }
    }
    public class KeyCmdRepeat : KeyCmd
    {
        protected RepeatKey mrepeatkey = new RepeatKey();
        public override int init()
        {
            base.init();

            mrepeatkey.init();

            return 0;
        }
        public override int onKey(Keys key)
        {
            int eatkey = 0;

            Keys mkey = Keys.A;
            if (mapkey.getMapKey(key, ref mkey))
            {
                int num = mrepeatkey.getKeyNum();
                sendKeyNum(mkey, num);
                eatkey = 1;
            }
            else if (mrepeatkey.setKeyNum(key) > 0)
            {
                eatkey = 1;
            }
            return eatkey;
        }
        protected void sendKeyNum(Keys key, int num)
        {
            for (int i = 0; i < num; i++)
                sendKey(key);
        }
    }

    // ---+separate key; ---
    public class KeyCmdSeparateRepeat : KeyCmdRepeat
    {
        protected SeparateRepeatKey mseparatekey = new SeparateRepeatKey();
        public override int init()
        {
            base.init();
            mseparatekey.init();

            return 0;
        }
        public void onStart()
        {
            mseparatekey.reset();
        }
        
        public override int onKey(Keys key)
        {
            int eatkey = 0;

            mseparatekey.onKeyNumsStart();

            Keys mkey = Keys.A;
            int num = 0;
            if (mapkey.getMapKey(key, ref mkey))
            {
                num = mseparatekey.checkUpdateKeyNums(mkey);
                sendKeyNum(mkey, num);
                eatkey = 1;
            }
            else if ( (num = mrepeatkey.setKeyNum(key)) > 0)
            {
                mseparatekey.onKeyNums(num);
                eatkey = 1;
            }

            return eatkey;
        }
    }
    public class SeparateRepeatKey
    {
        private Hashtable mkeynums = new Hashtable();
        private int mkeynumtemp = 1;
        private int mkeynumscnt = 0;
        public void init()
        {
            mkeynums.Add(Keys.Up, (int)1);
            mkeynums.Add(Keys.Down, (int)1);
            mkeynums.Add(Keys.Left, (int)1);
            mkeynums.Add(Keys.Right, (int)1);
        }
        public void reset()
        {
            mkeynums[Keys.Up] = 1;
            mkeynums[Keys.Down] = 1;
            mkeynums[Keys.Left] = 1;
            mkeynums[Keys.Right] = 1;

            mkeynumscnt = 0;
        }
        
        public void onKeyNums( int num)
        {
            mkeynumtemp = num;
            mkeynumscnt = 2;
        }
        public void onKeyNumsStart()
        {
            if (mkeynumscnt > 0)
                mkeynumscnt--;
        }
        public int checkUpdateKeyNums(Keys key)
        {
            if (!mkeynums.Contains(key))
            {
                return 1;
            }
            if (mkeynumscnt > 0)
            {
                mkeynums[key] = mkeynumtemp;
                return mkeynumtemp;
            }
            else
                return 1;
        }
    }

    // --- pair repeat key ---
    public class KeyCmdPair : KeyCmdSeparateRepeat
    {
        protected PairRepeatKey mpairkey = new PairRepeatKey();
        public override int init()
        {
            base.init();
            mpairkey.init();

            return 0;
        }
        public void onStart()
        {
            base.onStart();
        }

        public override int onKey(Keys key)
        {
            int eatkey = 0;

            mseparatekey.onKeyNumsStart();

            Keys mkey = Keys.A;
            int num = 0;
            if (mapkey.getMapKey(key, ref mkey))
            {
                if (mpairkey.getPair(mkey, ref key))
                {
                    mseparatekey.checkUpdateKeyNums(key);
                }
                num = mseparatekey.checkUpdateKeyNums(mkey);
                sendKeyNum(mkey, num);
                eatkey = 1;
            }
            else if ((num = mrepeatkey.setKeyNum(key)) > 0)
            {
                mseparatekey.onKeyNums(num);
                eatkey = 1;
            }

            return eatkey;
        }
    }
    public class PairRepeatKey
    {
        private Hashtable mkeynumspair = new Hashtable();
        public int init()
        {
            mkeynumspair.Add(Keys.Up, Keys.Down);
            mkeynumspair.Add(Keys.Down, Keys.Up);
            mkeynumspair.Add(Keys.Left, Keys.Right);
            mkeynumspair.Add(Keys.Right, Keys.Left);
            return 0;
        }
        public bool getPair(Keys key, ref Keys pkey)
        {
            if (mkeynumspair.Contains(key))
            {
                pkey = (Keys)mkeynumspair[key];

                return true;
            }
            else
                return false;
        }
    }
}
