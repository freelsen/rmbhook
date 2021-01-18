using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RmbHook
{
    public class LISortedList: IComparer
    {
        public int Compare(object x, object y)
        {
            // return -1; // not sorted;

            // sorted;
            int rt = -((int)x - (int)y); // decrese;

            if (rt == 0) 
                rt = -1;

            return rt;

        }
    }
}
