using System;
using KeyMouseDo.src.keyword;

namespace KeyMouseDo
{
    class FormEventMan
    {
        /* 2021-01-17,
         *
         */
        public static TaskbarNotify mtasknotify = null;

        public FormEventMan()
        {
        }

        public static void Load(object sender, EventArgs e)
        {
            AppMan.init();
        }

        public static void Closing(object sender, EventArgs e)
        {
            AppMan.exit();
            LsKeyword.getThis().exit();     // 20160503;
        }

        public static void Closed(object sender, EventArgs e)
        {
            Console.WriteLine("->hookform closed.");
        }

        public static void SizeChanged(object sender, EventArgs e)
        {
            //TaskbarNotify mtasknotify = ObjMan.gthis.mtasknotify; // 2021-02-07,
            if (mtasknotify != null)
                mtasknotify.onFormSizeChanged();
        }

        // hook control;
        public static void HookStart()
        {
            MouseKeyHook.gthis.startMouseHook();
        }

        public static void HookStop()
        {
            MouseKeyHook.gthis.stopMouseHook();
        }



        public static void KeyOn()
        {
            KeyCommandMode.gthis.startKeyMode();
        }
        public static void KeyOff()
        {
            KeyCommandMode.gthis.stopKeyMode();
        }



        public static void MouseOn()
        {
            MouseKeyHook.gthis.startMouseHook();
            //GestureMan.mthis.Start();
        }
        public static void MouseOff()
        {
            //GestureMan.mthis.Stop();
            MouseKeyHook.gthis.stopMouseHook();
        }


        public static void GestureOn()
        {            
            GestureMan.mthis.Start();
        }
        public static void GestureOff()
        {
            GestureMan.mthis.Stop();            
        }
    }
}