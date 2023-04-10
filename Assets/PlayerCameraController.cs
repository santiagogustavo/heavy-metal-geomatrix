using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    GameObject matchCamera;

    void Update() {
        if (GameManager.instance.MatchWasStarted()) {
            mainCamera.SetActive(true);
            matchCamera.SetActive(false);
        } else {
            mainCamera.SetActive(false);
            matchCamera.SetActive(true);
        }
    }
}
