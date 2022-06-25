using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicOrbFocusState : VicOrbBaseState
{
    // Start is called before the first frame update
    public override void enterState(VicOrb orb)
    {
       return;
    }


    public override void updateState(VicOrb orb)
    {
        if (orb.rayhitgo == orb.orbx.thisgo)
        {
            orb.orbx.showBigR(true);
            orb.orby.showBigR(false);
            orb.orbz.showBigR(false);

        }
        else if (orb.rayhitgo == orb.orby.thisgo)
        {
            orb.orby.showBigR(true);
            orb.orbx.showBigR(false);
            orb.orbz.showBigR(false);
        }
        else if (orb.rayhitgo == orb.orbz.thisgo)
        {
            orb.orbz.showBigR(true);
            orb.orby.showBigR(false);
            orb.orbx.showBigR(false);
        }
        if (orb.raycasthit==false)
        {
            orb.switchState(orb.orbNopS);
        }
        else if (orb.leftmousepressed)
        {
            orb.switchState(orb.orbRotateS);
        };
    }
}
