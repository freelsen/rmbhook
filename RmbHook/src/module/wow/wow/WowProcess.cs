using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WrittingHelper
{
    class WowProcess
    {
        Process _wowproc=null;

        List<string> _wowprocnames = new List<string> { "WoW", "WowClassic", "Wow-64" };
        
        public IntPtr GetHwnd()
        {
            return _wowproc.MainWindowHandle;
        }
        
        Process GetProcess()
        {
            var proclist = Process.GetProcesses();
            foreach(var p in proclist)
            {
                if (_wowprocnames.Contains(p.ProcessName))
                    return p;
            }
            return null;
        }
    }
}
