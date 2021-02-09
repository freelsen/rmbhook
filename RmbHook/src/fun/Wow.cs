using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MouseKeyboardLibrary;
using System.Windows.Forms;
using System.Threading;

// 2021-02-08,
// f> to make the game automatic, by process key, mouse event,
namespace KeyMouseDo
{
    class Wow
    {
        public Wow()
        {
            initMacroc1();
        }

        // mouse event 1,
        // start strategy,
        bool _ismacrostart = true; // right double click to start/stop;
        public void pushMacrobtn()
        {
            _ismacrostart=!_ismacrostart;
            //if (!_ismacrostart)
                KeyboardSimulator.KeyPress(Keys.Space);
        }

        Macro1 _macro1 = new Macro1();
        Keys[,] _keylist1 = new Keys[10, 2];
        int[,] _mac1 = new int[4, 10];
        void initMacroc1()
        {
            _mac1[0, 0] = 2579; _mac1[0, 1] = 982;
            _mac1[0, 2] = 255; _mac1[0, 3] = 21; _mac1[0, 4] = 21;

            _mac1[1, 0] = 3302; _mac1[1, 1] = 982;
            _mac1[1, 2] = 229; _mac1[1, 3] = 110; _mac1[0, 4] = 144;

            _keylist1[0, 0] = Keys.Control; _keylist1[0, 1] = Keys.D5;
            _keylist1[1, 0] = Keys.Control; _keylist1[1, 1] = Keys.D6;
            _keylist1[2, 0] = Keys.Control; _keylist1[2, 1] = Keys.D7;
            _keylist1[3, 0] = Keys.Control; _keylist1[3, 1] = Keys.D8;
            _keylist1[4, 0] = Keys.Control; _keylist1[4, 1] = Keys.D9;
        }

        public void doMacro()
        {
            if (!_ismacrostart) return;

            for (int i = 0; i < 5; i++)
            {
                KeyHandler.SentKeyMof(_keylist1[i, 0], _keylist1[i, 1]);
                //Thread.Sleep(10);
            }
        }
        public void doMacro1()
        {
            // send command by sequence,
            // judge which is avaiable by fetch color at pecific position,
            
            //Color c = FetchColor.gtColor(_mac1[0,0],_mac1[0,1]);
            //Console.Out.WriteLine(c.R.ToString()+"," + c.G.ToString() +","+ c.B.ToString());
            //mForm.ShowColor(c);
            //string str = String.Format("Mouse:({0},{1}),color({2},{3},{4}", e.X, e.Y, c.R, c.G, c.B);
            //mForm.setMouseLabe(str);

            //if (c.R > _mac1[0, 2] - 50)
            //{
            //    KeyboardSimulator.KeyDown(Keys.Control);
            //    KeyboardSimulator.KeyPress(Keys.D3);
            //    KeyboardSimulator.KeyUp(Keys.Control);
            //}

            Color c;
            for (int i = 0; i < 2; i++)
            {
                c = FetchColor.gtColor(_mac1[i, 0], _mac1[i, 1]);
                if (c.R > _mac1[i, 2] - 50)
                {
                    //KeyboardSimulator.KeyDown(Keys.Control);
                    //KeyboardSimulator.KeyPress(Keys.D3);
                    //KeyboardSimulator.KeyUp(Keys.Control);
                    KeyHandler.SentKeyMof(_keylist1[i, 0], _keylist1[i, 1]);
                }
            }
        }
        

    }
}
