using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrittingHelper.libs;
using WrittingHelper.wow;

namespace WrittingHelper
{
    class DrawFormMan
    {
        public static DrawFormMan _this = null;

        public WowMan mwowman = null;
        public IntPtr mformhwnd = (IntPtr)null;

        public DrawForm mdrawform = new DrawForm();
        public WowWin _wowwin = new WowWin();
        public DfEvent _dfevent = new DfEvent();
        public FormClientEvent _formclientevent = new FormClientEvent();


        public int init()
        {
            _this = this;

            Assemble();

            _dfevent.mdrawform = mdrawform;
            _dfevent._formclientevent = this._formclientevent;
            _dfevent._wowwin = this._wowwin;

            mformhwnd = mdrawform.Handle;
            mdrawform.mdfman = this;
            mdrawform.init();

            return 0;
        }

        void Assemble()
        {
            
        }

        

    }
}
