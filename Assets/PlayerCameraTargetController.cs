using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraTargetController : MonoBehaviour {
    [SerializeField]
    float verticalLimitAngle = 60f;

    float dampTime = 0.2f;
    public Vector2 turn;

    void Start() {
        turn.x = transform.rotation.eulerAngles.y;
    }

    void ComputeInputs() {
        turn.x += InputManager.instance.mouseX;
        turn.y += InputManager.instance.mouseY;
        turn.y = Mathf.Clamp(turn.y, -verticalLimitAngle, verticalLimitAngle);
    }

    void MoveRotation() {
        Quaternion rotation = Quaternion.Euler(-turn.y, turn.x, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, dampTime);
    }

    void Update() {
        MoveRotation();
        if (GameManager.instance.IsGamePaused() || !GameManager.instance.MatchWasStarted()) {
            return;
        }
        ComputeInputs();
    }
}
