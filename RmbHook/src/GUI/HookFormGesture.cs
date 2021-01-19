using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace RmbHook
{
    public partial class HookForm : Form
    {
        // a thread;
        public BackgroundWorker getWorker()
        {
            return backgroundWorker1;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            GestureMan.mthis.Work();
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            GestureMan.mthis.Progress();
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GestureMan.mthis.Progress();
        }

        // setting;
        void onGestureTabSelected()
        {
            GestureParamter gesprm=GestureParamter.mthis;
            textBox5.Text = gesprm.GetGesturePrm(1);
            textBox6.Text = gesprm.GetGesturePrm(2);
            textBox7.Text = gesprm.GetGesturePrm(3);
            textBox8.Text = gesprm.GetGesturePrm(4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GestureParamter gesprm = GestureParamter.mthis;
            gesprm.SetGesturePrm(textBox5.Text,1);
            gesprm.SetGesturePrm(textBox6.Text, 2);
            gesprm.SetGesturePrm(textBox7.Text, 3);
            gesprm.SetGesturePrm(textBox8.Text, 4);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            FormEventMan.GestureOn();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormEventMan.GestureOff();
        }
    }
}
