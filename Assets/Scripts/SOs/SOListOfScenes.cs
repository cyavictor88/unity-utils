using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "VicSO/List Of Scenes")]

public class SOListOfScenes : ScriptableObject
{

    public List<string> scenesStrs = new List<string>
            { "Base Page","LineRenderer with Collider","Custom Mesh Line 3D"};

    // Start is called before the first frame update
}
