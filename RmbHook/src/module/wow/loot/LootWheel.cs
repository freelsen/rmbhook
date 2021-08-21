using Libsw;
using Microsoft.Extensions.Logging;
using System;
using System.Drawing;
using System.Threading.Tasks;
using WrittingHelper.libs;

namespace WrittingHelper.wow
{
    class LootWheel
    {
        public Func<Point[]> getLootPos;
        public Func<Point> getStopPos;

        public WowProcess _wowProcess;
        //private readonly PlayerReader playerReader;             
        //private ILogger logger;
        
        
        

        bool misbusy = false;
        public async void DoLoot()
        {
            if (misbusy)
            {
                Lslog.log("loot busy");
                return;
           }
            await Task.Run(this.Loot);
        }

        public CursorClassification Classification { get; set; }
        public void Loot()
        {            
            misbusy = true;
            
            if(this.SearchLoot())
            { 
                Point mousePosition = new Point(0, 0);
                WinApis.GetCursorPos(ref mousePosition);
                _wowProcess.RightClickMouseSleep(mousePosition);
                //Classification = cls;

                doLoot();

                // back to the ? position;
                mousePosition = this.getStopPos();
                this._wowProcess.LeftClickMouseClient(mousePosition);

                //WinApis.ClientToScreen(_wowProcess.GetHwnd(), ref mousePosition);
                //NativeMethods.SetCursorPos(mousePosition.X, mousePosition.Y);
                //wowProcess.RightClickMouseSleep(mousePosition);
            }
            misbusy = false;
        }
        bool SearchLoot()
        {
            bool isfound = false;

            // change locations;
            for (int r = 0; r < _gdrow; r++)
            {
                for (int c = 0; c < _gdcol; c++)
                {
                    Point point = this._gdcenter[r, c];
                    this._wowProcess.SetCursorPosClientDelay(point);

                    isfound = CheckCursor();

                    if (isfound)
                        return isfound;
                }
            }

            return isfound;
        }
        bool CheckCursor()
        {
            bool isfound = false;

            Classification = CursorClassification.None;
            CursorClassifier.Classify(out var cls).Dispose();

            if (cls == CursorClassification.Vendor) //(cls == CursorClassification.Loot)
            {
                Lslog.log("Found: " + cls.ToString());
                isfound = true;
            }
            return isfound;
        }
        

        void doLoot()
        {
            Lslog.log("do loot");

            for (int i = 0; i < 4; i++)
            {
                Point pt = new Point(0, 0);// = mpositions[idx, 1];
                pt = this.getLootPos()[i];
                _wowProcess.LeftClickMouseClient(pt);
            }
            
        }


        int _gdwid = 40;
        int _gdhig = 40;
        static int _gdrow = 4;
        static int _gdcol = 6;
        Rectangle[,] _lootgrids = new Rectangle[_gdrow, _gdcol];
        Point[,] _gdcenter;

        void InitGrid()
        {
            Wgrids.InitGrids(this._lootgrids, _gdrow, _gdcol);
            OnSize(0, 0);
        }

        public void OnSize(int cx, int cy)
        {
            Lslog.log($"client center=({cx.ToString()},{cy.ToString()})");
            Wgrids.SetGridsCenter(_lootgrids, cx, cy, _gdwid, _gdhig, _gdrow, _gdcol);
            _gdcenter = Wgrids.GetGridCenter(_lootgrids, _gdrow, _gdcol);
        }









        private Point lastLootFoundAt;
        private readonly float num_theta = 32;
        private readonly float radiusLarge;
        private readonly float dtheta;
        private readonly Point centre;











        /*
        public LootWheel(WowProcess wowProcess, PlayerReader playerReader, ILogger logger)
        {
            this.wowProcess = wowProcess;
            this.playerReader = playerReader;
            this.logger = logger;

            var rect = wowProcess.GetWindowRect();

            rect.right = wowProcess.ScaleDown(rect.right);
            rect.bottom = wowProcess.ScaleDown(rect.bottom);

            centre = new Point((int)(rect.right / 2f), (int)((rect.bottom / 5) * 3f));
            radiusLarge = rect.bottom / 6;
            dtheta = (float)(2 * Math.PI / num_theta);
        }
        */

        private readonly bool debug = true;
        private void Log(string text)
        {
            if (debug && !string.IsNullOrEmpty(text))
            {
                //Lslog.log($"{this.GetType().Name}: {text}");
            }
        }

        public void Reset()
        {
            lastLootFoundAt = new Point(0, 0);
        }

