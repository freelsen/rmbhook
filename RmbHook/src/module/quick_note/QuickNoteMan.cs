using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper.quicknote
{
    class QuickNoteMan
    {
        // 2021-01-22, 
        // purpose: popup a window for the use to input some words,
        //  save these words into a file.

        public static QuickNoteMan mthis = null;

        public QuickNoteForm mqnform = new QuickNoteForm();
        public QuickNoteParameter mqnrm = new QuickNoteParameter();
        public QnProc _qnproc = new QnProc();
        public QnEvent _qnevent = new QnEvent();


        public QuickNoteMan()
        {
            mthis = this;
        }

        public int init()
        {
            this.mqnform.onVisible = this._qnproc.OnVisible;
            this.mqnform.onHide = this._qnproc.OnHide;

            this._qnevent._qnform = this.mqnform;

            mqnrm._qnproc = this._qnproc;
            mqnrm.init();

            return 0;
        }

    }
}
