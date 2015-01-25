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

        private MouseKeybardLib mlib = new MouseKeybardLib();
        private TaskbarNotify mtasknotify = new TaskbarNotify();
        private RmbKey mrmbkey = new RmbKey();

        public HookForm()
        {
            InitializeComponent();
        }

        private void hookForm_Load(object sender, EventArgs e)
        {
            mtasknotify.init( this );

            mlib.init( this );
            mlib.start();

            mrmbkey.init();

        }
        private void HookForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Not necessary anymore, will stop when application exits
            mlib.stop();
            mtasknotify.exit();
        }   
        private void hookForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void HookForm_SizeChanged(object sender, EventArgs e)
        {
            mtasknotify.onFormSizeChanged();
        }

        
// --- show event; ---

        public void SetXYLabel(int x, int y)
        {
            curXYLabel.Text = String.Format("Current Mouse Point: X={0}, y={1}", x, y);
        }
        public void AddMouseEvent(string eventType, string button, string x, string y, string delta)
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
        public void AddKeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
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
