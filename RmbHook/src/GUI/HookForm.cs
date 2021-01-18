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
            gthis = this;
            InitializeComponent();

            AppMan.create();
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
        public void ShowColor(Color clr)
        {
            Graphics g = this.CreateGraphics();
            Rectangle rect = new Rectangle(10, 10, 50, 50);

            SolidBrush b1 = new SolidBrush(clr);
            g.FillRectangle(b1, rect);
        }

        public void ShowMouseLocation(int x, int y)
        {
            curXYLabel.Text = String.Format("Current Mouse Point: X={0}, y={1}", x, y);
        }
        private static int mcntMouseEvent=0;
        public void onMouseEvent(string eventType, string button, string x, string y, string delta)
        {
            if (mcntMouseEvent++ > 100)   mcntMouseEvent = 0;

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
        private static int mcntKeyEvent = 0;
        public void onKeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {
            if (mcntKeyEvent++ > 100) mcntKeyEvent = 0;
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

        private void mhookStart_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOn();
        }

        private void mhookStop_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOff();
        }

        // a thread;
        public BackgroundWorker getWorker()
        {
            return backgroundWorker1;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GestureMan.mthis.Work();
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GestureMan.mthis.Progress();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GestureMan.mthis.Progress();
        }

    }
}
