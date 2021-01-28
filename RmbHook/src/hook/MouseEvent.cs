using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class MouseEHandler
    {
        public static MouseEHandler mthis = null;

        public MouseEHandler()
        {
        }
        
        bool mrdbclick = false;
        bool mldbclick = false;
        bool mmdbclick = false;


        DateTime mrdownlast = DateTime.Now;
        DateTime mldownlast = DateTime.Now;
        public bool OnRDown()
        {
            return CheckDbTime(ref mrdownlast);
        }
        public bool OnLDown()
        {
            return CheckDbTime(ref mldownlast);
        }
        public void OnMDown()
        {
        }

        int mdbtime = 500; //ms;
        bool CheckDbTime(ref DateTime dtm)
        {
            bool dk = false;

            DateTime dtnow = DateTime.Now;
            TimeSpan ts = dtnow.Subtract(dtm);
            dtm = dtnow;

            int dt = (int)ts.TotalMilliseconds;    // time difference;
            //Console.WriteLine(dt.ToString());
            if (dt < mdbtime)
            {
                dk = true;
                dtm=dtm.AddMilliseconds(-mdbtime);
            }
            return dk;
        }
    }
}
