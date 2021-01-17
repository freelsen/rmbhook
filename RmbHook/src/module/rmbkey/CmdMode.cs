using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Collections;

namespace RmbHook
{
    public class CmdMode
    {
        public int init()
        {
            initMapKey();
            initRepeat();
            initSeparate();
            initPair();
            return 0;
        }
        public int reset()
        {
            resetSeparate();
            return 0;
        }
        public int onKey(Keys key)
        {
            int eatkey = 0;

            onKeySeparate();

            Keys mapkey = Keys.A;
            if (getMapkey(key, ref mapkey))
            {
                onSeparateUpdate(mapkey);
                Keys pairkey = Keys.A;
                if (getPairKey(mapkey, ref pairkey))
                {
                    onSeparateUpdate(pairkey);
                }
                int num =  getSeparateNum(mapkey);
                sendKeyNum(mapkey, num);
            }
            else if (setRepeatNum(key))
            {
                int num = getRepeatNum();
                onSeparateStart(num);
                eatkey = 1;
            }

            return eatkey;
        }
        // ---+do command---
        void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
        void sendKeyNum(Keys key, int num)
        {
            for (int i = 0; i < num; i++)
                sendKey(key);
        }
        // ===+1> send map key===
        Hashtable mkeymap = new Hashtable();
        int initMapKey()
        {
            mkeymap.Add(Keys.I, Keys.Up);
            mkeymap.Add(Keys.K, Keys.Down);
            mkeymap.Add(Keys.J, Keys.Left);
            mkeymap.Add(Keys.L, Keys.Right);
            mkeymap.Add(Keys.U, Keys.Home);
            mkeymap.Add(Keys.O, Keys.End);

            return 0;
        }
        bool getMapkey(Keys key, ref Keys mkey)
        {
            if (mkeymap.ContainsKey(key))
            {
                mkey = (Keys)mkeymap[key];
                return true;
            }
            else
                return false;
        }
        
        // ===+2>send repeat keys;===
        int mkeynum = 1;
        int getRepeatNum() { return mkeynum; }
        Hashtable mkeynumtable = new Hashtable();
        void initRepeat()
        {
            mkeynumtable.Add(Keys.F, (int)1);
            mkeynumtable.Add(Keys.D, (int)2);
            mkeynumtable.Add(Keys.S, (int)5);
            mkeynumtable.Add(Keys.A, (int)10);
        }
        bool setRepeatNum(Keys key)
        {
            if (mkeynumtable.Contains(key))
            {
                mkeynum = (int)mkeynumtable[key];
                return true;
            }
            else
                return false;
        }

        // ===+3>separate repeat key===
        Hashtable mkeynums = new Hashtable();
        int mkeynumtemp = 1;
        int mkeynumscnt = 0;
        void initSeparate()
        {
            mkeynums.Add(Keys.Up, (int)1);
            mkeynums.Add(Keys.Down, (int)1);
            mkeynums.Add(Keys.Left, (int)1);
            mkeynums.Add(Keys.Right, (int)1);
        }
        void resetSeparate()
        {
            mkeynums[Keys.Up] = 1;
            mkeynums[Keys.Down] = 1;
            mkeynums[Keys.Left] = 1;
            mkeynums[Keys.Right] = 1;

            mkeynumscnt = 0;
        }
        void onKeySeparate()
        {
            if (mkeynumscnt > 0)
                mkeynumscnt--;
        }
        void onSeparateStart(int num)
        {
            mkeynumtemp = num;
            mkeynumscnt = 2;
        }
        void onSeparateUpdate(Keys key)
        {
            if (!isSeparate(key))
                return;

            if (mkeynumscnt > 0)
            {
                mkeynums[key] = mkeynumtemp;
            }
        }
        bool isSeparate(Keys key)
        {
            return mkeynums.Contains(key);
        }
        int getSeparateNum(Keys key)
        {
            if (!isSeparate(key))
                return 1;
            else
                return (int)mkeynums[key];
        }

        // ===+pair repeat keys===
        Hashtable mkeynumspair = new Hashtable();
        void initPair()
        {
            mkeynumspair.Add(Keys.Up, Keys.Down);
            mkeynumspair.Add(Keys.Down, Keys.Up);
            mkeynumspair.Add(Keys.Left, Keys.Right);
            mkeynumspair.Add(Keys.Right, Keys.Left);
        }
        bool getPairKey(Keys key, ref Keys pkey)
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
