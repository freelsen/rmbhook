using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RmbHook
{
    class HookEventHandler
    {
        public static HookForm mform = null;
        public static RmbKey mrmbkey = null;

        // keyboard events;
        public static void KeyDown(object sender, KeyEventArgs e)
        {
            mform.onKeyboardEvent("KeyDown", e.KeyCode.ToString(), "",
               e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());

            if (mrmbkey.onKeymsg(e) > 0)
                e.Handled = true;
        }
        public static void KeyUp(object sender, KeyEventArgs e)
        {
            mform.onKeyboardEvent("KeyUp", e.KeyCode.ToString(), "",
                e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
        }
        public static void KeyPress(object sender, KeyPressEventArgs e)
        {
            mform.onKeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
        }

        // mouse event;
        public static void MouseMove(object sender, MouseEventArgs e)
        {
            //Color c = FetchColor.gtColor(e.X, e.Y);
            //Console.Out.WriteLine(c.A.ToString()+c.B.ToString()+c.G.ToString());    
            //mform.ShowColor(c);
            //mform.ShowMouseLocation(e.X, e.Y);

            //mgesture.onMove2(e.X, e.Y);
        }

        public static void MouseDown(object sender, MouseEventArgs e)
        {
            //WinMon.gtWinMouse();
            //mgesture.ts_dis(e);

            //mform.onMouseEvent("MouseDown", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
        }

        public static void MouseUp(object sender, MouseEventArgs e)
        {
            //mform.onMouseEvent("MouseUp", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
        }

        public static void MouseWheel(object sender, MouseEventArgs e)
        {
            //if (isMouseRun())
            {
                mform.onMouseEvent("MouseWheel", "", "", "", e.Delta.ToString());
            }
        }
    }
}
