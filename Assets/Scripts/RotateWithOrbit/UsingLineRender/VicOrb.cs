using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using OrbWithLineRenderer;

namespace OrbWithLineRenderer
{
    public class VicOrb : MonoBehaviour
    {
        public VicOrbLiner orbxliner;
        public VicOrbLiner orbyliner;
        public VicOrbLiner orbzliner;
        public VicOrbLiner startIndicatorLiner;
        public VicOrbLiner endIndicatorLiner;
        [HideInInspector] public RaycastHit last_hitData_for_ray;
        [HideInInspector] public GameObject rayhitgo;
        [HideInInspector] public GameObject rotatoeThisOrbgo;
        [HideInInspector] public bool raycasthit;
        [HideInInspector] public bool leftmousepressed;
        [HideInInspector] public VicOrbFocusState orbFcousS = new VicOrbFocusState();
        [HideInInspector] public VicOrbNopState orbNopS = new VicOrbNopState();
        [HideInInspector] public VicOrbRotateState orbRotateS = new VicOrbRotateState();
        [HideInInspector] public float radius = 2f;
        [HideInInspector] public Plane xpl;
        [HideInInspector] public Plane ypl;
        [HideInInspector] public Plane zpl;

        [SerializeField] public Camera orbcam;
        [SerializeField] public int orbsLayer = 4;
        [SerializeField] public GameObject FinalTargetGOToBeOrbit;

        RaycastHit hitData_for_ray;
        Quaternion qx;
        Quaternion qy;
        Quaternion qz;
        VicOrbBaseState orbCurS;





        void Start()
        {

            // orbcam.transform.position = new Vector3(0f, 0f, -5f);
            // orbcam.rect = new Rect(0.85f, 0, 0.15f, 0.25f);


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

            orbxliner = new VicOrbLiner(this.gameObject, "orbxliner", Color.red, points);
            orbxliner.thisgo.layer = orbsLayer;
            orbxliner.activateCollider();


            for (int t = 0; t <= numpts_in_circle; t = t + 1)
            {
                points[t] = qy * Quaternion.Inverse(qx) * points[t];
            }
            orbyliner = new VicOrbLiner(this.gameObject, "orbyliner", Color.green, points);
            orbyliner.thisgo.layer = orbsLayer;
            orbyliner.activateCollider();


            for (int t = 0; t <= numpts_in_circle; t = t + 1)
            {
                points[t] = qz * Quaternion.Inverse(qy) * points[t];
            }
            orbzliner = new VicOrbLiner(this.gameObject, "orbzliner", Color.blue, points);
            orbzliner.thisgo.layer = orbsLayer;
            orbzliner.activateCollider();


            startIndicatorLiner = new VicOrbLiner(this.gameObject, "startIndicatorLiner", Color.white, new Vector3[2] { Vector3.zero, Vector3.down });
            startIndicatorLiner.thisgo.layer = orbsLayer;
            startIndicatorLiner.lr.enabled = false;

            endIndicatorLiner = new VicOrbLiner(this.gameObject, "endIndicatorLiner", Color.white, new Vector3[2] { Vector3.zero, Vector3.down });
            endIndicatorLiner.thisgo.layer = orbsLayer;
            endIndicatorLiner.lr.enabled = false;

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


        }
    }

}