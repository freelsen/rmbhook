using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmbHook
{
    public class Factor
    {
        public static Factor gm = null;
        public Parameter mparameter = new Parameter();

        public MouseKeybardLib mlib = new MouseKeybardLib();
        public TaskbarNotify mtasknotify = new TaskbarNotify();
        public RmbKey mrmbkey = new RmbKey();

        public Factor()
        {
            gm = this;
        }
        public int init()
        {
            mparameter.init();

            HookForm hform = HookForm.mthis;
            mtasknotify.init(hform);

            mlib.init(hform);
            mlib.start();

            mrmbkey.init();

            return 0;
        }
        public void exit()
        {
            // Not necessary anymore, will stop when application exits
            mlib.stop();
            mtasknotify.exit();
        }
    }
}
