using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;

namespace WrittingHelper.src.keyword
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
        public SearchForm getSearchForm() { return searchform; }

        private PathAnalyser pathanalyser = new PathAnalyser();
        public PathAnalyser getPathAnalyser() { return pathanalyser; }
        
        private PathFile pathfile = new PathFile();
        public PathFile getPathfile() { return pathfile; }

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
                searchform.setSearchFocus();
            }
            else
            {
                searchform.Hide();
            }
        }

        public void saveFile()
        {
            ICollection ic = pathanalyser.getPaths();
            string pf = pathfile.getPathfile();
            pf = pf + ".new";
            pathfile.saveFile(ic, pf);

        }
        public void saveFiletable()
        {
            Hashtable ic = pathanalyser.getPathtable();
            string pf = pathfile.getPathfile();
            //pf = pf + ".new";
            pathfile.saveFile(ic, pf);
        }
        
    }
}
