using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MouseKeyboardLibrary;

namespace WrittingHelper
{
    public class KeyCommand
    {
        public int onKey(Keys key)
        {
            int eatkey = 0;

            mseparatechecker.onKey();
            int num = 1;
            Keys akey = Keys.A;
            if (mmapkey.check(key, ref akey))
            {
                if (mseparatechecker.check(ref num))
                    mseparatekey.setSeparateKeynum(akey, num);

                Keys pkey = Keys.A;
                if (mpairkey.getPair(akey, ref pkey))
                    mseparatekey.setSeparateKeynum(pkey, num);

                CmdAction.sendKeyNum(akey, num);
                eatkey = 1;
            }
            else if (mrepeatkey.getKeyNum(key, ref num))
            {
                mseparatechecker.onSeparateKey(num);
                eatkey = 1;
            }
            return eatkey;
        }

        Mapkey mmapkey = new Mapkey();
        Repeatkey mrepeatkey = new Repeatkey();
        Separatekey mseparatekey = new Separatekey();
        Pairkey mpairkey = new Pairkey();
        SeparateChecker mseparatechecker = new SeparateChecker();
        public int init()
        {
            mmapkey.init();
            mrepeatkey.init();
            mseparatekey.init();
            mpairkey.init();
            return 0;
        }
        public int reset()
        {
            mseparatekey.reset();
            mseparatechecker.reset();
            return 0;
        }
    }  
    public class CmdAction
    {
        public static void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
        public static void sendKeyNum(Keys key, int num)
        {
            for (int i = 0; i < num; i++)
                sendKey(key);
        }
    }
    public class Mapkey
    {
        public bool check(Keys key, ref Keys mkey)
        {
            bool contain = false;
            if (mkeymap.ContainsKey(key))
            {
                mkey = (Keys)mkeymap[key];
                contain = true;
            }
            return contain;
        }
        Hashtable mkeymap = new Hashtable();
        public void init()
        {
            mkeymap.Add(Keys.I, Keys.Up);
            mkeymap.Add(Keys.K, Keys.Down);
            mkeymap.Add(Keys.J, Keys.Left);
            mkeymap.Add(Keys.L, Keys.Right);
            mkeymap.Add(Keys.U, Keys.Home);
            mkeymap.Add(Keys.O, Keys.End);
        }
    }
    public class Repeatkey
    {
        public bool getKeyNum(Keys key, ref int num)
        {
            if (mkeynumtable.Contains(key))
            {
                num = (int)mkeynumtable[key];
                return true;
            }
            else
                return false;
        }
        Hashtable mkeynumtable = new Hashtable();
        public void init()
        {
            mkeynumtable.Add(Keys.F, (int)1);
            mkeynumtable.Add(Keys.D, (int)2);
            mkeynumtable.Add(Keys.S, (int)5);
            mkeynumtable.Add(Keys.A, (int)10);
        }
    }
    public class Separatekey
    {
        public bool setSeparateKeynum(Keys key, int num)
        {
            if (mkeynums.Contains(key))
            {
                mkeynums[key] = num;
                return true;
            }
            else
                return false;
        }
        Hashtable mkeynums = new Hashtable();
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
        }
    }
    public class SeparateChecker
    {
        int mkeycnt = 0;
        // true when 1 repeat key + 1 map key;
        public void reset() { mkeycnt = 0; }
        public void onKey()
        {
            if (mkeycnt > 0)
                mkeycnt--;
        }
        int mkeynum = 1;
        public void onSeparateKey(int num)
        {
            mkeynum = num;
            mkeycnt = 2;
        }
        public bool check(ref int num)
        {
            if (mkeycnt > 0)
            {
                num = mkeynum;
                return true;
            }
            else
                return false;
        }
    }
    public class Pairkey
    {
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
        private Hashtable mkeynumspair = new Hashtable();
        public void init()
        {
            mkeynumspair.Add(Keys.Up, Keys.Down);
            mkeynumspair.Add(Keys.Down, Keys.Up);
            mkeynumspair.Add(Keys.Left, Keys.Right);
            mkeynumspair.Add(Keys.Right, Keys.Left);
        }
    }
}
