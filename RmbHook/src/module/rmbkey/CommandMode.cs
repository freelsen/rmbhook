using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Collections;

namespace RmbHook
{
    public class CommandMode
    {
        private Hashtable mkeymap = new Hashtable();

        public CommandMode()
        {
        }
        public int init()
        {
            mkeymap.Add(Keys.I, Keys.Up);
            mkeymap.Add(Keys.K, Keys.Down);
            mkeymap.Add(Keys.J, Keys.Left);
            mkeymap.Add(Keys.L, Keys.Right);
            mkeymap.Add(Keys.U, Keys.Home);
            mkeymap.Add(Keys.O, Keys.End);

            initKeyNum();                       

            initKeyNums();

            initKeyNumsPair();

            return 0;
        }

        public void onStart()
        {
            resetKeyNums();
        }
        public int onKey(Keys key)
        {
            onKeyNumsStart();            

            int eatkey = 0;
            if (mkeymap.ContainsKey(key))
            {
                Keys kmap = (Keys)mkeymap[key];
                sendKeyNumsPair(kmap);
                eatkey = 1;
            }
            else                                
            {
                eatkey = onKeyNums(key);
            }
            return eatkey;
        }
        private void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }

        // --send repeat keys;---
        private int mkeynum = 1;
        private Hashtable mkeynumtable = new Hashtable();
        private int getKeyNum() { return mkeynum; }

        private void initKeyNum()
        {
            mkeynumtable.Add(Keys.F, (int)1);
            mkeynumtable.Add(Keys.D, (int)2);
            mkeynumtable.Add(Keys.S, (int)5);
            mkeynumtable.Add(Keys.A, (int)10);
        }
        private void sendKeyNum(Keys key)
        {
            sendKeyNum(key, mkeynum);
        }
        private void sendKeyNum(Keys key, int num)
        {
            for (int i = 0; i < num; i++)
                sendKey(key);
        }
        public int onKeyNum(Keys key)
        {
            int eatkey = 0;
            if (mkeynumtable.Contains(key))
            {
                mkeynum = (int)mkeynumtable[key];           

                eatkey = 1;
            }
            return eatkey;
        }
        
        // ---+separate repeat keys; ---
        private Hashtable mkeynums = new Hashtable();
        private int mkeynumtemp = 1;
        private int mkeynumscnt = 0;
        private void initKeyNums()
        {
            mkeynums.Add(Keys.Up, (int)1);
            mkeynums.Add(Keys.Down, (int)1);
            mkeynums.Add(Keys.Left, (int)1);
            mkeynums.Add(Keys.Right, (int)1);
        }
        private void resetKeyNums()
        {
            mkeynums[Keys.Up] = 1;
            mkeynums[Keys.Down] = 1;
            mkeynums[Keys.Left] = 1;
            mkeynums[Keys.Right] = 1;

            mkeynumscnt = 0;
        }
        private void sendKeyNums(Keys key)
        {
            int num = 1;
            if (mkeynums.Contains(key))
            {
                checkUpdateKeyNums(key);

                num = (int)mkeynums[key];
            }
            sendKeyNum(key, num);
        }
        private int onKeyNums(Keys key)
        {
            int eatkey = onKeyNum(key);
            if (eatkey > 0)
            {
                mkeynumtemp = getKeyNum();
                mkeynumscnt = 2;
            }
            return eatkey;
        }
        private void onKeyNumsStart()
        {
            if (mkeynumscnt > 0)
                mkeynumscnt--;
        }
        private void checkUpdateKeyNums(Keys key)
        {
            if (mkeynumscnt > 0)
            {
                mkeynums[key] = mkeynumtemp;
            }
        }

        // ---pair repeat keys ---
        private Hashtable mkeynumspair = new Hashtable();
        private void initKeyNumsPair()
        {
            mkeynumspair.Add(Keys.Up, Keys.Down);
            mkeynumspair.Add(Keys.Down, Keys.Up);
            mkeynumspair.Add(Keys.Left, Keys.Right);
            mkeynumspair.Add(Keys.Right, Keys.Left);
        }
        private void sendKeyNumsPair(Keys key)
        {
            if (mkeynumspair.Contains(key))
            {
                Keys pair = (Keys)mkeynumspair[key];

                checkUpdateKeyNums(pair);
            }

            sendKeyNums(key);
        }
    }
}
