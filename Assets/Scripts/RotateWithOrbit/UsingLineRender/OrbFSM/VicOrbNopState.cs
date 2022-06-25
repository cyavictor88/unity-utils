using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrbWithLineRenderer;

namespace OrbWithLineRenderer
{
    public class VicOrbNopState : VicOrbBaseState
    {
        // Start is called before the first frame update
        public override void enterState(VicOrb orb)
        {
            return;
        }


        public override void updateState(VicOrb orb)
        {

            orb.orbxliner.showFocus(false);
            orb.orbyliner.showFocus(false);
            orb.orbzliner.showFocus(false);
            if (orb.raycasthit) { orb.switchState(orb.orbFcousS); }
        }
    }

}