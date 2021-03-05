using MouseKeyboardLibrary;
using System.Drawing;
using System.Threading;

namespace WrittingHelper
{
    internal class MouseHelper
    {
        public static bool misbusy = false;  // 2021-02-14,

        private static bool setBusy()
        {
            bool busy = misbusy;  // record old status;
            if (!misbusy)
            {
                misbusy = true;
            }
            return busy;
        }

        private static void restoreBusy(bool busy)
        {
            if (misbusy != busy)
                misbusy = busy;
        }

        public static void Click(int button, int x, int y)
        {
            bool b = setBusy();

            MouseButton bt = MouseButton.Left;
            if (button == 1)
                bt = MouseButton.Left;
            else if (button == 2)
                bt = MouseButton.Right;
            else if (button == 3)
                bt = MouseButton.Middle;

            MouseSimulator.Position = new Point(x, y);
            Thread.Sleep(100);
            //MouseSimulator.MouseDown(bt, 0, 0, 0);
            //Thread.Sleep(10);
            //MouseSimulator.MouseUp(bt, 0, 0, 0);

            //MouseSimulator.Click(bt, 0, 0, 0);
            //MouseSimulator.Click(bt,1,x,y);

            SimSentInput.ClickLeftMouseButton();
            Thread.Sleep(100);

            restoreBusy(b);
        }

        public static void setPos(Point pt)
        {
            bool b = setBusy();

            MouseSimulator.Position = pt;
            Thread.Sleep(50);

            restoreBusy(b);
        }
    }
}