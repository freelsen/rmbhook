using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RmbHook
{
    public partial class HookForm : Form
    {
        public static HookForm mthis = null;
        private Factor mfactor = new Factor();  //20150621;

        private MouseKeybardLib mlib;// = new MouseKeybardLib();
        private TaskbarNotify mtasknotify;// = new TaskbarNotify();
        private RmbKey mrmbkey;// = new RmbKey();

        public HookForm()
        {
            mthis = this;
            InitializeComponent();
        }
        public int init()
        {
            mlib = Factor.gm.mlib;
            mtasknotify = Factor.gm.mtasknotify;
            mrmbkey = Factor.gm.mrmbkey;

            int d = mfactor.init();

            return d;
        }
        public void exit()
        {
            mfactor.exit();
        }

// --- form event; ---
        private void hookForm_Load(object sender, EventArgs e)
        {
            init();
        }
        private void HookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            exit();
        }   
        private void hookForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void HookForm_SizeChanged(object sender, EventArgs e)
        {
            if( mtasknotify != null )
                mtasknotify.onFormSizeChanged();
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
