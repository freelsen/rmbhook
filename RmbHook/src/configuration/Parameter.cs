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


        // gesture;
        public int mvmin = 5;
        public int mgaptm = 60;
        public int movertime = 200;
        public int mdismin = 40;                // 140717;
        public int mpair = 1;
        // high timer;
        public int mtminterval = 10;            // 140715;

        // ges fun;
        public int mpage = 1;                   // 140717;

        public int stPage(int d) { mpage = d; return set("page", mpage.ToString()); }
        public int stPair(int d) { mpair = d; return set("pair", mpair.ToString()); }
        public int stDismin(int d) { mdismin = d; return set("dismin", mdismin.ToString()); }

        public int stTmInterval(int d) { mtminterval = d; return set("timer_interval", mtminterval.ToString()); }
        public int stGaptime(int d) { mgaptm = d; return set("gaptime", mgaptm.ToString()); }
        public int stOverTime(int d) { movertime = d; return set("overtime", movertime.ToString()); }
        public int stVmin(int d) { mvmin = d; return set("vmin", mvmin.ToString()); }

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
