using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OrbWithLineRenderer;


namespace OrbWithLineRenderer
{
    public class VicOrbFocusState : VicOrbBaseState
    {
        // Start is called before the first frame update
        public override void enterState(VicOrb orb)
        {
            return;
        }


        public override void updateState(VicOrb orb)
        {
            if (orb.rayhitgo == orb.orbxliner.thisgo)
            {
                orb.orbxliner.showFocus(true);
                orb.orbyliner.showFocus(false);
                orb.orbzliner.showFocus(false);

            }
            else if (orb.rayhitgo == orb.orbyliner.thisgo)
            {
                orb.orbyliner.showFocus(true);
                orb.orbxliner.showFocus(false);
                orb.orbzliner.showFocus(false);
            }
            else if (orb.rayhitgo == orb.orbzliner.thisgo)
            {
                orb.orbzliner.showFocus(true);
                orb.orbyliner.showFocus(false);
                orb.orbxliner.showFocus(false);
            }
            if (orb.raycasthit == false)
            {
                orb.switchState(orb.orbNopS);
            }
            else if (orb.leftmousepressed)
            {
                orb.switchState(orb.orbRotateS);
            };
        }
    }


}