using ls.libs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoW.libs;

namespace WoW.wow
{
    class ColorFetcher
    {
        public Func<IntPtr> getHwnd;

        public Color getColorClient(int x, int y)
        {
            IntPtr hwnd = getHwnd();
            return FetchColor.getColorClient((int)hwnd, x, y);
        }
    }
}
