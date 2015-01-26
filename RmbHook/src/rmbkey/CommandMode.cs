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

            initKeyNum();                     // +repeat keys;

            return 0;
        }
        public int onKey(Keys key)
        {
            int eatkey = 0;
            if (mkeymap.ContainsKey(key))
            {
                Keys kmap = (Keys)mkeymap[key];
                sendKeyEx(kmap);
                eatkey = 1;
            }
            else                                // +repeat keys;
            {
                eatkey = onKeyNum(key);
            }
            return eatkey;
        }
        private void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
        private void sendKeyEx(Keys key)
        {
            sendKeyNum(key);
        }

        // --send repeat keys;---
        private void sendKeyNum(Keys key)
        {
            for (int i = 0; i < mkeynum; i++)
                sendKey(key);
        }
        private int mkeynum = 1;
        private Hashtable mkeynumtable = new Hashtable();
        private void initKeyNum()
        {
            mkeynumtable.Add(Keys.F, (int)1);
            mkeynumtable.Add(Keys.D, (int)2);
            mkeynumtable.Add(Keys.S, (int)5);
            mkeynumtable.Add(Keys.A, (int)10);
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
    }
}
