using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace KeyMouseDo
{
    public class TaskbarMenu
    {
        private TaskbarNotify mnotify = null;
        //private RmbKey mrmbkey = null;

        public TaskbarMenu()
        {
        }

        private ContextMenuStrip mmenu = new ContextMenuStrip();
        private ToolStripMenuItem mopenitem = new ToolStripMenuItem();
        private ToolStripMenuItem mexititem = new ToolStripMenuItem();
        private ToolStripMenuItem mkeyonitem = new ToolStripMenuItem();
        private ToolStripMenuItem mkeyoffitem = new ToolStripMenuItem();
        private ToolStripMenuItem mmouseonitem = new ToolStripMenuItem();
        private ToolStripMenuItem mmouseoffitem = new ToolStripMenuItem();
        private ToolStripMenuItem mgestureonitem = new ToolStripMenuItem();
        private ToolStripMenuItem mgestureoffitem = new ToolStripMenuItem();
        public ContextMenuStrip getMenu() { return mmenu; }

        public int init()
        {

            mopenitem.Text = "Open";
            mopenitem.Click += new EventHandler(mopenitem_Click);
            mexititem.Text = "Exit";

            mexititem.Click += new EventHandler(mexititem_Click);
            mkeyonitem.Text = "Key On";
            mkeyonitem.Click += new EventHandler(mkeyonitem_Click);
            mkeyoffitem.Text = "Key Off";
            mkeyoffitem.Click += new EventHandler(mkeyoffitem_Click);

            mgestureonitem.Text = "Gesture On";
            mgestureonitem.Click += new EventHandler(GestureOnItem_Click);
            mgestureoffitem.Text = "Gesture Off";
            mgestureoffitem.Click += new EventHandler(GestureOffItem_Click);

            mmouseonitem.Text = "Mouse On";
            mmouseonitem.Click += new EventHandler(MouseOnItem_Click);
            mmouseoffitem.Text = "Mouse Off";
            mmouseoffitem.Click += new EventHandler(MouseOffItem_Click);


            mmenu.Items.Add(mopenitem);

            mmenu.Items.Add(mkeyonitem);
            mmenu.Items.Add(mkeyoffitem);

            mmenu.Items.Add(mgestureonitem);
            mmenu.Items.Add(mgestureoffitem);

            mmenu.Items.Add(mmouseonitem);
            mmenu.Items.Add(mmouseoffitem);

            mmenu.Items.Add(mexititem);

            mnotify = TaskbarNotify.gthis;

            //mrmbkey = RmbKey.gthis;
            
            return 0;
        }
        void mopenitem_Click(object sender, EventArgs e)
        {
            mnotify.onFormShow();
        } 
        void mexititem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void mkeyoffitem_Click(object sender, EventArgs e)
        {
            FormEventMan.KeyOff();
        }
        void mkeyonitem_Click(object sender, EventArgs e)
        {
            FormEventMan.KeyOn();
        }


        void MouseOnItem_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOn();
        }
        void MouseOffItem_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOff();
        }


        void GestureOnItem_Click(object sender, EventArgs e)
        {
            FormEventMan.GestureOn();
        }
        void GestureOffItem_Click(object sender, EventArgs e)
        {
            FormEventMan.GestureOff();
        }




    }
}
