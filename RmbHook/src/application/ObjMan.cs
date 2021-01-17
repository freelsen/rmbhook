using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmbHook
{
    class ObjMan
    {
        public static ObjMan gthis = null;

        // configuration;
        public Parameter mparameter = null;
        // hook lib;
        public MouseKeybardHook mmklib = null;
        // user functions;
        public RmbKey mrmbkey = null;
        public WinMon mwinmon = null;
        // GUI;
        public HookForm mform = null;
        public TaskbarNotify mtasknotify = null;

        private void create()
        {
            mparameter = new Parameter();
            mmklib = new MouseKeybardHook();
            mtasknotify = new TaskbarNotify();
            mrmbkey = new RmbKey();
            mwinmon = new WinMon();
        }
        
        public ObjMan()
        {
            create();
            gthis = this;
        }

        public int init()
        {
            mform = HookForm.gthis;
         
            mparameter.init(); 
            mtasknotify.init(mform);

            mrmbkey.init();
            mwinmon.init();

            HookHandler.mform = mform;
            HookHandler.mrmbkey = mrmbkey;

            mmklib.init();
            mmklib.start();

            return 0;
        }
        public int exit()
        {
            // Not necessary anymore, will stop when application exits
            mmklib.stop();
            mtasknotify.exit();
            return 0;
        }
    }
}
