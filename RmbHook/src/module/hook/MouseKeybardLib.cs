using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MouseKeyboardLibrary;
using System.Windows.Forms;

namespace RmbHook
{
    public class MouseKeybardHook
    {
        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        public MouseKeybardHook()
        {
        }

        public int init()
        {
            //mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            //mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            //mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            //mouseHook.MouseWheel += new MouseEventHandler(mouseHook_MouseWheel);

            keyboardHook.KeyDown += new KeyEventHandler(HookHandler.KeyDown);
            //keyboardHook.KeyUp += new KeyEventHandler(keyboardHook_KeyUp);
            //keyboardHook.KeyPress += new KeyPressEventHandler(keyboardHook_KeyPress);

            return 0;
        }
        public void start()
        {
            mouseHook.Start();
            keyboardHook.Start();
        }
        public void stop()
        {
            mouseHook.Stop();
            keyboardHook.Stop();
        }

    }
}
