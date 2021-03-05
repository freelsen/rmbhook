using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WrittingHelper.wow
{
    class WowProc
    {
        public Action runProc;

        public static WowProc mthis = null;
        public Thread _botthread = null;

        public bool Enable { get; set; } = false;
        public WowProc()
        {
            mthis = this;
        }

        public void Toggle()
        {
            if (!_isrun)
                this.Start();
            else
                this.Stop();
        }
        bool _isrun = false;
        public void Start()
        {
            if (!_isrun)
            {
                this.Enable = true;

                 this._botthread = new Thread(WowProc.Loop);
                _botthread.Start();

                _isrun = true;
            }
        }
        public void Stop()
        {
            this.Enable = false;
        }


        public static void Loop()
        {
            WowProc mthis = WowProc.mthis;
            while (mthis.Enable)
            {
                mthis.runProc();
            }
            //return 0;
            mthis._isrun = false;
        }
    }
}
