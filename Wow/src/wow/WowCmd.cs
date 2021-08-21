using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace WoW.wow
{
    class WowCmd
    {
        public static WowCmd mthis = null;
        public  WowCmd()
        {
            mthis = this;

            initCmdList();
        }
        public Keys[,] mkeylist1 = new Keys[10, 2];
        public  Keys[,] _keylist2 = new Keys[10, 2];

        void initCmdList()
        {
            mkeylist1[0, 0] = Keys.Alt; mkeylist1[0, 1] = Keys.D4;
            mkeylist1[1, 0] = Keys.Alt; mkeylist1[1, 1] = Keys.D5;
            mkeylist1[2, 0] = Keys.Alt; mkeylist1[2, 1] = Keys.D6;
            mkeylist1[3, 0] = Keys.Alt; mkeylist1[3, 1] = Keys.D7;
            mkeylist1[4, 0] = Keys.Alt; mkeylist1[4, 1] = Keys.D8;
            mkeylist1[5, 0] = Keys.Alt; mkeylist1[5, 1] = Keys.D9;
            mkeylist1[6, 0] = Keys.Alt; mkeylist1[6, 1] = Keys.D0;
            mkeylist1[7, 0] = Keys.Alt; mkeylist1[7, 1] = Keys.D1;
            mkeylist1[8, 0] = Keys.Alt; mkeylist1[8, 1] = Keys.D2;
            mkeylist1[9, 0] = Keys.Alt; mkeylist1[9, 1] = Keys.D3;
        }

        public void doCmd(int index)
        {
            //if (WowCmd.mthis == null) return;
            Keys[,] keylist = WowCmd.mthis.mkeylist1;
            KeyHelper.SentKeyMof(keylist[index, 0], keylist[index, 1]);
        }

        public void doMap()
        {
            KeyHelper.SentKeyMof(Keys.None, Keys.M);
        }
        public void doAutoRun()
        {
            KeyHelper.SentKeyMof(Keys.None, Keys.Oemtilde);
        }
    }
}
