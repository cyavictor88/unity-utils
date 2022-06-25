
/*
LineRendererCollider.cs
for this to work, do the following steps:
1  Make sure "Main Camera" exists, and its position and rotation are 0s for all xyz directions
2. Create a scripts with filename "LineRendererCollider.cs"
3. Copy this whole code into the LineRendererCollider script  (the script is using the new input system, if you are on old one , rmb to change two lines( code below have details ))
4. Create an Empty GameObject in your scene, and add the LineRendererCollider script as a component
5. click Play, you should be able to see a red line, click on it, will change color, then hooray, collider works
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineRendererCollider : MonoBehaviour
{
   LineRenderer lr;
   Color[] colors = { Color.red, Color.black };
   int curColor = 0;
   void Start()
   {

        Camera.main.transform.position=new Vector3(0,1f,-10f);
        Camera.main.transform.rotation= Quaternion.identity;


       lr = this.gameObject.AddComponent<LineRenderer>();
       lr.material = new Material(Shader.Find("Sprites/Default"));
       lr.material.color = colors[curColor];
       lr.startWidth = 1f;
       lr.endWidth = 1f;
       lr.positionCount = 2;

       Vector3[] poses = new Vector3[2];
       poses[0] = new Vector3(0, 5f, 5f);
       poses[1] = new Vector3(1f, -1f, -4f);
       lr.SetPositions(poses);


       // making a meshcolllider and attach to the lineRenderer

       //beware this makes lineRenderer using local space
       lr.useWorldSpace = false;

       MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
       Mesh mesh = new Mesh();
       lr.BakeMesh(mesh, Camera.main, false);
       meshCollider.sharedMesh = mesh;


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
                   if (theGameObjectHitByRay == this.gameObject)
                   {
                       curColor = (curColor + 1) % 2;
                       lr.material.color = colors[curColor];
                   }
               }
           }
       }
   }
}
