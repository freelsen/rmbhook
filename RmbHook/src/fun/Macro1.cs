using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyMouseDo
{
    class Macro1
    {
        public Keys[,] _keylist1 = new Keys[10, 2];
        public Macro1()
        {
            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D3;
            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D4;
            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D5;
            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D6;
            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D7;
        }
    }
}
