using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WrittingHelper
{
    class WowProc
    {
        public static WowProc mthis = null;
        public WowProc()
        {
            mthis = this;
        }
        public bool Enable { get; set; } = false;
        Thread mthread = new Thread(Loop);


        public static void Loop()
        {
            WowProc mthis = WowProc.mthis;
            while (mthis.Enable)
            {

            }
            //return 0;
        }
    }
}
