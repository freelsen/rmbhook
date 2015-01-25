using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MouseKeyboardLibrary;
using System.Windows.Forms;

namespace RmbHook
{
    public class MouseKeybardLib
    {
        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        private HookForm mparent = null;
        private RmbKey mrmbkey = null;

        public MouseKeybardLib()
        {

        }
        public int init(HookForm parent)
        {
            mparent = parent;
            mrmbkey = RmbKey.gthis;

            //mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            //mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown);
            //mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);
            //mouseHook.MouseWheel += new MouseEventHandler(mouseHook_MouseWheel);

            keyboardHook.KeyDown += new KeyEventHandler(keyboardHook_KeyDown);
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

        // --- keyboard event handle ---
        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            mparent.AddKeyboardEvent( "KeyDown", e.KeyCode.ToString(), "", 
               e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString() );

            if (mrmbkey.onKeymsg(e) > 0)
                e.Handled = true;
        }
        void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {
            mparent.AddKeyboardEvent("KeyUp", e.KeyCode.ToString(), "",
                e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
        }
        void keyboardHook_KeyPress(object sender, KeyPressEventArgs e)
        {
            mparent.AddKeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
        }

        // --- mouse event handle; ---
        void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            mparent.SetXYLabel(e.X, e.Y);
        }
        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            mparent.AddMouseEvent( "MouseDown", e.Button.ToString(),
                e.X.ToString(), e.Y.ToString(), "" );
        }
        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            mparent.AddMouseEvent( "MouseUp", e.Button.ToString(),
                e.X.ToString(), e.Y.ToString(), "" );
        }
        void mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            //if (isMouseRun())
            {
                mparent.AddMouseEvent("MouseWheel", "", "", "", e.Delta.ToString());
            }
        }

    }
}
