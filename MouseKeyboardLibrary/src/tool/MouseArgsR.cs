using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseKeyboardLibrary
{
    public class MouseArgsR : EventArgs
    {
        public bool ishandled = false;
        public int X = 0;
        public int Y = 0;

        public MouseArgsR()
        {

        }
        public MouseArgsR(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
