using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW.wow
{
    class Botmove
    {
        public WowProcess _wowprocess = null;

        bool misup = false;
        bool misdown = false;
        public void Up()
        {
            misup = !misup;
            this._wowprocess.SetKeyState(ConsoleKey.UpArrow, misup, false, "");
        }

        public void Down()
        {
            misdown = !misdown;
            this._wowprocess.SetKeyState(ConsoleKey.DownArrow, misdown, false, "");
        }


        Point _tourstart = new Point(0, 0);
        int _tourradius = 10;
        public void TourAround(int x, int y)
        {

        }
    }
}
