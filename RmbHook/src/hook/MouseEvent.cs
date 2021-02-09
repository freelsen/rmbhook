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
        int mrdowncnt = 0;
        int mrdowncntmax = 3;

        DateTime mldownlast = DateTime.Now;
        int mldowncnt = 0;
        int mldowncntmax = 3;

        bool misldown = false;
        bool misrdown = false;

        public bool OnRDown(ref bool isdouble)
        {
            misrdown = true;
            
            //isdouble= CheckDbTime(ref mrdownlast);
            //if (isdouble)
            //    mrdownlast = mrdownlast.AddMilliseconds(-mdbtime);
            isdouble = checkNTime(ref mrdownlast, ref mrdowncnt, mrdowncntmax);

            return isdouble;
        }
        public void OnRUp()
        {
            misrdown = false;
        }

        public bool OnLDown(ref bool isdouble)
        {
            misldown = true;

            //if (misrdown)
                //isdouble= CheckDbTime(ref mldownlast);
                isdouble= checkNTime(ref mldownlast,ref mldowncnt, mldowncntmax);
            //else
            //    isdouble= false;

            if (isdouble)
                return true;
            else
                return false;
        }
        public void OnLUp()
        {
            misldown = false;
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
                //dtm=dtm.AddMilliseconds(-mdbtime);
            }
            return dk;
        }
        bool checkNTime(ref DateTime dtm, ref int count, int times)
        {
            if (CheckDbTime(ref dtm))
            {
                count++;
            }
            else
                count = 0;
            //
            if (count+1 >= times)
            {
                count = 0;
                return true;
            }
            else
                return false;
        }

        
    }
}
