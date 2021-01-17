﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using RmbHook.src.keyword;

namespace RmbHook
{
    public class RmbKey
    {
        public static RmbKey gthis = null; 

        private TaskbarNotify mtasknotify = null;
        private CommandMode mcommandmode = new CommandMode();


        public RmbKey()
        {
            gthis = this;
        }

        private bool menWindow = false; // 2020-04-24;
        private bool mensearch = false;

        public int init()
        {
            int d = 0;
            Parameter prm = Factor.gm.mparameter;
            if ((d = setTopkey(prm.getTopkey())) < 0)
            {
                System.Console.WriteLine("->RmbKey.init(): setTopKey failed.");
            }
            if (prm.mconfigfile.readInt("enable_by_count") >= 0)
            {
                d = prm.mconfigfile.getInt();
                mHookmEnByKey = (d > 0) ? true : false;
            }

            if (prm.mconfigfile.readInt("enable_function_window") >= 0)
            {
                d = prm.mconfigfile.getInt();
                menWindow = (d > 0) ? true : false;
            }
            if (prm.mconfigfile.readInt("enable_function_search") >= 0)
            {
                d = prm.mconfigfile.getInt();
                mensearch = (d > 0) ? true : false;
            }

            initModeIcon();

            mtasknotify = TaskbarNotify.gthis;

            mcommandmode.init();

            setHookMode(true);

            return 1;
        }

        // ---key msg entry here;---
        public int onKeymsg(KeyEventArgs e)
        {
            Keys key = e.KeyCode;

            setEatKey(0);

            if (key == getTopkey())// 20150621, Keys.Escape)
            {
                onTopkPressedHm();

                if (getHookMode())
                {
                    setEatKey(1);

                    // normally, when cmd mode is on, the top key will be eated;
                    // but here give a chance: when you tap esc key twince, the last will be send to app(not eat);
                    // this is a must have function. or your apps will not receive esc key msg;
                    incEatCntCm();

                    setCmdMode(!getCmdMode());
                }
            }
            else
            {
                onNtopkPressedHm();

                resetEatCntCm();

                if (getCmdMode())
                {
                    int d = onCmdKey(key);
                    setEatKey(d);
                } 
            }

            return getEatKey();
        }

        public void onHookModeChangedHm()
        {
            if (getHookMode())
            {
                resetEatCntCm();
            }

            updateModeIcon();
        }
        public void onCmdModeChanged()
        {
            updateModeIcon();

            if (getCmdMode())
            {
                mcommandmode.onStart();
            }
        }

        // ---on cmd keys; ---
        public int onCmdKey(Keys key)
        {
            int eatkey = 0;

            // check function group-1,
            switch (key)
            {
                case Keys.G:
                    setCmdMode(false);
                    eatkey = 1;
                    break;
            }

            // window operation,
            if (eatkey == 0 && menWindow)
            {
                switch (key)
                {
                    case Keys.B:            // 20150621;
                        WinMon.mthis.stBottom();
                        setCmdMode(false);
                        eatkey = 1;
                        break;
                    case Keys.T:
                        WinMon.mthis.restore();
                        setCmdMode(false);
                        eatkey = 1;
                        break;
                    case Keys.Y:
                        WinMon.mthis.minNow();
                        setCmdMode(false);
                        eatkey = 1;
                        break;
                }
            }

            // search dialog,
            if (eatkey == 0 && mensearch)
            {
                switch (key)
                {
                    case Keys.P:            // 20160503, keyword search;
                        setCmdMode(false);
                        eatkey = 1;
                        LsKeyword kw = LsKeyword.getThis();
                        kw.onSearchKey(true);
                        break;
                    case Keys.N:
                        setCmdMode(false);
                        eatkey = 1;
                        LsKeyword kw2 = LsKeyword.getThis();
                        kw2.onSearchKey(false);
                        break;
                }
            }

            // moving control,
            if (0 == eatkey)
            {
                eatkey = mcommandmode.onKey(key);
            }

            // exit cmd mode if non-function chars pressed,2020-04-16,
            if (0 == eatkey)
            {
                UInt16 d = (UInt16)key;
                if (d >= 0x41 && d <= 0x5A)
                {
                    eatkey = 0;
                    setCmdMode(false);
                }
            }

            return eatkey;
        }

