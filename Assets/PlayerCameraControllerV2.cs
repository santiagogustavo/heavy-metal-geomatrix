using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControllerV2 : MonoBehaviour {
    [SerializeField]
    Transform cameraTarget;

    Camera cam;
    float originalFov;
    float aimFov;
    float currentFov;

    float positionLerp = 0.2f;
    float rotationLerp = 0.1f;

    void Start() {
        cam = GetComponent<Camera>();
        originalFov = cam.fieldOfView;
        aimFov = originalFov - 15f;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, cameraTarget.position, positionLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, rotationLerp);

        if (InputManager.instance.leftTrigger > 0f) {
            currentFov = aimFov;
        } else {
            currentFov = originalFov;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, currentFov, 0.1f);
    }
}
