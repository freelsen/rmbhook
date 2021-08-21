using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ls.libs;

namespace WrittingHelper
{
    class DfEvent
    {
        public DrawForm mdrawform = null;
        public WowWin _wowwin = null;
        public FormClientEvent _formclientevent = null;

        // the timer process;
        public void setTimer()
        {
            mdrawform.setTimer();
        }
        public void setTimer(bool b)
        {
            mdrawform.setTimer(b);
        }
        // show, hide the GUI;
        public void doTest()
        {
            if (mdrawform.Visible == false)
            {
                mdrawform.Show();

                Bind2Wow();

                // reset background;
                Graphics grap = mdrawform.CreateGraphics();
                grap.Clear(Color.Transparent);

                // notify graphs,
                _formclientevent.OnSize(_wowwin.mclient);

                _formclientevent.OnParint(grap);
            }
            else
            {
                mdrawform.Visible = false;
            }
        }



        void Bind2Wow()
        {
            if (_wowwin.FindWin())
            {
                Lslog.log("target win found");
                WinApis.MoveWindow(mdrawform.Handle,
                    _wowwin.mposition.X, _wowwin.mposition.Y
                    , _wowwin.mrect.right - _wowwin.mrect.left,
                    _wowwin.mrect.bottom - _wowwin.mrect.top,
                    true);
            }
        }
    }
}
