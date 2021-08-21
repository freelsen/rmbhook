using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper
{
    class CmdTimer
    {
        DateTime _tmlast;
        DateTime _tmnow;
        int _cmddelay = 5000; // 5s;

        bool _iscmdbefore = false;
        public bool OnCmd(bool iscmd)
        {
            bool isovertime = false;

            _tmnow = DateTime.Now;
            if (!_iscmdbefore && iscmd) // start;
            {
                _tmlast = _tmnow;
            }
            if (iscmd)
            {
                TimeSpan ts = _tmnow - _tmlast;
                if (ts.TotalMilliseconds > _cmddelay)
                    isovertime = true;

            }
            _iscmdbefore = iscmd;

            return isovertime;
        }
    }
}
