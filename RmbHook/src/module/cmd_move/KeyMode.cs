using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using RmbHook.src.keyword;
using System.Globalization;
using System.Threading;

namespace RmbHook
{
    class KeyMode
    {
        public static KeyMode gthis = null; 

        public TaskbarNotify mtasknotify = null;
        public KeyCommandNonmove mKeyCmdNmv = null;


        KeyCommandMove mcommandmode = new KeyCommandMove();

        // topkey is the key to trigger different modes,
        public Keys mtopkey = Keys.Escape;             

        //public bool getHookMode() { return mHookMode; }
        // enable/disable by keep pressing the topkey,
        public bool mHookmEnByKey = false;
        public bool mHookMode = false;

        bool mCmdMode = false;          // cmd model,


        public KeyMode()
        {
            gthis = this;
        }

        public int init()
        {
            initModeIcon();

            mtasknotify = TaskbarNotify.gthis;

            mcommandmode.init();

            startKeyMode();

            return 1;
        }

        // ---key msg entry here;---

        int mEatKey = 0;
        Keys mk = Keys.None;

        public void startKeyMode()
        {
            mHookMode = true;
            mCmdMode = false;

            mEatCntCm = 0;
            mTopKeyCntAll = 0;

            updateModeIcon();
        }
        public void stopKeyMode()
        {
            mHookMode = false;

            updateModeIcon();
        }

        public int mcmd = 0;    // 2021-01-23;

        public int onKeyMsg(KeyEventArgs e)
        {
            mk = e.KeyCode;

            //Console.WriteLine(e.ToString());
            //Thread.Sleep(100);

            mEatKey = 0;
            mcmd = 0;

            if (isHookModeChange())
            {
                // hook mode changed.
                mHookMode=!mHookMode;

                mEatCntCm = 0;
                mTopKeyCntAll = 0;

                updateModeIcon();
            }

            if (!mHookMode)
                return mEatKey;

            //
            if (isCmdModeChange())
            {
                mCmdMode = !mCmdMode;

                updateModeIcon();

                if (mCmdMode)
                {
                    mcommandmode.onStart();
                }
            }
            if (!mCmdMode)
                return mEatKey;

            // 
            if ((mk!=mtopkey))
            {
               onCmdKey(mk);
            }

            //Console.WriteLine(mEatKey.ToString());
            return mEatKey;
        }

        // --- eat key control --- 
        // determine if to eat topkey or not in cmd mode;
        int mEatCntCm = 0;

        bool isCmdModeChange()
        {
            bool ischange = false;
            if ((mk == mtopkey)) // 20150621, Keys.Escape)
            {
                mEatKey = 1;

                // normally, when cmd mode is on, the top key will be eated;
                // but here give a chance: when you tap esc key twince, the last will be send to app(not eat);
                // this is a must have function. or your apps will not receive esc key msg;
                //incEatCntCm();
                if (++mEatCntCm >= 2)
                {
                    mEatCntCm = 0;
                    mEatKey = 0;
                }

                ischange = true;
            }
            else
            {
                mEatCntCm = 0;
            }

            return ischange;
        }

        int mTopKeyCntAll = 0;       // count topkey in any state,     
        bool isHookModeChange()
        {
            return false;
            bool ismodechange = false;

            if (mHookmEnByKey && (mk != mtopkey))              // 2015-06-21;
            {
                // press the top-key n times continuously.
                // will change the hood mode;
                if (++mTopKeyCntAll >= 5)
                {
                    mTopKeyCntAll = 0;

                    ismodechange= true;
                }
            }
            else
            {
                mTopKeyCntAll = 0;
            }
            return ismodechange;
        }

        void stopCmdMode()
        {
            mCmdMode = false;

            updateModeIcon();
        }
        

        // ---on cmd keys; ---
        public bool onCmdKey(Keys key)
        {
            //int mEatKey = 0;

            bool iskeepcmd = mKeyCmdNmv.doNonMovingCmd(key);
            if (iskeepcmd == false)
            {
                mEatKey = 1;
                stopCmdMode();

                return iskeepcmd;
            }

            // moving control,
            if (0 == mEatKey)
            {
                mEatKey = mcommandmode.onKey(key);
            }

            // exit cmd mode if non-function chars pressed,2020-04-16,
            //if (0 == mEatKey)
            //{
            //    UInt16 d = (UInt16)key;
            //    if (d >= 0x41 && d <= 0x5A)
            //    {
            //        mEatKey = 0;
            //        iskeepcmd = false; //setCmdMode(false);
            //    }
            //}

            return iskeepcmd; //;
        }




        // ---indicator icon ---
        public void updateModeIcon()
        {
            if (mHookMode)
            {
                if( mCmdMode )
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
