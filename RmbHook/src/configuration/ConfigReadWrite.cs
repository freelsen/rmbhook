using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace WrittingHelper
{
    class ConfigReadWrite
    {
        // --- win32 dll; ---
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        // nonzero=success; If the function successfully copies the string to the initialization file, the return value is nonzero.
        // zero=failed;     If the function fails, or if it flushes the cached version of the most recently accessed initialization file, the return value is zero. 
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        // The return value is the number of characters copied to the buffer, not including the terminating null character.
        
        // read 
        // read string;
        public static int read(ref string result, string key)
        {
            return read(ref result, key, msection, mpath);
        }
        public static int read(ref string result, string key, string section)
        {
            return read(ref result, key, section, mpath);
        }
        public static int read(ref string result, string key, string section, string path)
        {
            int i = GetPrivateProfileString(section, key, "", mtemp, mmaxchar, path);
            if (i>0)
                result=mtemp.ToString();
            return i;
        }
        // read integer;
        public static int readInt(ref int result, string key)
        {
            int ret = 0;

            string val="";
            if ((ret=read(ref val,key))>0)
                result=Convert.ToInt32(val);
            return ret;

        }
        public static int readDouble(ref double result, string key)
        {
            int ret = 0;

            string val = "";
            if ((ret = read(ref val, key)) > 0)
                result = Convert.ToDouble(val);
            return ret;
        }

        // write;
        public static int write(string key, string val)
        {
            return write(msection, key, val);
        }
        public static int write(string section, string key, string value)
        {
            return (int)WritePrivateProfileString(section, key, value, mpath);
        }

        // --- parameter; --
        public static int setFilename(string str)
        {
            mfilename = str;
            return updatePath();
        }
        public static void setSection(string str)
        {
            msection = str;
        }

        // parameters;
        static string mpath = "";
        static string mdir = "";
        static string mfilename = "lslib.ini";
        static string msection = "lslib";

        static int mmaxchar = 500;
        static StringBuilder mtemp = new StringBuilder(mmaxchar);

        private static int updatePath()
        {
            int ret = 0;

            mdir = System.Environment.CurrentDirectory;
            mpath = mdir + "\\" + mfilename;

            if (!isExist())
                --ret;

            return ret;
        }
        public static bool isExist()
        {
            return File.Exists(mpath);
        }

    }
}
