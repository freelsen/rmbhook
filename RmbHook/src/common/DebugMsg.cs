﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper
{
    class Lslog
    {
        public static void log(string str)
        {
#if DEBUG
            Console.WriteLine(str);
#endif
        }
    }
}
