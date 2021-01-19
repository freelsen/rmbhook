using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace RmbHook
{
    public class GesFun                         // 140717;
    {
        private WinMon mwinmon = null;

        int mpgidx = 0;

        public static int mkeynum=64;
        public Keys[] mkeys = new Keys[mkeynum];
        Keys[] mkeys1 = new Keys[mkeynum];

        public GesFun()
        {
            mkeys[0] = Keys.E;  // right;
            mkeys[1] = Keys.D3;  // right up;
            mkeys[2] = Keys.D2; // up;
            mkeys[3] = Keys.D1; // up left;
            mkeys[4] = Keys.Q;  // left;
            mkeys[5] = Keys.C;  // left down;
            mkeys[6] = Keys.X;  // down
            mkeys[7] = Keys.F;  // down righg;

            mkeys1[0] = Keys.E;  // right;
            mkeys1[1] = Keys.D3;  // right up;
            mkeys1[2] = Keys.D2; // up;
            mkeys1[3] = Keys.D1; // up left;
            mkeys1[4] = Keys.Q;  // left;
            mkeys1[5] = Keys.Z;  // left down;
            mkeys1[6] = Keys.X;  // down
            mkeys1[7] = Keys.C;  // down righg;

            // initializa;
            for (int i = 8; i < mkeynum; i++)
            {
                mkeys[i] = Keys.D0;
                mkeys1[i] = Keys.D0;
            }
        }

        public int linit()
        {
            return 0;
        }
        public void start()
        {
            //Parameter prm = Parameter.mthis;
            //mpgidx = prm.mpage;

            mwinmon = WinMon.mthis;
        }
        public void onGesture(int idx)
        {
            if (mpgidx == 0)
            {
                //if (idx == 5)
                //{
                //    KeyboardSimulator.KeyDown(Keys.Alt);
                //    KeyboardSimulator.KeyPress(Keys.D4);
                //    KeyboardSimulator.KeyUp(Keys.Alt);
                //    return;
                //}
                sendKey( mkeys, idx);
                
            }
            else if (mpgidx == 1)
            {
                sendKey(mkeys1, idx);
            }
            else if (mpgidx == 2)
            {
                sendWin( idx );
            }
        }
        void sendWin( int idx)
        {
            if (idx == 1)
            {
                mwinmon.restore();
            }
            else if (idx == 2)
            {
                sendKey(Keys.Home);
            }
            else if (idx == 5)
            {
                mwinmon.stBottom();
            }
            else if (idx == 6)
            {
                sendKey(Keys.End);
            }
            else if (idx == 7)
            {
                mwinmon.minNow();
            }
        }
        
        void sendKey(Keys [] keys, int idx)
        {
            sendKey(keys[idx]);
        }
        void sendKey(Keys key)
        {
            KeyboardSimulator.KeyDown(key);
            KeyboardSimulator.KeyUp(key);
        }
    }
}
