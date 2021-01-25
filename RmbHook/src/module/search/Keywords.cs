using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace KeyMouseDo.src.keyword
{
    public class Keywords
    {
        public static Keywords mthis = new Keywords();
        // 
        int keywordminlen = 3;

        // common words;
        Hashtable comwords = new Hashtable();   // <string, string>;
        HashSet<string> comwordset = new HashSet<string>();

        public Keywords()
        {
            initCommonWords();
        }

        void initCommonWords()
        {
            comwords.Add("what", "");
            comwords.Add("when", "");
            comwords.Add("where", "");
            comwords.Add("why", "");
            comwords.Add("how", "");
            comwords.Add("a", "");
            comwords.Add("an", "");
            comwords.Add("the", "");
            comwords.Add("of", "");
            comwords.Add("in", "");
            comwords.Add("on", "");
            comwords.Add("to", "");
            comwords.Add("from", "");
            comwords.Add("above", "");
            comwords.Add("below", "");
            comwords.Add("about", "");

            comwordset.Add("what");
        }

        public HashSet<string> getKeywords(string phrase)
        {
            HashSet<string> keywds = new HashSet<string>();

            string[] strs = phrase.Split(
                new string[] { ",", ".", " ", "*", "?", ":", ";", "-", "/" },
                StringSplitOptions.RemoveEmptyEntries); //不保留空元素

            string s;
            for (int i = 0; i < strs.Length; i++)
            {
                // remote unnessary common words, which are stored in a harshmap;
                s = strs[i];
                if (s.Length < 3)
                    continue;
                if (comwords.Contains(s))
                    continue;

                // add
                if (!keywds.Contains(s))
                {
                    keywds.Add(s);
                }
            }

            return keywds;
        }
    }
}
