using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

// 2021-01-23. sheng li. AB.CA;
// set the parameters related to the keyboard;

namespace KeyMouseDo
{
    class KeyParameter
    {
        public static KeyParameter mthis = null;
        public KeyMode mRmbKey = null;
        public KeyCommandNonmove mKeyCmdNmv = null;

        string msection = "rmbkey";

        public KeyParameter()
        {
            mthis = this;
        }

        public int init()
        {
            // read parameters from configuration file;
            ConfigReadWrite.setSection(msection);

            string str = "";
            Keys k = Keys.None;

            if (ConfigReadWrite.read(ref str, "topkey") > 0)
            {
                if (parseKey(ref k, str))
                    mRmbKey.mtopkey=k;
            }

            int d=0;
            if (ConfigReadWrite.readInt(ref d, "enable_by_count") > 0)
            {
                mRmbKey.mHookmEnByKey = (d > 0) ? true : false;
            }
            if (ConfigReadWrite.readInt(ref d, "enable_function_window") > 0)
            {
                mKeyCmdNmv.menWindow = (d > 0) ? true : false;
            }
            if (ConfigReadWrite.readInt(ref d, "enable_function_search") > 0)
            {
                mKeyCmdNmv.mensearch = (d > 0) ? true : false;
            }

            return 0;
        }


        //------------------------------------------------------------
        bool parseKey(ref Keys k, string str)        // 2015-06-21;
        {
 
            if (str.Length == 0) return false;

            //
            bool iskey = true;

            if (str.Equals("esc")) 
            { 
                k = Keys.Escape; 
            }
            else if (str.Equals("cap")) 
            { 
                k = Keys.CapsLock; 
            }
            else if (str.Equals("tab"))
            {
                k = Keys.Tab; 
            }
            else
            {
                try
                {
                    //Keys.Oemtilde;//192;~
                    int d = Int32.Parse(str);
                    k = (Keys)d;
                }
                catch (Exception e)
                {
                    iskey = false;
                    Console.Out.WriteLine("setTopkey: parse error.");
                }
            }

            return iskey;
        }
    }
}
