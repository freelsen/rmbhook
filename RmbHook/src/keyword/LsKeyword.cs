using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RmbHook.src.keyword
{
    public class LsKeyword
    {
        private static LsKeyword mthis;
        public static LsKeyword getThis()
        {
            if (mthis == null) mthis = new LsKeyword();
            return mthis;
        }

        private SearchForm searchform;

        public LsKeyword()
        {
            searchform = new SearchForm();
            searchform.Hide();

        }
        public void exit()
        {
            searchform.setExit(true);
        }
               
        public void onSearchKey(bool b)
        {
            if (b)
            {
                searchform.Show();
                searchform.Activate();
            }
            else
            {
                searchform.Hide();
            }
        }

    }
}
