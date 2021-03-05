using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WrittingHelper
{
    partial class DrawForm : Form
    {
        //public EventHandler onTimer1;
        public Action onTimer1;

        //public delegate void mwork1do(v)
        public EventHandler mwork1do = null;
        public EventHandler mwork1progress = null;
        public EventHandler mwork1completed = null;
        public BackgroundWorker Work1 { get { return this.backgroundWorker1; } }

        // timer 1;
        public void setTimer(bool b)
        {
            this.timer1.Enabled = b;
        }
        public void setTimer()
        {
            timer1.Enabled = !timer1.Enabled;
            DbMsg.Msg("timer1=" + timer1.Enabled.ToString());

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //mdfman.onTimer();
            this.onTimer1();
            // tickProc1();            
        }

        //void tickProc1()
        //{
        //    if (this.Visible == true)
        //    {
        //        if (mgrap != null)
        //        {
        //            if (mdfman != null)
        //            {
        //                mdfman.onParint(mgrap);
        //                //    return;
        //            }

        //            // Redraw immediatily
        //            Invalidate(false);
        //        }
        //    }
        //}



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (mwork1do != null)
                mwork1do(sender, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (mwork1progress != null)
                mwork1progress(sender, e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (mwork1completed != null)
                mwork1completed(sender, e);
        }
    }
}
