using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KeyMouseDo
{
    class TaskbarNotify
    {
        public static TaskbarNotify gthis = null;

        private NotifyIcon mnotifyicon = new NotifyIcon();
        private Icon micon1;

        private Form mparent = null;
        private Timer mtimer = new Timer();

        private TaskbarMenu mtaskbarmenu = new TaskbarMenu();

        public TaskbarNotify()
        {
            gthis = this;

            mtimer.Enabled = false;
            mnotifyicon.Visible = true;
        }
        public void setIcon( Icon icon)
        {
            if (icon == null)
                mnotifyicon.Icon = micon1;
            else
                mnotifyicon.Icon = icon;
        }
        public int init(Form parent)
        {
            mparent = parent;
            mparent.WindowState = FormWindowState.Minimized;
            mparent.Hide();

            micon1 = new Icon("res\\icon1.ico");

            mnotifyicon.Text = "KeyMouseDo";
            mnotifyicon.Icon = micon1;
            mnotifyicon.MouseDoubleClick += new MouseEventHandler(mnotifyicon_MouseDoubleClick);

            
            mtimer.Interval = 100;
            mtimer.Tick += new EventHandler(mtimer_Tick);
            mtimer.Start();

            mtaskbarmenu.init();
            mnotifyicon.ContextMenuStrip = mtaskbarmenu.getMenu();

            return 0;
        }
        public void exit()
        {
            mnotifyicon.Visible = false;
        }

        void mtimer_Tick(object sender, EventArgs e)
        {
            mtimer.Stop();
            mparent.Hide();
            mnotifyicon.Visible = true;
        }


        void mnotifyicon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            onFormShow();
        }
        public void onFormShow()
        {
            mparent.Show();
            mparent.WindowState = FormWindowState.Normal;
            mparent.Activate();
        }
        public void onFormSizeChanged()
        {
            if (mparent.WindowState == FormWindowState.Minimized)
            {
                mparent.Hide();
                mnotifyicon.Visible = true;
            }
        }
        
    }
}
