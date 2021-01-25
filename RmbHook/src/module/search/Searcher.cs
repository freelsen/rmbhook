using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace KeyMouseDo.src.keyword
{
    public class Searcher
    {
        public Searcher()
        {
        }
        public static ArrayList search(string key, string setence)
        {
            string str = "(\\w*)" + key + "(\\w*)";
            Regex regex = new Regex(str, RegexOptions.IgnoreCase);
            MatchCollection mc = regex.Matches(setence);

            ArrayList als = new ArrayList();
            foreach (Match mt in mc)
            {
                //Console.WriteLine(mt.Value);
                als.Add(mt.Value);

            }
            return als;
        }
    }
}
