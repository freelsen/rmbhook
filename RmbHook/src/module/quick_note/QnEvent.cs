using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper.quicknote
{
    class QnEvent
    {
        public QuickNoteForm _qnform = null;

        public void ShowWindow()
        {
            if (_qnform.Visible)
            {
                _qnform.Hide();
            }
            else
            {
                _qnform.Show();
                _qnform.Activate();
                _qnform.Focus();
            }
        }
        public void ResetWindow()
        {
            _qnform.ResetNote();
        }
    }
}
