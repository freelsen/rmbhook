using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper.quicknote
{
    class QuickNoteParameter
    {
        public QnProc _qnproc = null;
        public int init()
        {
            if (_qnproc == null)
            {
                Console.WriteLine("QuickNoteParameter: null");
                return -1;
            }
            //
            string str="";
            int d=0;

            // file;
            ConfigReadWrite.setSection("quick_note");
            if (ConfigReadWrite.read(ref str, "path") > 0)
            {
                _qnproc.mfolder = str;
            }
            if (ConfigReadWrite.read(ref str, "defaultfile") > 0)
            {
                _qnproc.mfiledefault = str;
            }
            _qnproc.mpath = $"{_qnproc.mfolder}\\{_qnproc.mfiledefault}";

            if (ConfigReadWrite.read(ref str, "text_append") > 0)
            {
                _qnproc.mappendText = str;
            }
            if (ConfigReadWrite.readInt(ref d, "show_last_lines") > 0)
            {
                _qnproc.mshowLastLines = d;
            }
            return 0;
        }
    }
}
