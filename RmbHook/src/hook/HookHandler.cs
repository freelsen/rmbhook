using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MouseKeyboardLibrary;

namespace KeyMouseDo
{
    class HookEventHandler
    {
        public MainForm mForm = null;
        //RmbKey mrmbkey = null;
        public KeyEventMan mKeyEventMan = null;
        public MouseEHandler mMouseEHandler = null;


// ---------------------keyboard events;-----------------------------------
        // 2021-01-23. this variable is used to pervent re-enter the msg proc;
        // re-enter happens when you send key-msg during the process;
        bool misbusy = false;   
        public void KeyDown(object sender, KeyEventArgs e)
        {
            if (misbusy)
                return;
            else
                misbusy = true;
#if DEBUG
            mForm.onKeyboardEvent("KeyDown", e.KeyCode.ToString(), "",
               e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());
            //Console.WriteLine("KeyDown" + "-" + e.KeyCode.ToString() + "-" + e.Shift.ToString() + "-" + e.Alt.ToString() + "-" + e.Control.ToString());
#endif
            e.Handled = mKeyEventMan.onKeyDown(sender, e);
            misbusy = false;
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            //mform.onKeyboardEvent("KeyUp", e.KeyCode.ToString(), "",
            //    e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());

            //if (mrmbkey.onKeymsg(e) > 0)
            //    e.Handled = true;
        }
        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            //mform.onKeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
        }


// --------------------mouse event;----------------------------------------
        public void MouseMove(object sender, MouseEventArgs e)
        {
            //Color c = FetchColor.gtColor(e.X, e.Y);
            //Console.Out.WriteLine(c.A.ToString()+c.B.ToString()+c.G.ToString());    
            //mform.ShowColor(c);
            //mform.ShowMouseLocation(e.X, e.Y);

            //mgesture.onMove2(e.X, e.Y);
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {
            //WinMon.gtWinMouse();
            //mgesture.ts_dis(e);

            //Console.WriteLine(e.Button.ToString());
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (mMouseEHandler.OnLDown())
                    {
                        KeyboardSimulator.KeyDown(Keys.M);
                        KeyboardSimulator.KeyUp(Keys.M);
                    }
                    break;
                case MouseButtons.Right:
                    if (mMouseEHandler.OnRDown())
                    {
                        //Console.WriteLine("dclick");
                        KeyboardSimulator.KeyDown(Keys.Oemtilde);
                        KeyboardSimulator.KeyUp(Keys.Oemtilde);
                    }
                    break;
                case MouseButtons.Middle:
                    mMouseEHandler.OnMDown();
                    break;
            }

            //mform.onMouseEvent("MouseDown", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
        }

        public void MouseUp(object sender, MouseEventArgs e)
        {
            //mform.onMouseEvent("MouseUp", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
        }

        public void MouseWheel(object sender, MouseEventArgs e)
        {
            //if (isMouseRun())
            {
                mForm.onMouseEvent("MouseWheel", "", "", "", e.Delta.ToString());
            }
        }
    }
}
