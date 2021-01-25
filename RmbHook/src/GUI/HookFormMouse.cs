using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KeyMouseDo
{
    public partial class MainForm : Form
    {
        private void mhookStart_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOn();
        }
        private void mhookStop_Click(object sender, EventArgs e)
        {
            FormEventMan.MouseOff();
        }

        public void ShowMouseLocation(int x, int y)
        {
            curXYLabel.Text = String.Format("Current Mouse Point: X={0}, y={1}", x, y);
        }


        private static int mcntMouseEvent = 0;
        public void onMouseEvent(string eventType, string button, string x, string y, string delta)
        {
            if (mcntMouseEvent++ > 100) mcntMouseEvent = 0;

            listView1.Items.Insert(0,
                new ListViewItem(
                    new string[]{
                        eventType, 
                        button,
                        x,
                        y,
                        delta
                    }));
        }

        public void ShowColor(Color clr)
        {
            Graphics g = this.CreateGraphics();
            Rectangle rect = new Rectangle(10, 10, 50, 50);

            SolidBrush b1 = new SolidBrush(clr);
            g.FillRectangle(b1, rect);
        }
    }
}
