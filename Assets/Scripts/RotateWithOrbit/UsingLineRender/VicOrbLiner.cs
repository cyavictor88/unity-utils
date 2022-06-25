using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using OrbWithLineRenderer;


namespace OrbWithLineRenderer
{
    public class VicOrbLiner
    {
        public GameObject thisgo;

        public LineRenderer lr;

        private float smallw = 0.1f;
        private float bigw = 0.3f;

        Mesh lrmesh;

        public VicOrbLiner(GameObject gparent, string name, Color color, Vector3[] points)
        {
            thisgo = new GameObject() { name = name };
            thisgo.transform.parent = gparent.transform;
            lr = thisgo.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.material.color = color;
            lr.startWidth = smallw;
            lr.endWidth = smallw;
            lr.positionCount = points.Length;
            lr.SetPositions(points);
            lr.enabled = true;



        }

        public void activateCollider()
        {
            lr.useWorldSpace = false;
            MeshCollider meshCollider = thisgo.AddComponent<MeshCollider>();
            lrmesh = new Mesh();
            lr.BakeMesh(lrmesh, Camera.main, false);
            meshCollider.convex = true;
            meshCollider.sharedMesh = lrmesh;
        }

        public void showFocus(bool yeano)
        {
            if (!yeano)
            {
                lr.startWidth = smallw;
                lr.endWidth = smallw;
            }
            else
            {
                lr.startWidth = bigw;
                lr.endWidth = bigw;
            }
        }




    }

}