using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lslibcs;      // 20150621;

namespace RmbHook
{
    public class Parameter
    {
        // ----
        public int init()
        {
            int d = 0;
                        //
            if ((d = mconfigfile.setFilename("rmbhook.ini")) < 0) 
                return d;
            if (!mconfigfile.isExist())
                return -1;
//  load parameter;
            mconfigfile.setSection("rmbkey");
            if (mconfigfile.readStr("topkey") >= 0)
            {
                topkey = mconfigfile.getStr();
            }
            //-win;
            if (mconfigfile.readInt("keeptime") >= 0)
                mkeeptime = mconfigfile.getInt();
            if (mconfigfile.readInt("keepnum") >= 0)
                mkeepnum = mconfigfile.getInt();
            if (mconfigfile.readInt("storenum") >= 0)
                mstorenum = mconfigfile.getInt();
            if (mconfigfile.readInt("restorenum") >= 0)
                mrestorenum = mconfigfile.getInt();

            return 0;
        }
        // --- variables; ---
        // - key -;
        string topkey = "";
        public string getTopkey()
        {
            return topkey;
        }
        // - win; --
        int mkeeptime = 30;
        int mkeepnum = 2;
        int mstorenum = 20;
        int mrestorenum = 2;
        public int getKeepTime()
        {
            return mkeeptime;
        }
        public int getKeepNum() { return mkeepnum; }
        public int getStorenum() { return mstorenum; }
        public int getRestorenum() { return mrestorenum; }

        public Parameter()
        {
            //mconfigfile.init();
        }
        public ConfigFile mconfigfile = new ConfigFile();
    }
}
