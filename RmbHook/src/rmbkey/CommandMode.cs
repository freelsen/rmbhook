using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Collections;

namespace RmbHook
{
    public class CommandMode
    {
        private Hashtable mkeymap = new Hashtable();

        public CommandMode()
        {
        }
        public int init()
        {
            mkeymap.Add(Keys.I, Keys.Up);
            mkeymap.Add(Keys.K, Keys.Down);
            mkeymap.Add(Keys.J, Keys.Left);
            mkeymap.Add(Keys.L, Keys.Right);
            mkeymap.Add(Keys.U, Keys.Home);
            mkeymap.Add(Keys.O, Keys.End);

            return 0;
        }
        public int onKey(Keys key)
        {
            int eatkey = 0;
            if( mkeymap.ContainsKey(key) )
            {
                Keys kmap = (Keys)mkeymap[key];
                sendKey(kmap);
                eatkey = 1;
            }
            return eatkey;
        }
        private void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
    }
}
