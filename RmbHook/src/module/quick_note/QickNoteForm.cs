using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WrittingHelper
{
    public partial class QuickNoteForm : Form
    {
        public string mpath = "C:\\Users\\Windows User\\OneDrive\\shengdoc\\abc.md";
        public int mshowLastLines = 0;
        public string mappendText = "";

        public QuickNoteForm()
        {
            InitializeComponent();
        }
        public void init()
        {
            
        }

        void checkAppendText()
        {
            if (mshowLastLines > 0)
            {
                //mtextman.Open();
                //string str = TextMan.ReadLastLine(mfilename,mskipline);
                //textBox1.Text = str;
            }
            if (mappendText.Length > 0)
            {
                string str = mappendText;
                str=str.Replace("time", DateTime.Now.ToString("hh:mm tt"));
                str=str.Replace("date", DateTime.Now.ToString("yyyy-MM-dd"));

                //string str = "### **" + DateTime.Now.ToString("hh:mm tt") + "**";
                textBox1.AppendText(str);
                textBox1.Select(textBox1.TextLength, 0);
            }
        }
        //--------------------------------------
        public void ResetNote()
        {
            textBox1.Clear();
        }

        int mdefaultTextLen = 0;
        int mskipline = 3;
        private void QuickNoteForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                textBox1.Clear();
                textBox1.Focus();

                checkAppendText();                

                mdefaultTextLen = textBox1.TextLength;

            }
            else
            {
                string str = textBox1.Text;
                if (str.Length > mdefaultTextLen)
                {
                    TextMan.AppendText(mpath, str + System.Environment.NewLine);
                }
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
