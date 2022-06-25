using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OrbWithLineRenderer;

namespace OrbWithLineRenderer
{
    public class VicOrbRotateState : VicOrbBaseState
    {

        // VicMeshLine vlm_start;
        // VicMeshLine vlm_end;
        Vector3[] points = new Vector3[2];
        Vector3[] sec_points = new Vector3[2];

        Quaternion ori_rot;
        Plane rot_pl;
        Vector3 p1_on_pl;
        Vector3 p2_on_pl;

        // Start is called before the first frame update
        public override void enterState(VicOrb orb)
        {
            orb.rotatoeThisOrbgo = orb.rayhitgo;
            ori_rot = orb.FinalTargetGOToBeOrbit.transform.rotation;

            if (orb.rotatoeThisOrbgo == orb.orbxliner.thisgo)
            {
                rot_pl = orb.xpl;
            }
            if (orb.rotatoeThisOrbgo == orb.orbyliner.thisgo)
            {
                rot_pl = orb.ypl;
            }
            if (orb.rotatoeThisOrbgo == orb.orbzliner.thisgo)
            {
                rot_pl = orb.zpl;
            }

            if (orb.orbxliner.thisgo != orb.rotatoeThisOrbgo)
            {
                // orb.orbxliner.thisgo.layer = orb.orbsLayerWhenNotBeingRotated;
                orb.orbxliner.lr.enabled = false;
            }
            if (orb.orbyliner.thisgo != orb.rotatoeThisOrbgo)
            {
                // orb.orbyliner.thisgo.layer = orb.orbsLayerWhenNotBeingRotated;
                orb.orbyliner.lr.enabled = false;

            }
            if (orb.orbzliner.thisgo != orb.rotatoeThisOrbgo)
            {
                // orb.orbzliner.thisgo.layer = orb.orbsLayerWhenNotBeingRotated;
                orb.orbzliner.lr.enabled = false;

            }


            // Color color = Color.gray;

            // if (orb.orbxliner.thisgo == orb.rotatoeThisOrbgo)
            // {
            //     color = orb.orbxliner.lr.startColor;
            // }
            // if (orb.orbyliner.thisgo == orb.rotatoeThisOrbgo)
            // {
            //     color = orb.orbyliner.lr.startColor;

            // }
            // if (orb.orbzliner.thisgo == orb.rotatoeThisOrbgo)
            // {
            //     color = orb.orbzliner.lr.startColor;

            // }



            points[0] = Vector3.zero;
            sec_points[0] = Vector3.zero;

            p1_on_pl = rot_pl.ClosestPointOnPlane(orb.last_hitData_for_ray.point);
            p2_on_pl = rot_pl.ClosestPointOnPlane(orb.last_hitData_for_ray.point);

            points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius;
            sec_points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius;


            Color color = orb.rotatoeThisOrbgo.GetComponent<LineRenderer>().material.color;
            // if (vlm_start == null)
            // {
            //     vlm_start = new VicMeshLine(orb.gameObject, points, Matrix4x4.identity, "orbl", color, 8);
            //     vlm_end = new VicMeshLine(orb.gameObject, points, Matrix4x4.identity, "orbend", color, 8);
            //     vlm_start.thisgo.layer = orb.orbsLayer;
            //     vlm_end.thisgo.layer = orb.orbsLayer;
            //     vlm_start.thisgo.GetComponent<MeshRenderer>().material.color = color;
            //     vlm_end.thisgo.GetComponent<MeshRenderer>().material.color = Color.black;
            // }
            // else
            // {
            orb.startIndicatorLiner.lr.startColor = color;
            orb.startIndicatorLiner.lr.endColor = color;

            orb.endIndicatorLiner.lr.startColor = Color.white;
            orb.endIndicatorLiner.lr.endColor = Color.white;

            orb.startIndicatorLiner.lr.SetPositions(points);
            orb.endIndicatorLiner.lr.SetPositions(sec_points);
            // }
            orb.startIndicatorLiner.lr.enabled = true;
            orb.endIndicatorLiner.lr.enabled = true;
            //vlm_end.showMesh(true);



            return;
        }


        public override void updateState(VicOrb orb)
        {
            Quaternion target = Quaternion.identity;
            if (orb.leftmousepressed)
            {




                p2_on_pl = rot_pl.ClosestPointOnPlane(orb.last_hitData_for_ray.point);


                sec_points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius;

                Vector3 a = points[1];
                Vector3 b = sec_points[1];

                float ang = Vector3.Angle(points[1], sec_points[1]);

                Vector3 cross = Vector3.Cross(a, b).normalized;
                Vector3 plnorm = rot_pl.normal.normalized;
                if (plnorm == cross) { ang = -ang; }

                //Debug.Log(ang);



                target = Quaternion.AngleAxis(ang, Vector3.forward) * ori_rot;//Quaternion.Euler(0, 0, ang);
                                                                              //target.in
                if (orb.orbxliner.thisgo == orb.rotatoeThisOrbgo)
                {
                    target = Quaternion.AngleAxis(ang, orb.transform.right) * ori_rot;
                }
                else if (orb.orbyliner.thisgo == orb.rotatoeThisOrbgo)
                {
                    target = Quaternion.AngleAxis(ang, orb.transform.up) * ori_rot;
                }
                else if (orb.orbzliner.thisgo == orb.rotatoeThisOrbgo)
                {
                    target = Quaternion.AngleAxis(ang, orb.transform.forward) * ori_rot;
                }

                orb.FinalTargetGOToBeOrbit.transform.rotation = Quaternion.RotateTowards(orb.FinalTargetGOToBeOrbit.transform.rotation, target, 180);


                orb.endIndicatorLiner.lr.SetPositions(sec_points);
                orb.startIndicatorLiner.lr.enabled = true;
                orb.endIndicatorLiner.lr.enabled = true;


                // VicPlayActions.updateMainGameObjectRot?.Invoke( orb.FinalTargetGOToBeOrbit.transform.rotation);
            }

            if (!orb.leftmousepressed)
            {
                orb.startIndicatorLiner.lr.enabled = false;
                orb.endIndicatorLiner.lr.enabled = false;
                // orb.orbxliner.thisgo.layer = orb.orbsLayer;
                // orb.orbyliner.thisgo.layer = orb.orbsLayer;
                // orb.orbzliner.thisgo.layer = orb.orbsLayer;
                orb.orbxliner.lr.enabled = true;
                orb.orbyliner.lr.enabled = true;
                orb.orbzliner.lr.enabled = true;

                orb.switchState(orb.orbNopS);
            };
        }
    }

}