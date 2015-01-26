using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace RmbHook
{
    public class RmbKey
    {
        public static RmbKey gthis = null; 

        private TaskbarNotify mtasknotify = null;
        private CommandMode mcommandmode = new CommandMode();

        public RmbKey()
        {
            gthis = this;
        }

        public int init()
        {
            initKeyIcon();

            mtasknotify = TaskbarNotify.gthis;

            mcommandmode.init();

            setCmdEnable(true);

            return 1;
        }

        // ---key msg entry here;---
        public int onKeymsg(KeyEventArgs e)
        {
            int eatkey = 0;

            Keys key = e.KeyCode;

            if (key == Keys.Escape)         
            {
                // In any status( enable or disable ), if you tap the escape key 
                // for x(now x=5) times, the status will change;
                // thus give a keyboard way to enable/disable key hook function;
                if (checkEnable())
                    onCmdEnChange();
                else if( mcmden )
                {
                    // normally, when cmd mode is on, the escape key will be eated;
                    // but here give a chance: when you tap esc key twince, the last will be send to app(not eat);
                    // this is a must have function. or your apps will not receive esc key msg;
                    eatkey = checkEatEsc();
                    onModeChange();
                }               
            }
            else
            {
                resetCount();

                if (mcmdmode)
                {
                    eatkey = onCmdKey(key);    // eat this key, if need;
                }
            }

            return eatkey;
        }

        // ---mesccnt: repeat Escape key, 
        // ---mesccnt2: eat esc key or not when in cmd mode;
        private int mesccnt = 0;
        private int mesccnt2 = 0;
        private void resetCount()
        {
            mesccnt = 0;
            mesccnt2 = 0;
        }
        private void resetEsccnt2()
        {
            mesccnt2 = 0;
        }
        private bool checkEnable()
        {
            mesccnt++;
            if (mesccnt >= 5)
            {
                mesccnt = 0;

                return true;
            }
            return false;
        }
        private int checkEatEsc()
        {
            int eat = 0;
            if (mcmden)
            {
                if (mesccnt2 >= 2)
                    mesccnt2 = 0;
                mesccnt2++;

                if (mesccnt2 >= 2)
                    eat = 0;
                else
                    eat = 1;
            }
            return eat;
        }

        // ---mode enable;---
        private bool mcmden = false;
        private void onCmdEnChange()
        {
            setCmdEnable(!mcmden);
        }
        public void setCmdEnable(bool en)
        {
            if (en && !mcmden)
            {
                onCmdEnable();
            }
            else if (!en && mcmden)
                onCmdDisable();

            mcmden = en;

            updateIcon();
        }
        private void onCmdEnable()
        {
            resetCmdMode();

            resetEsccnt2();
        }
        private void onCmdDisable()
        {

        }

        // ---command mode; ---
        private bool mcmdmode = false;
        private void resetCmdMode()
        {
            mcmdmode = false;
        }
        private void onModeChange()
        {
            setCmdMode(!mcmdmode);
        }
        private void setCmdMode(bool b)
        {
            mcmdmode = b;

            updateIcon();

            if (mcmdmode)
                onCmdModeStart();
            else
                onCmdModeEnd();
        }
        private void onCmdModeStart()
        {

        }
        private void onCmdModeEnd()
        {

        }

        // ---on cmd keys; ---
        public int onCmdKey(Keys key)
        {
            int eatkey = 0;

            eatkey = mcommandmode.onKey(key);

            return eatkey;
        }

        // ---indicator icon ---
        private Icon micon2;
        private Icon micon3;
        private int initKeyIcon()
        {
            micon2 = new Icon("res\\icon2.ico");
            micon3 = new Icon("res\\icon3.ico");

            return 0;
        }
        private void updateIcon()
        {
            if (mcmden)
            {
                if( mcmdmode )
                    mtasknotify.setIcon(micon3);    // cmd mode on;
                else
                    mtasknotify.setIcon(micon2);    // cmd monde off;
            }
            else
                mtasknotify.setIcon(null);          // default;
        }
    }
}
