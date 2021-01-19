using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lslibcs;      // 20150621;

namespace RmbHook
{
    class Parameter
    {
        public static Parameter mthis = null;

        public ConfigFile mconfigfile = new ConfigFile();
        string msec = "lsmon";

        public Parameter()
        {
            mthis = this;
        }

        // ----
        public int init()
        {
            int d = 0;
             
            //
            if ((d = mconfigfile.setFilename("rmbhook.ini")) < 0) 
                return d;
            if (!mconfigfile.isExist())
                return -1;
            ConfigReadWrite.setFilename("rmbhook.ini");

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

        public int stKeeptime(int d) { mkeeptime = d; return set("keeptime", mkeeptime.ToString()); }
        public int stKeepnum(int d) { mkeepnum = d; return set("keepnum", mkeepnum.ToString()); }
        public int stStorenum(int d) { mstorenum = d; return set("storenum", mstorenum.ToString()); }
        public int stRestorenum(int d) { mrestorenum = d; return set("restorenum", mrestorenum.ToString()); }

        public int set(string key, string val)
        {
            ConfigFile cf = mconfigfile;
            if (!cf.isExist()) { return -1; }

            cf.setSection(msec);
            cf.write(key, val);

            return 1;
        }
    }
}
