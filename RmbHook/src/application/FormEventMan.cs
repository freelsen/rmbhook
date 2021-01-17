using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RmbHook.src.keyword;

namespace RmbHook
{
    public class FormEventMan
    {
        /* 2021-01-17, 
         * 
         */

        public FormEventMan()
        {
        }

        public static void Load(object sender, EventArgs e)
        {
            AppMan.init();
        }
        public static void Closing(object sender, EventArgs e)
        {
            AppMan.exit();
            LsKeyword.getThis().exit();     // 20160503;
        }
        public static void Closed(object sender, EventArgs e)
        {
            Console.WriteLine("->hookform closed.");
        }

        public static void SizeChanged(object sender, EventArgs e)
        {
            TaskbarNotify mtasknotify = ObjMan.gthis.mtasknotify;
            if (mtasknotify != null)
                mtasknotify.onFormSizeChanged();
        }
    }
}
