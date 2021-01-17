using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RmbHook.src.keyword;

namespace RmbHook
{
    public partial class HookForm : Form
    {
        public static HookForm gthis = null;

        public HookForm()
        {
            AppMan.create();

            gthis = this;
            InitializeComponent();
        }

// --- form event; ---
        private void hookForm_Load(object sender, EventArgs e)
        {
            FormEventMan.Load(sender, e);
        }
        private void HookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormEventMan.Closing(sender, e);
        }   
        private void hookForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormEventMan.Closed(sender, e);
        }
        private void HookForm_SizeChanged(object sender, EventArgs e)
        {
            FormEventMan.SizeChanged(sender, e);
        }
        
// --- show event; ---

        public void SetXYLabel(int x, int y)
        {
            curXYLabel.Text = String.Format("Current Mouse Point: X={0}, y={1}", x, y);
        }
        public void onMouseEvent(string eventType, string button, string x, string y, string delta)
        {

            listView1.Items.Insert(0,
                new ListViewItem(
                    new string[]{
                        eventType, 
                        button,
                        x,
                        y,
                        delta
                    }));
        }
        public void onKeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {
            listView2.Items.Insert(0,
                 new ListViewItem(
                     new string[]{
                        eventType, 
                        keyCode,
                        keyChar,
                        shift,
                        alt,
                        control
                }));
        }

    }
}
