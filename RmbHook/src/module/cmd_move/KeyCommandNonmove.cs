using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RmbHook.src.keyword;

namespace RmbHook
{
    class KeyCommandNonmove
    {
        public bool menWindow = false; // 2020-04-24;
        public bool mensearch = false;

        public KeyCommandNonmove()
        {
        }
        public int init()
        {
            return 0;
        }

        public bool doNonMovingCmd(Keys k)
        {

            bool iskeepcmd = true;  // 2021-01-23;

            // check function group-1,
            if (k==Keys.G)
            {
                return false;
            }
            
            // quick note window;
            if (k==Keys.H)
            {
                QuickNoteMan.mthis.ShowWindow();
                return false;
            }
            else if (k==Keys.N)
            {
                QuickNoteMan.mthis.ResetWindow();
                return false;
            }
            
            // date & time string;
            if (k==Keys.T)
            {
                DateTime dt = DateTime.Now;
                string str = dt.ToString("yyyy-MM-dd");
                Console.WriteLine(str);
                KeyHandler.SentString(str);

                return false;
            }
            else if (k==Keys.Y)
            {
                DateTime dt1 = DateTime.Now;
                string str1 = dt1.ToString("hh:mm tt");
                //Console.WriteLine(str);
                KeyHandler.SentString(str1);
                return false;
            }

            // window management, 20150621;
            if (menWindow)
            {
                if (k==Keys.B)           // 
                {
                        WinMon.mthis.stBottom();

                        return false;
                }
                else if (k==Keys.C)
                {
                        WinMon.mthis.restore();

                   return false;
                }
                else if (k== Keys.V)
                {
                        WinMon.mthis.minNow();
                        return false;
                }
            }

            // search function;
            if (mensearch)
            {
                if (k== Keys.P)            // 20160503, keyword search;
                {
                    LsKeyword kw = LsKeyword.getThis();
                    kw.onSearchKey(true);

                    return false;
                }
                else if (k== Keys.M)//2021-01-22;N:
                {
                    LsKeyword kw2 = LsKeyword.getThis();
                    kw2.onSearchKey(false);

                    return false;
                }
            }

            return iskeepcmd;
        }
    }
}
