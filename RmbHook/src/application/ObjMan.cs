﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmbHook
{
    class ObjMan
    {
        public static ObjMan gthis = null;

        // configuration;
        public Parameter mparameter = new Parameter();
        // hook lib;
        public MouseKeyHook mmkhook = new MouseKeyHook();
        // user functions;
        public RmbKey mrmbkey = new RmbKey();
        public WinMon mwinmon = new WinMon();
        // GUI;
        public HookForm mform = null;
        public TaskbarNotify mtasknotify = new TaskbarNotify();
        // Gesture;
        public GesFun mgesfun = new GesFun();
        public Gesture mgesture = new Gesture();
        public GestureMan mgesman = new GestureMan();

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
            mform = HookForm.gthis;
         
            mparameter.init(); 
            mtasknotify.init(mform);
            
            // gesture;
            mgesture.linit();
            mgesfun.linit();

            mgesman.mgesfun = mgesfun;
            mgesman.mgesture = mgesture;
            mgesman.init();

            mrmbkey.init();
            mwinmon.init();

            HookEventHandler.mform = mform;
            HookEventHandler.mrmbkey = mrmbkey;

            mmkhook.init();
            mmkhook.start();

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