using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace KeyMouseDo
{
    class WowMacroOne
    {
        //public Keys[,] _keylist1 = new Keys[10, 2];
        public WowMacroOne()
        {
        }

        // mouse event 1,
        // start strategy,
        bool _ismacrostart = true; // right double click to start/stop;
        public void pushMacrobtn()
        {
            _ismacrostart = !_ismacrostart;
            //if (!_ismacrostart)
            KeyHelper.SentKeyMof(Keys.None, Keys.Space);
        }

        //public void 
        public bool isMacro(int x, int y)
        {
            return false;
        }
        public void doMacro()
        {
            doMacro(1);
        }
        public void doMacro(int index)
        {
            if (!_ismacrostart) return;

            if (WowCmd.mthis == null) return;
            Keys[,] keylist = WowCmd.mthis.mkeylist1;

            int istart = 0; int iend = 0;
            if (index == 1)
            {
                istart = 0; iend = 5;
            }
            else if (index == 2)
            {
                istart = 5; iend = 10;
            }

            for (int i = istart; i < iend; i++)
            {
                KeyHelper.SentKeyMof(keylist[i, 0], keylist[i, 1]);
                //Thread.Sleep(10);
            }
        }
    }
}
