using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
    [SerializeField]
    GameObject character;

    float speed = 10f;
    float blendTreesDamp = 0.1f;

    Animator characterAnimator;
    Vector2 position;
    Vector2 turn;
    bool isAiming;
    bool isShooting;

    float lookUpDownLimit = 60f;

    void Start() {
        characterAnimator = character.GetComponent<Animator>();
    }

    void ComputeInputs() {
        position.x = InputManager.instance.horizontal;
        position.y = InputManager.instance.vertical;
        turn.x += InputManager.instance.mouseX;
        turn.y += InputManager.instance.mouseY;
        turn.y = Mathf.Clamp(turn.y, -lookUpDownLimit, lookUpDownLimit);
        isAiming = InputManager.instance.leftTrigger > 0f;
        isShooting = InputManager.instance.rightTrigger > 0f;
    }

    void MovePosition() {
        transform.Translate(position.x * speed * Time.deltaTime, 0, position.y * speed * Time.deltaTime);
    }

    void MoveRotation() {
        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
    }

    void SetAnimatorVariables() {
        characterAnimator.SetFloat("X", position.x, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Y", position.y, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Turn", InputManager.instance.mouseX / 2f, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Aim", turn.y / 90f, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetBool("IsAiming", isAiming);
        characterAnimator.SetBool("IsShooting", isShooting);
    }

    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            return;
        }
        ComputeInputs();
        MovePosition();
        MoveRotation();
        SetAnimatorVariables();
    }
}
