using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class QuickNoteParameter
    {
        public QuickNoteForm mQuickNoteForm = null;
        public int init()
        {
            if (mQuickNoteForm == null)
            {
                Console.WriteLine("QuickNoteParameter: null");
                return -1;
            }
            //
            string str="";
            int d=0;
            ConfigReadWrite.setSection("quick_note");
            if (ConfigReadWrite.read(ref str, "path") > 0)
            {
                mQuickNoteForm.mpath = str;
            }
            if (ConfigReadWrite.read(ref str, "text_append") > 0)
            {
                mQuickNoteForm.mappendText = str;
            }
            if (ConfigReadWrite.readInt(ref d, "show_last_lines") > 0)
            {
                mQuickNoteForm.mshowLastLines = d;
            }
            return 0;
        }
    }
}
