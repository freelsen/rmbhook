using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class ObjMan
    {
        public static ObjMan gthis = null;

        // configuration;
        public Parameter mparameter = new Parameter();
        // hook lib;
        public MouseKeyHook mmkhook = new MouseKeyHook();
        public HookEventHandler mHookEventHandler = new HookEventHandler();//2021-01-23
        public MouseEHandler mmehandler = new MouseEHandler();
        // user functions;
        KeyEventMan mKeyEventman = new KeyEventMan();//2021-01-23;
        //public RmbKey mrmbkey = new RmbKey();
        //public WinMon mwinmon = new WinMon();
        public QuickNoteMan mqkman = new QuickNoteMan();// 2021-01-22;

        // GUI;
        public MainForm mform = null;
        public TaskbarNotify mtasknotify = new TaskbarNotify();
        // Gesture;
        //public GestureDirectionCommand mgesfun = new GestureDirectionCommand();
        //public GestureDetectByDirectionOne mgesture = new GestureDetectByDirectionOne();
        public GestureMan mGestureMan = new GestureMan();
        //public GestureParamter mgesprm = new GestureParamter();

        private void create()
        {
        }
        
        public ObjMan()
        {
            create();
            gthis = this;
        }

        public int init()
        {
            mform = MainForm.gthis;
         
            mparameter.init();

            mtasknotify.init(mform);
            
            // gesture;
            mGestureMan.mBkgWorker = mform.getWorker();
            mGestureMan.init();

            // rmbkey;
            mKeyEventman.init();
            //mrmbkey.init();
            //mwinmon.init();

            mHookEventHandler.mForm = mform;
            mHookEventHandler.mKeyEventMan = mKeyEventman;
            mHookEventHandler.mMouseEHandler = mmehandler;

            // quick note;
            mqkman.init();

            mmkhook.mHookEventHandler = mHookEventHandler;
            mmkhook.init();
            mmkhook.start();

            FormEventMan.mtasknotify = mtasknotify; // 2021-02-07,

            return 0;
        }
        public int exit()
        {
            // Not necessary anymore, will stop when application exits
            mmkhook.stop();
            mtasknotify.exit();
            return 0;
        }
    }
}
