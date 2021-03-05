using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;
using System.Threading;

namespace WrittingHelper
{
    class KeyHelper
    {
        public static bool miskeybusy = false;  // 2021-02-14,
        static bool setBusy()
        {
            bool busy = miskeybusy;  // record old status;
            if (!miskeybusy)
            {
                miskeybusy = true;
            }
            return busy;
        }
        static void restoreBusy(bool busy)
        {
            if (miskeybusy!=busy)
                miskeybusy = busy;
        }


        public static void SentKeyMof(Keys key1, Keys key2)
        {
            bool busy = setBusy();

            if (key1 != Keys.None) KeyboardSimulator.KeyDown(key1);
            KeyboardSimulator.KeyPress(key2);
            if (key1 != Keys.None) KeyboardSimulator.KeyUp(key1);

            restoreBusy(busy);
        }


        public static void SentString(string str)
        {
            bool busy = setBusy();

            Keys[,] akeys = null;
            if (Str2Keys(out akeys, str) > 0)
            {
                for (int i = 0; i < akeys.GetLength(0); i++)
                {
                    //Console.WriteLine(akeys[i, 0].ToString());
                    if (akeys[i, 1] != Keys.None)
                    {
                        SentKeyMof(akeys[i, 1], akeys[i, 0]);
                        //KeyboardSimulator.KeyDown(Keys.A);
                        //KeyboardSimulator.KeyPress(akeys[i, 0]);
                        //KeyboardSimulator.KeyUp(Keys.A);
                    }
                    else
                    {
                        KeyboardSimulator.KeyPress(akeys[i, 0]);
                        //Thread.Sleep(1);
                    }
                }
            }

            restoreBusy(busy);
        }



        public static int Str2Keys(out Keys[,] akeys, string str)
        {
            akeys = null;
            if (str.Length > 0)
            {
                akeys = new Keys[str.Length, 4];
                char[] cs = str.ToCharArray();
                for (int i = 0; i < str.Length; i++)
                {
                    Keys[] ak = ConvertCharToVirtualKey(cs[i]);
                    for(int j=0;j<4;j++)
                        akeys[i, j] = ak[j];

                }
                return 1;
            }
            else
                return -1;
        }

        public static Keys[] ConvertCharToVirtualKey(char ch)
        {
            Keys[] akeys = new Keys[4];

            short vkey = VkKeyScan(ch);
            Keys retval = (Keys)(vkey & 0xff);

            int modifiers = vkey >> 8;

            akeys[0] = retval;
            if ((modifiers & 1) != 0)
            {
                //retval |= Keys.Shift;
                akeys[1] = Keys.Shift;
            }
            if ((modifiers & 2) != 0)
            {
                //retval |= Keys.Control;
                akeys[2] = Keys.Control;
            }

            if ((modifiers & 4) != 0)
            {
                //retval |= Keys.Alt;
                akeys[3] = Keys.Alt;
            }

            return akeys;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        public static int Str2Keys2(out Keys[,] keys, string str)
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
