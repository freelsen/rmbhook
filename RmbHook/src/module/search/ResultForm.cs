using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WrittingHelper.src.keyword
{
    public partial class ResultForm : Form
    {
        public ResultForm()
        {
            InitializeComponent();
        }

        public void showList(HashSet<string> hss)
        {
            foreach (string s in hss)
            {
                listBox1.Items.Add(s);
            }
        }
    }
}
