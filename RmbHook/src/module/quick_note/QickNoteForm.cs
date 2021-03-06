using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WrittingHelper.quicknote
{
    public partial class QuickNoteForm : Form
    {
        public Action<TextBox> onVisible;
        public Action<TextBox> onHide;

        public QuickNoteForm()
        {
            InitializeComponent();
        }
        public void init()
        {
            
        }

        
        //--------------------------------------
        public void ResetNote()
        {
            textBox1.Clear();
        }

        
        private void QuickNoteForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                this.onVisible(this.textBox1);
            }
            else
            {
                this.onHide(this.textBox1);
            }
        }

        private void QuickNoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        
    }
}
