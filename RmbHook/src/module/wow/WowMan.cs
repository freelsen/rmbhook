using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WrittingHelper.wow;

namespace WrittingHelper
{
    class WowMan
    {
        public static WowMan mthis = null;
        public DrawFormMan mdfman = null;
        public HookEventHandler mhookhandler = null;

        //public FormEvent _formevent = new FormEvent();

        //        public D2c md2c = new D2c();    // 2021-02-24;
        public WowEvent _wowevent = new WowEvent();
        public WowCmd mwowcmd = new WowCmd();
        public RogueTwo _roguetwo = new RogueTwo();
        public WowRoguePixelOne _pixelone = new WowRoguePixelOne();

        public ColorGrids _colorgrids = new ColorGrids();
        public WowProcess _wowprocess = new WowProcess();
        public ColorFetcher _colorfetcher = new ColorFetcher();
        public RogueAction _wowrogueaction = new RogueAction();
        public DrawClient _drawclient = new DrawClient();
        public Dw3by3 mdw3by3 = new Dw3by3();
        public DwGraph mdwgraph = new DwGraph();

        public WowWorker mworker = new WowWorker();
        public WowProc _botpump = new WowProc();
        public LootWheel _lootwheel = new LootWheel();
        public Botmove _botmove = new Botmove();
        public Wpos _wpos = new Wpos();

        public WowMan()
        {
            mthis = this;
        }

        public int init()
        {
            Assemble();

            this._drawclient._colorgrids = this._colorgrids;
            this._drawclient.mdw3by3 = this.mdw3by3;
            this._drawclient.mdwgraph = this.mdwgraph;
            this._drawclient._pixelone = this._pixelone;
            this._drawclient._loot = this._lootwheel;

            this._wowevent.mwowcmd = this.mwowcmd;
            this._wowevent.mdfman = this.mdfman;
            this._wowevent.mdw3by3 = this.mdw3by3;
            this._wowevent._colorgrids = this._colorgrids;
            this._wowevent._wowroguetwo = this._roguetwo;
            this._wowevent._botpump = this._botpump;
            this._wowevent._wowprocess = this._wowprocess;
            this._wowevent._botmove = this._botmove;
            this._wowevent._wpos = this._wpos;

            this._lootwheel._wowProcess = this._wowprocess;

            this._botmove._wowprocess = this._wowprocess;
            

            this._wowrogueaction.pressKey = this._wowprocess.KeyPressSleep;

            return 0;
        }

        public void Assemble()
        {
            //this._colorfetcher.getHwnd = this._wowprocess.GetHwnd;
            this._colorfetcher.getHwnd = mdfman._wowwin.GetHwnd;
            this._colorgrids.getColorClient = this._colorfetcher.getColorClient;

            this._roguetwo.getVal = this._colorgrids.GetVal;
            this._roguetwo.doAction = this._wowrogueaction.DoAction;
            this._roguetwo.doLoot = this._lootwheel.DoLoot;
            this._roguetwo.onPos = this._wpos.AddPos;

            mhookhandler.onLmouseDown += this._wowevent.onLmouseDown;
            mhookhandler.onRmouseDown += this._wowevent.onRmouseDown;
            mhookhandler.onLmouseDouble += this._wowevent.onLmouseDouble;
            mhookhandler.onRmouseDouble += this._wowevent.onRmouseDouble;

            mdfman._formclientevent.onSizeChanged = this._drawclient.OnSizeChanged;
            mdfman._formclientevent.onPaint = this._drawclient.OnPaint;

            this._lootwheel.getLootPos = this._pixelone.GetLootPos;
            this._lootwheel.getStopPos = this._pixelone.GetLootstopPos;

            //mdfman.mdrawform.onTimer1 = this._wowevent.onTimer;
            this._botpump.runProc = this._wowevent.OnLoop;
            

        }







        // -- thread;
        void setWorker()
        {
            DrawForm df = mdfman.mdrawform;
            if (df.mwork1do == null)
            {
                df.mwork1do = WowWorker.doWork;
                df.mwork1progress = WowWorker.reportWork;
                df.mwork1completed = WowWorker.doneWork;
            }
        }
        void startWorker()
        {
            setWorker();
            BackgroundWorker work = mdfman.mdrawform.Work1;
            if (!work.IsBusy)
            {
                work.RunWorkerAsync();
            }
        }
        void stopWorker()
        {
            mworker.Enable = false;
        }

        




    }
}
