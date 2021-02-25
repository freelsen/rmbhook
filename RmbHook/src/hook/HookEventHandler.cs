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
        public MouseEventHelper mMouseEHandler = null;

        public WowMan mwowman = null;   // 2021-02-14,


        // ---------------------keyboard events;-----------------------------------
        // 2021-01-23. this variable is used to pervent re-enter the msg proc;
        // re-enter happens when you send key-msg during the process;
        bool misbusy = false;

        bool mistabdown = false;
        bool miskeydown = false;
        int mbusycnt = 0;
        public void KeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine("down=" + KeyHandler.miskeybusy.ToString());
            // 
            if (KeyHelper.miskeybusy)
                return;

            //mbusycnt++;

            if (misbusy)
                return;
            else
                misbusy = true;
            

            miskeydown = true;
            //Console.WriteLine("keydown=" + miskeydown.ToString());
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
            if (KeyHelper.miskeybusy)
                return;

            //mbusycnt--;

            miskeydown = false;
            DbMsg.Msg("keydown=" + miskeydown.ToString());

            mistabdown = false;
            //mform.onKeyboardEvent("KeyUp", e.KeyCode.ToString(), "",
            //    e.Shift.ToString(), e.Alt.ToString(), e.Control.ToString());

            //if (mrmbkey.onKeymsg(e) > 0)
            //    e.Handled = true;
        }
        public void KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyHelper.miskeybusy)
                return;

            //mform.onKeyboardEvent("KeyPress", "", e.KeyChar.ToString(), "", "", "");
        }


// --------------------mouse event;----------------------------------------
        public void MouseMove(object sender, MouseEventArgs e)
        {
            //if(WowMan.mthis!=null)
            //{
            //    WowMan.mthis.onMove(e.X, e.Y);
            //}
            //return;
            Color c = FetchColor.gtColor(e.X, e.Y);
            //Console.Out.WriteLine(c.R.ToString()+"," + c.G.ToString() +","+ c.B.ToString());
            mForm.ShowColor(c);
            string str=String.Format("Mouse:({0},{1}),color({2},{3},{4}", e.X,e.Y,c.R,c.G,c.B);
            mForm.setMouseLabe(str);
            
            //DbMsg.Msg(str);

            //mgesture.onMove2(e.X, e.Y);
            //return 0;
        }

        bool misrdouble = false;
        

        // wow> if you want to eat the mouse event, 
        // then must process the MouseDown event, as the wow does.
        public int MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseHelper.misbusy)
                return 0;
            //misbusy = true;
            //WinMon.gtWinMouse();
            //mgesture.ts_dis(e);

            //Point point1 = new Point();
            //point1.X = 0; point1.Y = 0;
            //WinApis.GetCursorPos(ref point1);
            
            DbMsg.Msg("mouse down," + e.X.ToString() + "," + e.Y.ToString());

            //return 0;

            bool ishandled = false;
            misrdouble = false;
            //Console.WriteLine(e.Button.ToString());
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (mwowman.onLmouseDown(e.X, e.Y))
                        ishandled = true;

                    //mMouseEHandler.OnLDown(ref misrdouble);
                    //if (misrdouble)
                    //{
                    //    mwowman.onLDouble();
                    //}

                    break;
                case MouseButtons.Right:
                    if (mwowman.onRmouseDown(e.X, e.Y))
                        ishandled = false;

                    break;
                case MouseButtons.Middle:
                    mMouseEHandler.OnMDown();
                    break;
            }

            //mform.onMouseEvent("MouseDown", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");
            int ret = ((ishandled) && !miskeydown) ? 1 : 0;
            DbMsg.Msg("ret=" + ret.ToString()+","+miskeydown.ToString());
            return ret;
        }

        public int MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseHelper.misbusy)
                return 0;
            DbMsg.Msg("mouse up," + e.X.ToString() + "," + e.Y.ToString());
            //return 0;
            //mform.onMouseEvent("MouseUp", e.Button.ToString(),
            //    e.X.ToString(), e.Y.ToString(), "");

            //Point point = new Point();
            //point.X = 0; point.Y = 0;
            //WinApis.GetCursorPos(ref point);
            //Console.WriteLine("mouse up," + point.X.ToString() + "," + point.Y.ToString());


            switch (e.Button)
            {
                case MouseButtons.Left:
                    mMouseEHandler.OnLUp();
                    break;
                case MouseButtons.Right:
                    mMouseEHandler.OnRUp();

                    mMouseEHandler.OnRDown(ref misrdouble);
                    if (misrdouble)
                    {
                        mwowman.onRDouble();
                    }

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
