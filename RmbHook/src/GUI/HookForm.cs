using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeyMouseDo.src.keyword;

namespace KeyMouseDo
{
    public partial class MainForm : Form
    {
        public static MainForm gthis = null;

        public MainForm()
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
        
        
        // 
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = tabControl1.SelectedIndex;
            if (idx == 0)
            {
            }
            else if (idx == 1) 
            {
                onGestureTabSelected();
            }
        }


    }
}
