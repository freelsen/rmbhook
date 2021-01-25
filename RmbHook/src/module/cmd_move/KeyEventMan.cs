using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 2021-01-23 Sheng Li. AB.CA.
// process all the events related to the keyboard.

namespace KeyMouseDo
{
    class KeyEventMan
    {
        public static KeyEventMan mthis = null;

        KeyMode mRmbKey = new KeyMode();
        WinMon mWinMon = new WinMon();
        KeyParameter mKeyPrm = new KeyParameter();
        KeyCommandNonmove mKeyCmdNmv = new KeyCommandNonmove();

        public KeyEventMan()
        {
        }
        public int init()
        {
            mKeyPrm.mKeyCmdNmv = mKeyCmdNmv;
            mKeyPrm.mRmbKey = mRmbKey;
            mKeyPrm.init();

            mKeyCmdNmv.init();

            mRmbKey.mKeyCmdNmv = mKeyCmdNmv;
            mRmbKey.init();

            mWinMon.init();

            return 0;
        }

        //-----------------------------------------------------
        public bool onKeyDown(object sender, KeyEventArgs e)
        {
            int d = mRmbKey.onKeyMsg(e);
            return (d>0)?true:false;
        }
        public void onKeyUp(object sender, KeyEventArgs e)
        {
        }
        public void onKeyPress(object sender, KeyEventArgs e)
        {
        }

        public void onKeyMessage(object sender, KeyEventArgs e)
        {
        }
    }
}
