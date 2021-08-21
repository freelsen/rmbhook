using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ls.libs
{
    class AProcess
    {
        Process mprocess;

        void test()
        {
            string str=mprocess.ProcessName;
            IntPtr hwnd= mprocess.MainWindowHandle;
        }


        //Get the wow-process, if success returns the process else null
        public static Process Get(string name = "")
        {
            var names = string.IsNullOrEmpty(name) ? new List<string> { "WoW", "WowClassic", "Wow-64" } : new List<string> { name };

            var processList = Process.GetProcesses();
            foreach (var p in processList)
            {
                if (names.Contains(p.ProcessName))
                {
                    return p;
                }
            }

            //logger.Error($"Failed to find the wow process, tried: {string.Join(", ", names)}");

            return null;
        }


        DateTime LastIntialised = DateTime.Now.AddHours(-1);
        public Process Proc
        {
            get
            {
                if ((DateTime.Now - LastIntialised).TotalSeconds > 10) // refresh every 10 seconds
                {
                    var process = Get();
                    if (process == null)
                    {
                        throw new ArgumentOutOfRangeException("Unable to find the process");
                    }
                    this.mprocess = process;
                }

                return this.mprocess;
            }

            private set { mprocess = value; }
        }

    }
}
