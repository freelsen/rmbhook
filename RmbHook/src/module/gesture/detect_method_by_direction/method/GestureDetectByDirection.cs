using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyMouseDo
{
    class GestureDetectByDirection
    {
        // the parent class of gesture classes, 
        // which based on the detection of the directions;
        public GestureDirection mGestureDirection = null;

        protected int mdirectionIndex = 0;

        public GestureDetectByDirection()
        {
        }

        public virtual void init() { ;}
        public virtual void start() { ;}
        public virtual void stop() { ;}

        public virtual bool onTimerTick(int ticktime)
        {
            return false;
        }

        public int getDirectionIndex()
        {
            return mdirectionIndex;
        }
    }
}
