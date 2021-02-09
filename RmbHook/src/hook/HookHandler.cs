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

        bool mistabdown = false;
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
            // 
            if (e.KeyCode == Keys.LMenu)
                mistabdown = true;

            e.Handled = mKeyEventMan.onKeyDown(sender, e);
            misbusy = false;
        }

        public void KeyUp(object sender, KeyEventArgs e)
        {
            mistabdown = false;
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
        Wow _wow = new Wow();
        public void MouseMove(object sender, MouseEventArgs e)
        {
            Color c = FetchColor.gtColor(e.X, e.Y);
            //Console.Out.WriteLine(c.R.ToString()+"," + c.G.ToString() +","+ c.B.ToString());
            mForm.ShowColor(c);
            string str=String.Format("Mouse:({0},{1}),color({2},{3},{4}", e.X,e.Y,c.R,c.G,c.B);
            mForm.setMouseLabe(str);

            //mgesture.onMove2(e.X, e.Y);
            //return 0;
        }

        bool _isrdouble = false;
        public int MouseDown(object sender, MouseEventArgs e)
        {
            //WinMon.gtWinMouse();
            //mgesture.ts_dis(e);

            bool ishandled = false;
            _isrdouble = false;
            //Console.WriteLine(e.Button.ToString());
            switch (e.Button)
            {
                case MouseButtons.Left:
                    ishandled = (mMouseEHandler.OnLDown(ref _isrdouble));
                    if (_isrdouble)
                    {
                        KeyboardSimulator.KeyDown(Keys.M);
                        KeyboardSimulator.KeyUp(Keys.M);
                        
                    }
                    break;
                case MouseButtons.Right:
                    ishandled = (mMouseEHandler.OnRDown(ref _isrdouble));
                    if (_isrdouble)
                    {
                        // auto run;
                        //Console.WriteLine("dclick");
                        //KeyboardSimulator.KeyDown(Keys.Oemtilde);
                        //KeyboardSimulator.KeyUp(Keys.Oemtilde);

                        // macro,
                        _wow.pushMacrobtn();
                    }
                    //else
                    //{
                    //    _wow.doMacro(); 
                    //}
                    //if (mistabdown)
                    if (e.Y>600)
                        _wow.doMacro();
                    break;
                case MouseButtons.Middle:
                    mMouseEHandler.OnMDown();
                    break;
            }

            //mform.onMouseEvent("MouseDown", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
            return 0;// (ishandled) ? 1 : 0;
        }

        public int MouseUp(object sender, MouseEventArgs e)
        {
            //mform.onMouseEvent("MouseUp", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mMouseEHandler.OnLUp();
                    break;
                case MouseButtons.Right:
                    mMouseEHandler.OnRUp();
                    //if (!_isrdouble)
                    //    _wow.doMacro();
                    break;
                case MouseButtons.Middle:
                    mMouseEHandler.OnMDown();
                    break;
            }
            return 0;

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
