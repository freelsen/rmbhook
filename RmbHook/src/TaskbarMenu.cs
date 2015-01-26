﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace RmbHook
{
    public class TaskbarMenu
    {
        private TaskbarNotify mnotify = null;
        private RmbKey mrmbkey = null;

        public TaskbarMenu()
        {
        }

        private ContextMenuStrip mmenu = new ContextMenuStrip();
        private ToolStripMenuItem mopenitem = new ToolStripMenuItem();
        private ToolStripMenuItem mexititem = new ToolStripMenuItem();
        private ToolStripMenuItem mkeyonitem = new ToolStripMenuItem();
        private ToolStripMenuItem mkeyoffitem = new ToolStripMenuItem();
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

            mmenu.Items.Add(mopenitem);
            mmenu.Items.Add(mkeyonitem);
            mmenu.Items.Add(mkeyoffitem);
            mmenu.Items.Add(mexititem);

            mnotify = TaskbarNotify.gthis;

            mrmbkey = RmbKey.gthis;
            
            return 0;
        }

        void mkeyoffitem_Click(object sender, EventArgs e)
        {
            mrmbkey.setCmdEnable(false);
        }

        void mkeyonitem_Click(object sender, EventArgs e)
        {
            mrmbkey.setCmdEnable(true);
        }

        void mexititem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void mopenitem_Click(object sender, EventArgs e)
        {
            mnotify.onFormShow();
        }
    }
}