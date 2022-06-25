// float virtualsphereRadius = Vector3.Magnitude(bounds.max-bounds.center);
// float minD = (virtualsphereRadius )/ Mathf.Sin(Mathf.Deg2Rad*cam.fieldOfView/2);
// Vector3 normVectorBoundsCenter2CurrentCamPos= (myCamera.transform.position - bounds.center) / Vector3.Magnitude(myCamera.transform.position -  bounds.center);
// myCamera.transform.position =  minD*normVectorBoundsCenter2CurrentCamPos;
// myCamera.transform.LookAt(bounds.center);
// myCamera.nearClipPlane = minD- virtualsphereRadius;
