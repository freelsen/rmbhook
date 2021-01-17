using System;

namespace RmbHook
{
    class AppMan
    {
        /* 2021-01-17, manage the lifecycle of the application;
         * 
         */

        public static ObjMan gthis = null;

        public static int create()
        {
            if (gthis != null)
            {
                return 0;
            }
            else
            {
                gthis = new ObjMan();
                return 0;
            }
        }
        public static int init()
        {
            if (gthis != null)
            {
                return gthis.init();
            }
            else
            {
                return -1;
            }
        }
        public static int exit()
        {
            if (gthis != null)
            {
                return gthis.exit();
            }
            else
            {
                return -1;
            }
        }

        public AppMan()
        {
        }
    }
}