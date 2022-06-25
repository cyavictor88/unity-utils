using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicOrbRotateState : VicOrbBaseState
{

    VicMeshLine vlm_start;
    VicMeshLine vlm_end;
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


        if(orb.rotatoeThisOrbgo == orb.orbx.thisgo)
        {
            rot_pl = orb.xpl;
        }
        if (orb.rotatoeThisOrbgo == orb.orby.thisgo)
        {
            rot_pl = orb.ypl;
        }
        if (orb.rotatoeThisOrbgo == orb.orbz.thisgo)
        {
            rot_pl = orb.zpl;
        }

        if (orb.orbx.thisgo!= orb.rotatoeThisOrbgo)
        {
            // orb.orbx.thisgo.layer =2;
        }
        if (orb.orby.thisgo != orb.rotatoeThisOrbgo)
        {
            // orb.orby.thisgo.layer = 2;
        }
        if (orb.orbz.thisgo != orb.rotatoeThisOrbgo)
        {
            // orb.orbz.thisgo.layer = 2;
        }


        points[0] = Vector3.zero;
        sec_points[0] = Vector3.zero;

        p1_on_pl = rot_pl.ClosestPointOnPlane( orb.last_hitData_for_ray.point)  ;
        p2_on_pl = rot_pl.ClosestPointOnPlane( orb.last_hitData_for_ray.point )  ;

        points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius; 
        sec_points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius; 


        Color color = orb.rotatoeThisOrbgo.GetComponent<MeshRenderer>().material.color;
        if (vlm_start==null)
        {
            vlm_start = new VicMeshLine(orb.gameObject, points, Matrix4x4.identity, "orbl",color, 8);
            vlm_end = new VicMeshLine(orb.gameObject, points, Matrix4x4.identity, "orbend",color, 8);
            vlm_start.thisgo.layer = 4;
            vlm_end.thisgo.layer = 4;
            vlm_start.thisgo.GetComponent<MeshRenderer>().material.color = color;
            vlm_end.thisgo.GetComponent<MeshRenderer>().material.color = Color.black;
        }
        else
        {
            vlm_start.thisgo.GetComponent<MeshRenderer>().material.color=color;
            vlm_end.thisgo.GetComponent<MeshRenderer>().material.color=Color.black;
            vlm_start.updateMesh(points);
            vlm_end.updateMesh(sec_points);
        }
        vlm_end.showMesh(true);
        vlm_start.showMesh(true);
        //vlm_end.showMesh(true);



        return;
    }


    public override void updateState(VicOrb orb)
    {
        Quaternion target =Quaternion.identity;
        if (orb.leftmousepressed)
        {




            p2_on_pl = rot_pl.ClosestPointOnPlane(orb.last_hitData_for_ray.point );


            sec_points[1] = p2_on_pl / p2_on_pl.magnitude * orb.radius;

            Vector3 a = points[1];
            Vector3 b = sec_points[1];

            float ang = Vector3.Angle(points[1], sec_points[1]) ;

            Vector3 cross = Vector3.Cross(a, b).normalized;
            Vector3 plnorm = rot_pl.normal.normalized;
            if (plnorm == cross) { ang = -ang; }

            //Debug.Log(ang);



             target = Quaternion.AngleAxis(ang, Vector3.forward) * ori_rot;//Quaternion.Euler(0, 0, ang);
            //target.in
            if (orb.orbx.thisgo == orb.rotatoeThisOrbgo)
            {
                target = Quaternion.AngleAxis(ang, orb.transform.right) * ori_rot;
            }
            else if (orb.orby.thisgo == orb.rotatoeThisOrbgo)
            {
                target = Quaternion.AngleAxis(ang, orb.transform.up) * ori_rot;
            }
            else if (orb.orbz.thisgo == orb.rotatoeThisOrbgo)
            {
                target = Quaternion.AngleAxis(ang, orb.transform.forward) * ori_rot;
            }

            orb.FinalTargetGOToBeOrbit.transform.rotation = Quaternion.RotateTowards(orb.FinalTargetGOToBeOrbit.transform.rotation, target, 180);

            vlm_end.updateMesh(sec_points);
            vlm_end.showMesh(true);
            vlm_start.showMesh(true);
            // VicPlayActions.updateMainGameObjectRot?.Invoke( orb.FinalTargetGOToBeOrbit.transform.rotation);
        }

        if (!orb.leftmousepressed) {

            vlm_start.showMesh(false);    
            vlm_end.showMesh(false);
            orb.orbx.thisgo.layer = 4;
            orb.orby.thisgo.layer = 4;
            orb.orbz.thisgo.layer = 4;





            orb.switchState(orb.orbNopS);
        };
    }
}