        public async Task<bool> Loot(bool searchForMobs)
        {
            WowProcess.SetCursorPosition(new Point(this.lastLootFoundAt.X + 200, this.lastLootFoundAt.Y + 120));
            await Task.Delay(150);

            //if (!searchForMobs)
            //{
            //    WowProcess.SetCursorPosition(this.lastLootFoundAt);
            //    await Task.Delay(200);
            //}

            if (await CheckForLoot(this.lastLootFoundAt, searchForMobs, false))
            {
                Lslog.log($"Loot at {this.lastLootFoundAt.X},{this.lastLootFoundAt.Y}");
                return true;
            }
            else
            {
                Lslog.log($"No loot at {this.lastLootFoundAt.X},{this.lastLootFoundAt.Y}");
            }

            if (!searchForMobs)
            {
                if (await SearchInCircle(radiusLarge / 2, radiusLarge / 2, false, centre, false))
                {
                    return true;
                }
            }

            if (await SearchInCircle(radiusLarge, radiusLarge, searchForMobs, centre, false))
            {
                return true;
            }

            if (searchForMobs && lastLootFoundAt.X!=0)
            {
                await CheckForLoot(lastLootFoundAt, false, true);
            }

            return false;
        }

        private async Task<bool> SearchInCircle(float rx, float ry, bool searchForMobs, Point circleCentre, bool ignoreMobs)
        {
            float theta = 0;
            for (int i = 0; i < num_theta; i++)
            {
                float x = (float)(circleCentre.X + rx * Math.Cos(theta));
                float y = (float)(circleCentre.Y + (ry * Math.Sin(theta)));
                var mousePosition = new Point((int)x, (int)y);

                WowProcess.SetCursorPosition(mousePosition);

                if (await CheckForLoot(mousePosition, searchForMobs, ignoreMobs))
                {
                    return true;
                }

                theta += dtheta;
            }

            return false;
        }

        

        private async Task<bool> CheckForLoot(Point mousePosition, bool searchForMobs, bool ignoreMobs)
        {
            var inCombat = false;// this.playerReader.PlayerBitValues.PlayerInCombat;

            Classification = CursorClassification.None;
            await Task.Delay(30);

            CursorClassifier.Classify(out var cls).Dispose();

            if (searchForMobs)
            {
                if (cls == CursorClassification.Kill)
                {
                    await Task.Delay(100);
                    CursorClassifier.Classify(out cls).Dispose();
                }
            }
            else
            {
                // found something, lets give the cursor a chance to update.
                if (cls == CursorClassification.Loot || cls == CursorClassification.Kill || cls == CursorClassification.Skin)
                {
                    await Task.Delay(200);
                    CursorClassifier.Classify(out cls).Dispose();
                }
            }

            if (cls == CursorClassification.Loot && !searchForMobs)
            {
                Log("Found: " + cls.ToString());
                await _wowProcess.RightClickMouse(mousePosition);
                Classification = cls;
                await Task.Delay(500);
                await Wait(2000, inCombat);
            }

            if (cls == CursorClassification.Skin && !searchForMobs)
            {
                Log("Found: " + cls.ToString());
                await _wowProcess.RightClickMouse(mousePosition);
                Classification = cls;
                await Task.Delay(1000);
                await Wait(6000, inCombat);
            }

            if (cls == CursorClassification.Kill && !ignoreMobs)
            {
                Log("Found: " + cls.ToString());
                await _wowProcess.RightClickMouse(mousePosition);
                Classification = cls;
            }

            if (cls == CursorClassification.Loot || cls == CursorClassification.Skin)
            {
                lastLootFoundAt = mousePosition;
                Lslog.log($"Loot found at {this.lastLootFoundAt.X},{this.lastLootFoundAt.Y}");
            }

            if (searchForMobs)
            {
                return cls == CursorClassification.Kill;
            }

            return cls == CursorClassification.Loot || cls == CursorClassification.Skin || cls == CursorClassification.Kill;
        }

        private async Task Wait(int delay, bool isInCombat)
        {
            return;
            /*
            for (int i = 0; i < delay; i += 100)
            {
                if (!isInCombat && this.playerReader.PlayerBitValues.PlayerInCombat)
                {
                    Lslog.log("We have enterred combat, aborting loot");
                    return;
                }
                if (!this.playerReader.IsCasting)
                {
                    CursorClassifier.Classify(out var cls2).Dispose();
                    if (cls2 != this.Classification)
                    {
                        return;
                    }
                }
                await Task.Delay(100);
            }
            return;
            */
        }
    }
}