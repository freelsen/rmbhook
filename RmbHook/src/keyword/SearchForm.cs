using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RmbHook.src.keyword
{
    public partial class SearchForm : Form
    {
        private bool exit = false;
        public void setExit(bool b) { exit = b; }

        public SearchForm()
        {
            InitializeComponent();
        }

        private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exit)
                e.Cancel = false;
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
            }
            e.Handled = false;
        }
    }
}
