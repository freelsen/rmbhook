using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace KeyMouseDo.src.keyword
{
    public partial class SearchForm : Form
    {
        private bool exit = false;
        public void setExit(bool b) { exit = b; }

        private string currentpath = "";
        public void setCurrentPath(string path)
        {
            currentpath = path;
            comboBox1.Text = currentpath;
        }

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
                string s = comboBox1.Text;
                if (s == null || s.Length == 0) return;
                if (s[0] == '.')
                {
                    onSearch(s);
                }
                else
                {
                    onOpenPath(s);
                }
            }
            e.Handled = false;
        }
        void onOpenPath(string path)
        {
            if (path.EndsWith(":"))
                path += "\\";

            if (currentpath == path)
                return;

            PathAnalyser pathanalyser = LsKeyword.getThis().getPathAnalyser();
            pathanalyser.openPath(path, true,-1);

            //currentpath = path;
            setCurrentPath(path);  

            //
            listBox1.Items.Clear();
            ArrayList als = pathanalyser.listPath(path);
            foreach (string s in als)
            {
                listBox1.Items.Add(s);
            }

        }
        void onSearch(string keys)
        {
            PathAnalyser pathanalyser = LsKeyword.getThis().getPathAnalyser();
            
            HashSet<string> hss = pathanalyser.search(keys);
            if (hss.Count > 0)
            {
                ResultForm rf = new ResultForm();
                rf.Show();
                rf.showList(hss);
                rf.Activate();
            }
        }

        private void pathFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = onChooseFile();
            PathFile pf = LsKeyword.getThis().getPathfile();
            pf.onSetPathfile(path);
            pf.loadFile();
        }

        private string onChooseFile()
        {
            string path = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";   //注意这里写路径时要用c:\\而不是c:\

            openFileDialog.Filter = "文本文件|*.*|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;
            }

            return path;
        }

        public void showList(HashSet<string> hss)
        {
            listBox2.Items.Clear();
            foreach (string s in hss)
                listBox2.Items.Add(s);
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PathAnalyser pathanalyser = LsKeyword.getThis().getPathAnalyser();

            string s = (string)this.listBox1.Items[listBox1.SelectedIndex];
            if (s == "..")
            {
                s = pathanalyser.getParentPath(currentpath);
            }
            else
                s = currentpath + "\\" + s;

            if (s == null)
                return;
            if (File.Exists(s))
                return;

            onOpenPath(s);
        }

        public void setSearchFocus()
        {
            this.comboBox1.Focus();
        }

        // --- search form page 2; ----
        private void button1_Click(object sender, EventArgs e)
        {
            // test reg;
            string str = "this is approximately forty approximately people";
            string s = @"\w*o(\w*)t\w*";
            Regex reg = new Regex(s);

            MatchCollection mc;
            //mc = Regex.Matches(str, s); 
            mc = reg.Matches(str);

            //Match mt = reg.Match(str);
            //string v = mt.Groups[1].Value;
            Console.WriteLine(mc.Count);

            foreach (Match mt in mc)
            {
                Console.WriteLine(mt.Value);
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LsKeyword.getThis().saveFile();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LsKeyword.getThis().saveFiletable();
        }
    }
}
