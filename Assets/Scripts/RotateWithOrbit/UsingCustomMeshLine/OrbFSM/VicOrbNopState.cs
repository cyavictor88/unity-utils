using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicOrbNopState : VicOrbBaseState
{
    // Start is called before the first frame update
    public override void enterState(VicOrb orb)
    {
        return;
    }


    public override void updateState(VicOrb orb)
    {

        orb.orbz.showBigR(false);
        orb.orby.showBigR(false);
        orb.orbx.showBigR(false);
        if (orb.raycasthit) { orb.switchState(orb.orbFcousS); }
    }
}
