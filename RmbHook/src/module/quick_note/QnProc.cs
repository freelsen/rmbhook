using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrittingHelper.quicknote
{
    class QnProc
    {
        public string mfolder = "C:\\Users\\Windows User\\OneDrive\\shengdoc";
        public string mfiledefault = "abc.md";
        
        public string mpath = "";
        public int mshowLastLines = 0;
        public string mappendText = "";


        int mdefaultTextLen = 0;
        
        public void OnVisible(TextBox tb)
        {
            tb.Clear();
            tb.Focus();

            //if (this._iscmdline) AddCmdLine(tb);
            AppendUserText(tb);

            mdefaultTextLen = tb.TextLength;
            //if (this._iscmdline) mdefaultTextLen -= this.GetCmdLines(tb).Length;
        }
        public void OnHide(TextBox tb)
        {
            string filename = this.mfiledefault;
            string str = tb.Text;
            if (_iscmdline)
            {
                this._filename = "";
                this.CheckCmdLines(tb);
                str = tb.Text;

                if (_filename.Length > 0)
                {
                    filename = this._filename;
                }
            }


            string path = $"{mfolder}\\{filename}";
            Lslog.log($"path={path}");
            Lslog.log(str);

            if (str.Length > mdefaultTextLen)
            {
                TextMan.AppendText(path, $"{str}{System.Environment.NewLine}");
            }
        }
        


        int mskipline = 3;
        int AppendUserText(TextBox tb)
        {
            int len = 0;
            if (mshowLastLines > 0)
            {
                //mtextman.Open();
                //string str = TextMan.ReadLastLine(mfilename,mskipline);
                //tb.Text = str;
            }
            if (mappendText.Length > 0)
            {
                string str = mappendText;
                str = str.Replace("time", DateTime.Now.ToString("hh:mm tt"));
                str = str.Replace("date", DateTime.Now.ToString("yyyy-MM-dd"));

                //string str = "### **" + DateTime.Now.ToString("hh:mm tt") + "**";
                tb.AppendText(str);
                len = str.Length;

                tb.Select(tb.TextLength, tb.Lines.Length-1);
                
            }

            return len;
        }

        // leave the first line for command;
        string _filename = "";

        bool _iscmdline = true;
        // cmdline strategy 2: cmdline locate anyline,
        char _cmdchar = ':';
        void CheckCmdLines(TextBox tb)
        {
            for (int i=0; i<tb.Lines.Length;i++)
            {
                string str = tb.Lines[i];
                if (str.Length <= 0)
                    continue;
                //
                if (str[0]==_cmdchar)
                {
                    tb.Lines = tb.Lines.Where(line => !(line == str)).ToArray();
                    ParseCmd(str.Remove(0,1));                    
                    break;
                }
            }
        }


        // cmdline strategy 1;
        int _cmdlinenum = 1;
        void AddCmdLine(TextBox tb)
        {
            //tb.AppendText("");
            tb.AppendText("\r\n");
        }
        string GetCmdLines(TextBox tb)
        {
            string str = "";
            Lslog.log("getcmd lines");
            for (int i = 0; i < _cmdlinenum; i++)
            {
                Lslog.log(tb.Lines[i]);
                str += tb.Lines[i] + "\r\n";
            }

            //Lslog.log(str);
            Lslog.log(str.Length.ToString());

            return str;
        }
        void ParseCmdLines(String cmds)
        {
            string[] cmdlines = cmds.Split(new char[] { '\r', '\n' });
            for (int i = 0; i < _cmdlinenum; i++)
                ParseCmd(cmdlines[i]);
        }
        void ParseCmd(string str)
        {
            str.Trim();
            if (str.Length <= 0)
                return;
            
            if (str.Contains('.'))
                this._filename = str;
            else
                this._filename = $"{str}.md";
        }
    }
}
