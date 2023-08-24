using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControllerV2 : MonoBehaviour {
    [SerializeField] LayerMask cullingMask = new LayerMask();
    [SerializeField] Transform cameraTarget;
    [SerializeField] Transform cameraPivot;

    Camera cam;
    float originalFov;
    float aimFov;
    float currentFov;
    float dashFov;

    bool isAiming;
    bool isDashing;

    float positionLerp = 0.2f;
    float rotationLerp = 0.1f;
    float collisionAdjust = 0.1f;

    void Start() {
        cam = GetComponent<Camera>();
        originalFov = cam.fieldOfView;
        aimFov = originalFov - 15f;
        dashFov = originalFov + 10f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ApplyAim(bool aim) {
        isAiming = aim;
    }

    public void ApplyDash(bool dash) {
        isDashing = dash;
    }

    void DetectCollisionAndCulling() {
        RaycastHit hit;
        if (Physics.Linecast(cameraTarget.position, cameraPivot.position, out hit, cullingMask)) {
            transform.position = hit.point + transform.forward * collisionAdjust;
        }
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, cameraTarget.position, positionLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, rotationLerp);
        //DetectCollisionAndCulling();

        if (isAiming) {
            currentFov = aimFov;
        } else if (isDashing) {
            currentFov = dashFov;
        } else {
            currentFov = originalFov;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFov, 0.1f);
    }
}
