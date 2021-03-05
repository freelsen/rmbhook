using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper.src.keyword
{
    public class PathInfo
    {
        public int linenum = -1;
        public HashSet<string> keywords = new HashSet<string>();
        public bool isdelete = false;
    }
}
