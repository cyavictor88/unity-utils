using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrbWithLineRenderer;

namespace OrbWithLineRenderer
{
    public abstract class VicOrbBaseState
    {
        // Start is called before the first frame update
        public abstract void enterState(VicOrb orb);
        public abstract void updateState(VicOrb orb);

    }


}