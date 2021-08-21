using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lcom
{
    public class LObj
    {
        public LObj()
        {
        }
        public virtual int init()
        {
            return 0;
        }
        public virtual int start()
        {
            return 0;
        }
        public virtual int reset()
        {
            return 0;
        }
        public virtual int work()
        {
            return 0;
        }
        public virtual void stop()
        {
        }
    }
}
