using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOnController : MonoBehaviour {
    public GameObject LockOnTarget;

    CinemachineFreeLook cam;
    Transform defaultLookAt;
    float defaultXAxis;
    float defaultYAxis;
    GameObject[] lookAtTargets;

    int currentLockTarget = -1;
    GameObject targetInstance;

    void Awake() {
        cam = GetComponent<CinemachineFreeLook>();
        defaultLookAt = cam.LookAt;
        defaultXAxis = cam.m_XAxis.Value;
        defaultYAxis = cam.m_YAxis.Value;
        lookAtTargets = GameObject.FindGameObjectsWithTag("Lock Target");
    }

    void Start() {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    public bool IsLockedOn() {
        return currentLockTarget != -1;
    }

    public float GetAxisCustom(string axisName) {
        if (IsLockedOn()) {
            return 0;
        } else {
            return UnityEngine.Input.GetAxis(axisName);
        }
    }

    void InstantiateLockTarget() {
        targetInstance = Instantiate(
                LockOnTarget,
                new Vector3(0f, -0.1f, -0.25f),
                Quaternion.Euler(0f, 0f, 0f)
            );
        targetInstance.transform.SetParent(lookAtTargets[currentLockTarget].transform, false);
    }

    void ClearLockTarget() {
        currentLockTarget = -1;
        cam.LookAt = defaultLookAt;
    }

    void ClearCameraOrbit() {
        cam.m_XAxis.Value = defaultXAxis;
        cam.m_YAxis.Value = defaultYAxis;
    }

    void NextLockTarget() {
        if (targetInstance) {
            Destroy(targetInstance);
        }
        currentLockTarget += 1;
        if (currentLockTarget == lookAtTargets.Length) {
            ClearLockTarget();
        } else if (IsLockedOn()) {
            InstantiateLockTarget();
            ClearCameraOrbit();
        }
    }

    void UpdateCameraYAxis() {
        Vector3 current = defaultLookAt.transform.position;
        Vector3 target = lookAtTargets[currentLockTarget].transform.position;
        Vector3 lookDirection = target - current;
        cam.m_YAxis.Value = Vector3.Angle(lookDirection, Vector3.up) / 180f;
    }

    void UpdateCameraXAxis() {
        Vector3 current = defaultLookAt.transform.position;
        Vector3 target = lookAtTargets[currentLockTarget].transform.position;
        float a1 = current.x;
        float a2 = current.z;
        float b1 = target.x;
        float b2 = target.z;
        float theta = Mathf.Atan2(b1 - a1, a2 - b2);
        float lookAngle = 90f - (Mathf.Rad2Deg * theta);
        cam.m_XAxis.Value = lookAngle;
    }

    void Update() {
        if (InputManager.instance.fire3) {
            NextLockTarget();
        }
        if (IsLockedOn()) {
            UpdateCameraXAxis();
            UpdateCameraYAxis();
        }
    }
}
