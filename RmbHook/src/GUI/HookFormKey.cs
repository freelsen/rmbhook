using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RmbHook
{
    public partial class HookForm : Form
    {
        private static int mcntKeyEvent = 0;
        public void onKeyboardEvent(string eventType, string keyCode, string keyChar, string shift, string alt, string control)
        {
            if (mcntKeyEvent++ > 100) mcntKeyEvent = 0;
            listView2.Items.Insert(0,
                 new ListViewItem(
                     new string[]{
                        eventType, 
                        keyCode,
                        keyChar,
                        shift,
                        alt,
                        control
                }));
        }
    }
}
