using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lslibcs;
using System.Windows.Forms;

namespace RmbHook
{
    class GestureParamter
    {
        public static GestureParamter mthis = null;
        public GesFun mgesfun = null;
        public GestureRec mgesture = null;

        string msection = "gesture";
        public GestureParamter()
        {
            mthis = this;
        }

        public int init()
        {
            // read parameters from configuration file;
            ConfigReadWrite.setSection(msection);

            // GesFun;
            string str = "";
            Keys key=Keys.A;
            string keystr = "key";
            for (int i = 0; i < 8; i++)
            {
                string ks = keystr + (i+1).ToString();
                if (ConfigReadWrite.read(ref str, ks) > 0)
                {
                    if (Str2Key(ref key, str) > 0)
                    {
                        mgesfun.mkeys[i] = key;
                    }
                }
            }
            //
            int d = 0;
            if (ConfigReadWrite.readInt(ref d, "ismodifier") > 0)
            {
                mgesfun.mismodifier = d;
            }
            if (ConfigReadWrite.read(ref str, "modifier") > 0)
            {
                if (str.Equals("ctl"))
                {
                    mgesfun.mmodifier = Keys.Control;
                }
                else if(str.Equals("alt"))
                    mgesfun.mmodifier=Keys.Alt;
            }

            // Gesture;

            return 0;
        }

        // Gesture;
        public string GetGesturePrm(int idx)
        {
            string str = "";
            switch (idx)
            {
                case 1:     // velocity min;
                    str = mgesture.mvelocitymin.ToString();
                    break;
                case 2:     // gap time;
                    str = mgesture.mgesgap.ToString();
                    break;
                case 3:     // overtime;
                    str = mgesture.mgesduration.ToString();
                    break;
                case 4:     // distance min;
                    str = mgesture.mdistancemin.ToString();
                    break;
                case 5:     // ;
                    str = mgesture.mgestrystopmax.ToString();
                    break;
                default:
                    break;
            }
            return str;
        }
        public void SetGesturePrm(string str, int idx)
        {
            switch (idx)
            {
                case 1:     // velocity min;
                    mgesture.mvelocitymin = Int32.Parse(str);
                    break;
                case 2:     // gap time;
                    mgesture.mgesgap = Int32.Parse(str);
                    break;
                case 3:     // overtime;
                    mgesture.mgesduration = Int32.Parse(str);
                    break;
                case 4:     // distance min;
                    mgesture.mdistancemin = Int32.Parse(str);
                    break;
                case 5:     // ;
                    mgesture.mgestrystopmax = Int32.Parse(str);
                    break;
                default:
                    break;
            }
        }







        private int Str2Key(ref Keys key, string str)
        {
            if (str.Length == 1)
            {
                char[] cs=str.ToCharArray();
                key = (Keys)cs[0];
                return 1;
            }
            return 0;
        }
    }
}
