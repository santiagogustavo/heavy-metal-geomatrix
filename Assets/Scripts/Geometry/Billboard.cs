using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera cam;
    public bool useStaticBillboard;
    public bool alwaysLookAtCamera;

    void Start() {
        if (Camera.main) {
            cam = Camera.main;
        }
    }

    void LateUpdate() {
        if (Camera.main) {
            cam = Camera.main;
        }
        if (!cam) {
            return;
        }
        if (!useStaticBillboard) {
            transform.LookAt(cam.transform);
        } else {
            transform.rotation = cam.transform.rotation;
        }
        float rotationX = alwaysLookAtCamera ? transform.rotation.eulerAngles.x : 0f;
        float rotationY = transform.rotation.eulerAngles.y;
        float rotationZ = alwaysLookAtCamera ? transform.rotation.eulerAngles.z : 0f;
        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
    }
}
