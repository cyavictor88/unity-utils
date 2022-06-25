using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.InputSystem;

public class VicOrb : MonoBehaviour
{

    //when hover bigger line
    //when pressdown a bll appear
    //when press and drag y/x/ , second ball appear and slide with

    // Start is called before the first frame update


    public VicMeshLine orbx;
    public VicMeshLine orby;
    public VicMeshLine orbz;





    [HideInInspector] public RaycastHit hitData_for_ray;
    [HideInInspector] public RaycastHit last_hitData_for_ray;
    [HideInInspector] public GameObject rayhitgo;
    [HideInInspector] public GameObject rotatoeThisOrbgo;

    [HideInInspector] public bool raycasthit;
    [HideInInspector] public bool leftmousepressed;
    VicOrbBaseState orbCurS;
    [HideInInspector] public VicOrbFocusState orbFcousS = new VicOrbFocusState();
    [HideInInspector] public VicOrbNopState orbNopS = new VicOrbNopState();
    [HideInInspector] public VicOrbRotateState orbRotateS = new VicOrbRotateState();
    [HideInInspector] public float radius = 2f;

    public Quaternion qx;
    public Quaternion qy;
    public Quaternion qz;


    [HideInInspector] public Plane xpl;
    [HideInInspector] public Plane ypl;
    [HideInInspector] public Plane zpl;
    [SerializeField] public Camera orbcam;
    [SerializeField] public GameObject FinalTargetGOToBeOrbit;


    void Start()
    {


        orbcam.transform.position = new Vector3(0f, 0f, -5f);
        orbcam.rect = new Rect(0.85f, 0, 0.15f, 0.25f);



        orbcam.transform.position = new Vector3(0f, 0f, -5f);
        qx = Quaternion.AngleAxis(-80, Vector3.up);
        qy = Quaternion.identity;
        qz = Quaternion.AngleAxis(80, Vector3.right);

        qx = Quaternion.AngleAxis(-80, Vector3.up);
        qy = Quaternion.AngleAxis(80, Vector3.right);
        qz = Quaternion.identity;

        xpl = new Plane(qx * Vector3.forward, Vector3.zero);
        ypl = new Plane(qy * Vector3.forward, Vector3.zero);
        zpl = new Plane(qz * Vector3.back, Vector3.zero);


        int numpts_in_circle = 32;
        Vector3[] points = new Vector3[numpts_in_circle + 1];
        for (int t = 0; t < numpts_in_circle; t = t + 1)
        {
            Vector3 p = Vector3.right * radius;
            p = Quaternion.AngleAxis(360 / 16 * t, Vector3.forward) * p;
            p = qx * p;
            points[t] = p;
        }
        points[numpts_in_circle] = qx * Vector3.right * radius;
        orbx = new VicMeshLine(this.gameObject, points, Matrix4x4.identity, "orbx", Color.red, 4);
        orbx.thisgo.layer = 4; // water layer
        orbx.activateCollider();
        orbx.showMesh(true);

        for (int t = 0; t <= numpts_in_circle; t = t + 1)
        {
            points[t] = qy * Quaternion.Inverse(qx) * points[t];
        }
        orby = new VicMeshLine(this.gameObject, points, Matrix4x4.identity, "orby", Color.green, 4);
        orby.thisgo.layer = 4; // water layer
        orby.activateCollider();
        orby.showMesh(true);


        for (int t = 0; t <= numpts_in_circle; t = t + 1)
        {
            points[t] = qz * Quaternion.Inverse(qy) * points[t];
        }
        orbz = new VicMeshLine(this.gameObject, points, Matrix4x4.identity, "orbz", Color.blue, 4);
        orbz.thisgo.layer = 4; // water layer
        orbz.activateCollider();
        orbz.showMesh(true);


        orbCurS = orbNopS;
        orbCurS.enterState(this);

    }
    public void switchState(VicOrbBaseState state)
    {
        orbCurS = state;
        state.enterState(this);
    }



    // Update is called once per frame
    void Update()
    {



        Ray ray = orbcam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (raycasthit = Physics.Raycast(ray, out hitData_for_ray))
        {

            rayhitgo = hitData_for_ray.collider.gameObject;
            last_hitData_for_ray = hitData_for_ray;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame) { leftmousepressed = true; }
        if (Mouse.current.leftButton.wasReleasedThisFrame) { leftmousepressed = false; }

        orbCurS.updateState(this);


        //float delx = Input.GetAxis("Mouse X");
        //float dely = Input.GetAxis("Mouse Y");
        ////transform.position = transform.position + (-transform.right * delx * 50 * Time.deltaTime);
        ////transform.position = transform.position + (-transform.up * dely * 50 * Time.deltaTime);


    }
}
