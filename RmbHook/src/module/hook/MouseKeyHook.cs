using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MouseKeyboardLibrary;
using System.Windows.Forms;

namespace RmbHook
{
    public class MouseKeyHook
    {
        public static MouseKeyHook gthis = null;
    
        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        public MouseKeyHook()
        {
            gthis = this;
        }

        public int init()
        {
            //mouseHook.MouseMove += new MouseEventHandler(HookEventHandler.MouseMove);
            mouseHook.MouseDown += new MouseEventHandler(HookEventHandler.MouseDown);
            mouseHook.MouseUp += new MouseEventHandler(HookEventHandler.MouseUp);
            //mouseHook.MouseWheel += new MouseEventHandler(HookEventHandler.MouseWheel);

            keyboardHook.KeyDown += new KeyEventHandler(HookEventHandler.KeyDown);
            //keyboardHook.KeyUp += new KeyEventHandler(kHookHandler.KeyUp);
            //keyboardHook.KeyPress += new KeyPressEventHandler(HookHandler.KeyPress);

            return 0;
        }

        // Run with Enable Control;
        public int mkeyRunEn = 1;
        public int mmouseRunEn = 0;

        public int start()
        {
            int r = 0;
            if (gthis.mkeyRunEn > 0)
                r = startKeyHook();

            if (gthis.mmouseRunEn > 0)
                r = startMouseHook();

            return r;
        }
        public void stop()
        {
            stopKeyHook();
            stopMouseHook();
        }

        // Run without Enable Control;
        public int startKeyHook()
        {
            return keyboardHook.Start() ? 1 : 0;
        }
        public void stopKeyHook()
        {
            keyboardHook.Stop();
        }
        public int startMouseHook()
        {
            return mouseHook.Start() ? 1 : 0;
        }
        public void stopMouseHook()
        {
            mouseHook.Stop();
        }
    }
}
