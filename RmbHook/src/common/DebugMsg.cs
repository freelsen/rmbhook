using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class DbMsg
    {
        public static void Msg(string str)
        {
#if DEBUG
            Console.WriteLine(str);
#endif
        }
    }
}
