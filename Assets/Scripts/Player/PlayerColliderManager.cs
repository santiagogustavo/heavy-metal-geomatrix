using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderManager : MonoBehaviour {
    [SerializeField]
    GameObject InvisibleWallEffect;

    void HandleInvisibleWallCollision(ControllerColliderHit hit) {
        Instantiate(
            InvisibleWallEffect,
            hit.point,
            Quaternion.LookRotation(hit.normal),
            hit.gameObject.transform
        );
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Invisible Wall") {
            HandleInvisibleWallCollision(hit);
        }
    }
}
