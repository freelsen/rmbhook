using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrittingHelper.wow
{
    class RogueAction
    {
        public Action<ConsoleKey> pressKey;

        public RogueAction()
        {
            initKeys();
            InitConsoleKeys();
        }

        public void DoAction(int idx)
        {
            //KeyHelper.SentKeyMof(mactionkeys[idx, 0], mactionkeys[idx, 1]);
            this.pressKey(makeys[idx]);
        }
        public void DoAction(EactionRogue action)
        {
            int idx = (int)action;
            this.DoAction(idx);
        }

        Keys[,] mactionkeys = new Keys[100, 3];
       
        void initKeys()
        {
            for (int i = 0; i < 100; i++)
            {
                mactionkeys[i, 0] = Keys.None;
                mactionkeys[i, 1] = Keys.None;
            }

            int idx;
            idx = (int)EactionRogue.jump;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
            idx = (int)EactionRogue.hide;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D8;
            idx = (int)EactionRogue.sinister;   mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D2;
            idx = (int)EactionRogue.eviscerate; mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D3;
            idx = (int)EactionRogue.auto;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D9;
            idx = (int)EactionRogue.backstab;   mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D4;
            idx = (int)EactionRogue.slice;      mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D5;
            idx = (int)EactionRogue.heal;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.D0;
            //idx = (int)EactionRogue.jump;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
            //idx = (int)EactionRogue.jump;       mactionkeys[idx, 0] = Keys.Control; mactionkeys[idx, 1] = Keys.Space;
        }


        // 
        ConsoleKey[] makeys = new ConsoleKey[100];
        void InitConsoleKeys()
        {
            for (int i=0; i<makeys.Length; i++)
            {
                makeys[i] = ConsoleKey.NoName;
            }

            int idx;
            idx = (int)EactionRogue.jump;           makeys[idx] = ConsoleKey.Spacebar;
            idx = (int)EactionRogue.hide;           makeys[idx] = ConsoleKey.C;
            idx = (int)EactionRogue.sinister;       makeys[idx] = ConsoleKey.Q;
            idx = (int)EactionRogue.eviscerate;     makeys[idx] = ConsoleKey.E;
            idx = (int)EactionRogue.auto;           makeys[idx] = ConsoleKey.D5;
            idx = (int)EactionRogue.backstab;       makeys[idx] = ConsoleKey.D2;
            idx = (int)EactionRogue.slice;          makeys[idx] = ConsoleKey.D3;
            idx = (int)EactionRogue.heal;           makeys[idx] = ConsoleKey.Z;

        }

       



    }
}
