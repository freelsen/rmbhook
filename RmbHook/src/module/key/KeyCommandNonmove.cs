using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrittingHelper.src.keyword;
using WrittingHelper.quicknote;

namespace WrittingHelper
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
        void ShowQuickNote()
        {
            QuickNoteMan.mthis._qnevent.ShowWindow();
        }


        public bool doNonMovingCmd(Keys k, ref bool iskeepcmd)
        {

            //bool iskeepcmd = false;  // 2021-01-23;
            iskeepcmd = true;
            bool ishindle = false;

            // check function group-1,
            if (k==Keys.G)
            {
                iskeepcmd = false;
                ishindle = true;
            }
            else if (k==Keys.H)  // quick note window;
            {
                this.ShowQuickNote();
                iskeepcmd = false;
                ishindle = true;
            }
            //else if (k==Keys.N)   // distable this, too dangeous!! 2021-01-23;
            //{
            //    QuickNoteMan.mthis.ResetWindow();
            //    return false;
            //}
            else if (k==Keys.Y)  //.T) // date & time string;
            {
                DateTime dt = DateTime.Now;
                string str = dt.ToString("yyyy-MM-dd");
                Console.WriteLine(str);
                KeyHelper.SentString(str);

                iskeepcmd = true;
                ishindle = true;
            }
            else if (k==Keys.P) //.Y) 2021-02-19,
            {
                DateTime dt1 = DateTime.Now;
                string str1 = dt1.ToString("hh:mm tt");
                //Console.WriteLine(str);
                KeyHelper.SentString(str1);

                iskeepcmd = true;
                ishindle = true;
            }
            else if (menWindow)      // window management, 20150621;
            {
                //if (k==Keys.V)//.B)           // 
                //{
                //        WinMon.mthis.stBottom();

                //    iskeepcmd = false;
                //    ishindle = true;
                //}
                //else if (k==Keys.C)//.V)     //.C)
                //{
                //        WinMon.mthis.restore();
                //    iskeepcmd = false;
                //    ishindle = true;
                //}
                //else 
                if (k== Keys.B)//.N)    //.V)
                {
                        WinMon.mthis.minNow();
                    iskeepcmd = false;
                    ishindle = true;
                }
            }
            else if (mensearch)      // search function;
            {
                if (k== Keys.P)            // 20160503, keyword search;
                {
                    LsKeyword kw = LsKeyword.getThis();
                    kw.onSearchKey(true);
                    iskeepcmd = false;
                    ishindle = true;
                }
                else if (k== Keys.M)//2021-01-22;N:
                {
                    LsKeyword kw2 = LsKeyword.getThis();
                    kw2.onSearchKey(false);
                    iskeepcmd = false;
                    ishindle = true;
                }
            }

            return ishindle;
        }


    }
}
