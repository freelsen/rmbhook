using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WrittingHelper
{
    public class GesPair
    {
        //bool mbpair = true;                        // 140716;
        int mpaircnt = 0;
        int mpairwant = -1;
        int mpairgap = 20;

        int movtime = 400;
        int mtmcnt = 0;

        public GesPair()
        {

        }
        public int linit()
        {
            return 0;
        }
        public void start()
        {
            Parameter prm = Parameter.mthis;
            //mbpair = (prm.mpair == 1) ? true : false;   // 140717;
            //mpairgap = 

            mpaircnt = 0;
        }
        public int gtStopGap()
        {
            if (mpaircnt == 1)
                return mpairgap;
            else
                return -1;
        }
        
        public void onStart()
        {
            if (mpaircnt <= 0)
            {
                mpaircnt = 0;
                mtmcnt = 0;
            }
            mpaircnt++;
        }
        public bool onStop(double dis, double dismin) {
            if (mpaircnt == 1){
                if (dis > dismin){
                    return true;
                }
                else {
                    mpaircnt = 0;
                    Console.WriteLine("gesPair.onStop()> stop when mpaircnt =1");
                    return false;
                }
            } else {
                Console.WriteLine("gesPair.onStop()> stop when mpaircnt =2");
                mpaircnt = 0;
                return true;
            }         
        }
        public bool ckOvtime(int tm)
        {
            if (mpaircnt == 1)
            {
                mtmcnt += tm;
                if (mtmcnt >= movtime)                 // overtime;
                {
                    mpaircnt = 0;
                    Console.WriteLine("gespair.ckOvtime()> overtime.");
                    return true;
                }
            }
            return false;
        }
        bool isOvertime(int tm)
        {
            return (tm >= movtime);
        }
            
        public int ckPair( int a) {
            //Console.WriteLine("gespair.ckPair()> mpaircnt=" + mpaircnt.ToString());

            //mpaircnt = 0;

            if (mpaircnt == 1) {
                mpairwant = a;
                return -1;
            }
            else{
                if (mpairwant < 4 && (a == mpairwant + 4)) {
                    //a = mpairwant;             // 140717;
                    return mpairwant;
                }
                else if (mpairwant >= 4 && (a == mpairwant - 4)) {
                    //a = mpairwant;
                    return mpairwant;
                }
            }
            
            return -1;
        }

        
    }
}
