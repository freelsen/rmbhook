using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ls.libs
{
    public class ConfigFile
    {
        // --- win32 dll; ---
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,string key,string val,string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key,string def,StringBuilder retVal,int size,string filePath);
        
        // --- assist; ---
        public void test()
        {
            int d;
            if (readInt("test") >= 0) d = getInt();
            string s = read("str");
        }
        public bool isExist()
        {
            return File.Exists(mpath);
        }

        // -- read; ---
        public string read(string Section, string Key)
        {
            int i = GetPrivateProfileString(Section, Key, "", mtemp, 500, mpath);
            return mtemp.ToString();
        }
        public string read(string key)
        {
            return read(msection, key);
        }
        public int readInt(string key)
        {
            int ret = 0;

            string val = read(key);
            if (val.Length == 0)
                --ret;
            else
                mintval = Convert.ToInt32(val);
            return ret;
        }
        public int getInt() { return mintval; }
        public int readStr(string key)
        {
            int ret = 0;

            mstrval = read(key);
            if (mstrval.Length == 0)
                --ret;

            return ret;
        }
        public string getStr() { return mstrval; }
        public int readDb(string key)
        {
            int ret = 0;
            string val = read(key);
            if (val.Length == 0)
                --ret;
            else
                mdbval = Convert.ToDouble(val);
            return ret;
        }
        public double getDb() { return mdbval; }

        //--- write; ---
        public int write(string key, string val)
        {
            return write(msection, key, val);
        }
        public int write(string Section,string Key,string Value)
        {
            return (int)WritePrivateProfileString(Section,Key,Value,this.mpath);
        }

        // --- parameter; --
        public int setFilename(string str)
        {
            mfilename = str;
            return updatePath();
        }
        public void setSection(string str)
        {
            msection = str;
        }

        // --- variables; ---
        string mpath = "";
        string mdir = "";
        string mfilename = "lslib.ini";
        string msection = "lslib";

        int mintval;
        double mdbval;
        string mstrval;

        StringBuilder mtemp = new StringBuilder(500);
        public ConfigFile()
        {
            mpath = mfilename;
        }
        private int updatePath()
        {
            int ret = 0;

            mdir = System.Environment.CurrentDirectory;
            mpath = mdir + "\\" + mfilename;
            
            if( !isExist() )
                --ret;

            return ret;
        }
    }
}