        // --- eat key control --- 
        private int mEatKey = 0;
        private void setEatKey(int d) { mEatKey = d; }
        public int getEatKey() { return mEatKey; }

        private int mEatCntCm = 0;           // determine if to eat topkey or not in cmd mode;
        private void resetEatCntCm()
        {
            mEatCntCm = 0;
        }
        private void incEatCntCm()
        {
            if (mEatCntCm >= 2)
                mEatCntCm = 0;
            mEatCntCm++;
            //Console.WriteLine(mEatCntCm.ToString());

            if (mEatCntCm >= 2)
            {
                onEatCmChanged();
            }
        }
        public void onEatCmChanged()
        {
            setEatKey(0);
        }

        // --- top key---;
        Keys mtopkey = Keys.Escape;             // topkey is the key to trigger different modes,
        public Keys getTopkey() { return mtopkey; }
        public int setTopkey(string str)        // 2015-06-21;
        {
            if (str.Length == 0) return -1;

            //
            if (str.Equals("esc")) { mtopkey = Keys.Escape; return 0; }
            if (str.Equals("caps")) { mtopkey = Keys.CapsLock; return 0; }
            if (str.Equals("tab")) { mtopkey = Keys.Tab; return 0; }

            int d = 0;
            try
            {
                //Keys.Oemtilde;//192;~
                d = Int32.Parse(str);
                mtopkey = (Keys)d;
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("setTopkey: parse error.");
            }

            return 0;
        }
        
        // ---hook mode---
        public void onTopkPressedHm()
        {
            if (mHookmEnByKey)              // 2015-06-21;
            {
                incTopKeyCntAll();
            }
        }
        public void onNtopkPressedHm()
        {
            resetTopkCntAll();
        }
        public void onHookModeChanged()
        {
            resetTopkCntAll();

            onHookModeChangedHm();
        }
        private void onTopkCntAllChanged()
        {
            setHookMode(!getHookMode()); // change hood mode,
        }

        public bool getHookMode() { return mHookMode; }
        public void setHookMode(bool b)
        {
            if ( (b==true  && mHookMode == false) ||
                 (b==false && mHookMode == true ) )
            {
                mHookMode = !mHookMode;                

                onHookModeChanged();
            }
        }
        private bool mHookMode = false;   
        private bool mHookmEnByKey = false;   // enable/disable by keep pressing the topkey,
        public bool getHookmEnByKey() { return mHookmEnByKey; }

        private int mTopKeyCntAll = 0;       // count topkey in any state,     
        public void resetTopkCntAll()
        {
            mTopKeyCntAll = 0;
        }
        private void incTopKeyCntAll()
        {
            // In any status( enable or disable ), if you tap the topkey 
            // for x(now x=5) times, the status will change;
            // thus give a keyboard way to enable/disable key hook function;
            // count number of pressing the topkey,
            mTopKeyCntAll++;
            if (mTopKeyCntAll >= 5)
            {
                mTopKeyCntAll = 0;

                onTopkCntAllChanged();
            }
        }

        // ---command mode; ---
        public bool getCmdMode() { return mCmdMode; }
        public void setCmdMode(bool b) 
        { 
            mCmdMode = b;

            onCmdModeChanged();
        }        
        private bool mCmdMode = false;          // cmd model,
        
        // ---indicator icon ---
        public void updateModeIcon()
        {
            if (getHookMode())
            {
                if( getCmdMode() )
                    mtasknotify.setIcon(micon3);    // cmd mode on;
                else
                    mtasknotify.setIcon(micon2);    // cmd monde off;
            }
            else
                mtasknotify.setIcon(null);          // default;
        }
        private Icon micon2;
        private Icon micon3;
        private int initModeIcon()
        {
            micon2 = new Icon("res\\icon2.ico");
            micon3 = new Icon("res\\icon3.ico");

            return 0;
        }

    }
}
