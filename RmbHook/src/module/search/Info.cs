using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper.src.keyword
{
    public class Info
    {
        static public void dinfo(string s)
        {
            System.Console.Write("->");
            System.Console.WriteLine(s);
        }
        static public void derror(string s)
        {
            System.Console.Write("->ERROR: ");
            System.Console.WriteLine(s);
        }

    }
}
