using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoW.wow
{
    // index start from 0,
    // for rogue;
    public enum EgridRogue : int
    {
        phpmax = 10,
        phpcur = 11,
        pmanamax = 12,
        pmanacur = 13,
        plevel = 14,
        ppoint = 54,
        pcombat = 42,
        xpos = 1,
        ypos = 2,

        ttype = 55,
        trange = 15,
        thpmax = 18,
        thpcur = 19,
        tname1 = 16,
        tname2 = 17,

        ashoot = 34,
        asinster = 35,
        aauto = 36,
        asteal = 53,
        aslice = 55

    }

    public enum EactionRogue : int
    {
        sinister,
        eviscerate,
        backstab,
        slice,
        auto,
        hide,
        heal,
        jump
    }

}
