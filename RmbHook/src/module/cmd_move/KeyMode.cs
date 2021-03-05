using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using WrittingHelper.src.keyword;
using System.Globalization;
using System.Threading;

namespace WrittingHelper
{
    class KeyCommandMode
    {
        public static KeyCommandMode gthis = null; 

        public TaskbarNotify mtasknotify = null;
        public KeyCommandNonmove mKeyCmdNmv = null;


        KeyCommandMove mcommandmode = new KeyCommandMove();

        // topkey is the key to trigger different modes,
        public Keys mtopkey = Keys.Escape;             

        //public bool getHookMode() { return mHookMode; }
        // enable/disable by keep pressing the topkey,
        public bool misEnableByKey = false;
        // 2021-01-26, what is hook mode actuall? the name is confusing.
        //  -it is actually the key-mode; (--> rename);
        //  -then what's cmd mode? that's when it start to receive cmd.
        //  -'cmd mode' mean's the keys are parsed as 'cmd' instead of chars;
        public bool misEnable = false;
        bool misCmdMode = false;          // cmd model,


        public KeyCommandMode()
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
            misEnable = true;
            misCmdMode = false;

            mEatCntCm = 0;
            mTopKeyCntAll = 0;

            updateModeIcon();
        }
        public void stopKeyMode()
        {
            misEnable = false;

            updateModeIcon();
        }

        //public int mcmd = 0;    // 2021-01-23;
        bool misstateChanged = false;   // 2021-01-26;

        public int onKeyMsg(KeyEventArgs e)
        {
            mk = e.KeyCode;

            //Console.WriteLine(e.ToString());
            //Thread.Sleep(100);

            mEatKey = 0;
            //mcmd = 0;
            misstateChanged = false;

            if (isEnableChanged())
            {
                // hook mode changed.
                misEnable=!misEnable;

                mEatCntCm = 0;
                mTopKeyCntAll = 0;

                misstateChanged = true;
                //updateModeIcon();
            }

            if (misEnable)
            {
                //
                if (isCmdModeChange())
                {
                    misCmdMode = !misCmdMode;

                    misstateChanged = true; //updateModeIcon();

                    if (misCmdMode)
                    {
                        mcommandmode.onStart();
                    }
                }
                else if (misCmdMode)
                {
                    // 
                    if (!onCmdKey(mk))
                    {
                        misCmdMode = false;

                        misstateChanged = true; //updateModeIcon();
                    }
                }
            }

            if (misstateChanged)
            {
                updateModeIcon();
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
        bool isEnableChanged()
        {
            return false;
            bool ismodechange = false;

            if (misEnableByKey && (mk != mtopkey))              // 2015-06-21;
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
            misCmdMode = false;

            updateModeIcon();
        }
        

        // ---on cmd keys; ---
        public bool onCmdKey(Keys k)
        {
            //int mEatKey = 0;
            //if (mk == mtopkey)
            //    return false;

            bool iskeepcmdmode = mKeyCmdNmv.doNonMovingCmd(k);
            if (iskeepcmdmode == false)
            {
                mEatKey = 1;
                //stopCmdMode();

                return iskeepcmdmode;
            }

            // moving control,
            if (0 == mEatKey)
            {
                mEatKey = mcommandmode.onKey(k);
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

            return iskeepcmdmode; //;
        }




        // ---indicator icon ---
        public void updateModeIcon()
        {
            if (misEnable)
            {
                if( misCmdMode )
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
