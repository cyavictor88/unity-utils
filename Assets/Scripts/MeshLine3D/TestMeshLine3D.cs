/*
TestMeshLine3D.cs
for this to work, do the following steps:
1  Make sure "Main Camera" exists
2. create a scripts with filename "MeshLine3D.cs" and copy the content of MeshLine3D.cs into it.
2. Create a scripts with filename "TestMeshLine3D.cs" and copy the content of TestMeshLine3D.cs into it.
3. the TestMeshLine3D.cs uses the new input system, if you are on the old input system , rmb to change two lines( code below have details ))
4. Create an Empty GameObject in your scene, and then add the TestMeshLine3D.cs script as a component
5. click Play, you should be able to see a red curved line, then yay meshline3d works
6. click on the curved line, if color changed from red to black, then yay, collider on the linemesh works
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMeshLine3D : MonoBehaviour
{
    List<Vector3> function_to_be_drawn = new List<Vector3>();
    MeshLine3D ml3d;
    Color[] colors = { Color.red, Color.black };
    int curColor = 0;
    void Start()
    {
        // setting the main camera at posistion so can see the to be drawned line mesh 
        Camera.main.transform.position=new Vector3(0,1f,-10f);
        Camera.main.transform.rotation=Quaternion.Euler(0,0,0);

        // i am drawing   f(x,y,z) = ( x, 2x^3 - 5x, 0 ) or simply:  y = 2x^3-5x   from x = -3 to 3 with 0.01 increment  
        for (float x = -3f; x <= 3f; x += 0.01f)
        {
            function_to_be_drawn.Add(new Vector3(x, 2 * Mathf.Pow(x, 3) - 5 * x, 0));
        }

        GameObject parent_gameObject_of_meshline = this.gameObject;

        // if the grid for your function is the same as the unity world space, leave it as identity matrix,
        //  or else you have to do some calculation on your own to supply the correct "grid to world(g2w)" matrix
        Matrix4x4 g2w = Matrix4x4.identity;
        
        string meshline_gobj_name = "MeshLine";
        int num_side_circle = 8;
        float lineThickness = 0.05f;
        ml3d = new MeshLine3D(parent_gameObject_of_meshline, function_to_be_drawn.ToArray(), g2w, meshline_gobj_name, colors[curColor], num_side_circle, lineThickness);
        ml3d.showMesh(true);
        ml3d.activateCollider();
    }

    void Update()
    {
        // when you click on the line, the line color should change between red and black

        // be aware that I am using the new input system
        // if using the old input system, i think you replace the the next line with "if(Input.GetMouseButtonDown(0))"
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // if using the old input system, i think you replace the the next line with "Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);"
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData_for_the_ray;
            if (Physics.Raycast(ray, out hitData_for_the_ray))
            {
                GameObject theGameObjectHitByRay = hitData_for_the_ray.collider.gameObject;
                {
                    if (theGameObjectHitByRay == ml3d.thisgo)
                    {
                        curColor = (curColor + 1) % 2;
                        ml3d.linemeshMaterial.color = colors[curColor];
                    }
                }
            }
        }
    }
}
