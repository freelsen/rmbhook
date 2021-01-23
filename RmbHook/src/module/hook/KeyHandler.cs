using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace RmbHook
{
    class KeyHandler
    {
        public static void SentKeyMof(Keys key1, Keys key2)
        {
            KeyboardSimulator.KeyDown(key1);
            KeyboardSimulator.KeyPress(key2);
            KeyboardSimulator.KeyUp(key1);

        }

        public static void SentString(string str)
        {
            Keys[,] keys = null;
            if (Str2Keys(out keys, str) > 0)
            {
                for (int i = 0; i < keys.GetLength(0); i++)
                {
                    if (keys[i, 1] != Keys.None)
                    {
                        SentKeyMof(keys[i,1],keys[i,0]);
                    }
                    else
                        KeyboardSimulator.KeyPress(keys[i,0]);
                }
            }
        }

        public static int Str2Keys(out Keys[,] keys, string str)
        {
           keys = null;
            if (str.Length > 0)
            {
                keys = new Keys[str.Length,2];
                char[] cs = str.ToCharArray();
                for (int i = 0; i < str.Length; i++)
                {
                    keys[i, 1] = Keys.None;

                    if (cs[i].Equals('-'))
                        keys[i,0] = Keys.OemMinus;
                    else if (cs[i].Equals(':'))
                    {
                        keys[i, 0] = Keys.Oem1;
                        keys[i, 1] = Keys.LShiftKey;
                    }
                    else
                        keys[i, 0] = Char2Key(cs[i]);
                }
                return 1;
            }
            else
                return -1;
        }
        public static Keys Char2Key(char c)
        {
            //if (str.Length == 1)
            //{
                //char[] cs = str.ToCharArray();
                return (Keys)c;
        }
    }
}
