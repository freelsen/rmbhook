using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RmbHook
{
    class QuickNoteMan
    {
        // 2021-01-22, 
        // purpose: popup a window for the use to input some words,
        //  save these words into a file.

        public static QuickNoteMan mthis = null;

        QuickNoteForm mqnform = new QuickNoteForm();

        public QuickNoteMan()
        {
            mthis = this;
        }
        public int init()
        {
            
            return 0;
        }

        public void ShowWindow()
        {
            if (mqnform.Visible)
            {
                mqnform.Hide();
            }
            else
            {
                mqnform.Show();
                mqnform.Activate();
                mqnform.Focus();
            }
        }
        public void ResetWindow()
        {
            mqnform.ResetNote();
        }

    }
}
