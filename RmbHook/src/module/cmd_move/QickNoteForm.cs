using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace KeyMouseDo
{
    public partial class QuickNoteForm : Form
    {
        public static string mfilename = "C:\\Users\\Windows User\\OneDrive\\shengdoc\\abc.md";

        public QuickNoteForm()
        {
            InitializeComponent();

        }

        public void ResetNote()
        {
            textBox1.Clear();
        }

        //--------------------------------------
        //TextMan mtextman = new TextMan(mfilename);
        int mtextlen = 0;
        int mskipline = 3;
        private void QuickNoteForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                textBox1.Clear();
                textBox1.Focus();

                //mtextman.Open();

                //string str = TextMan.ReadLastLine(mfilename,mskipline);
                //textBox1.Text = str;

                string str= "### **" + DateTime.Now.ToString("hh:mm tt") + "** ";
                textBox1.AppendText(str);
                textBox1.Select(textBox1.TextLength, 0);

                mtextlen = textBox1.TextLength;

            }
            else
            {
                string str = textBox1.Text;
                if (str.Length>mtextlen)
                    TextMan.AppendText(mfilename, str + System.Environment.NewLine);
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
