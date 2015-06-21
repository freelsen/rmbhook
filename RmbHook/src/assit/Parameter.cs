using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lslibcs;      // 20150621;

namespace RmbHook
{
    public class Parameter
    {
        public string getTopkey()
        {
            return topkey;
        }
        // ----
        public int init()
        {
            int d = 0;
            if ((d = mconfigfile.setFilename("rmbhook.ini")) < 0) return d;
            //  load parameter;
            mconfigfile.setSection("rmbkey");
            if (mconfigfile.readStr("topkey") >= 0)
            {
                topkey = mconfigfile.getStr();
            }

            return 0;
        }
        // --- variables; ---
        string topkey = "";

        public Parameter()
        {
            //mconfigfile.init();
        }
        ConfigFile mconfigfile = new ConfigFile();
    }
}
