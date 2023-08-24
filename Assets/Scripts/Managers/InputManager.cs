using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
    public static InputManager instance;

    /* PLAYER */
    public Vector2 move;
    public Vector2 look;
    public float horizontal;
    public float vertical;

    public bool jump;
    public bool pickup;
    public bool pause;
    public bool dash;
    public bool aim;
    public bool shootTap;
    public bool shootHold;

    /* UI */
    public Vector2 moveTap;
    public bool select;
    public bool cancel;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    public bool GetUpTap() {
        return Mathf.RoundToInt(moveTap.y) > 0;
    }

    public bool GetDownTap() {
        return Mathf.RoundToInt(moveTap.y) < 0;
    }

    public bool GetLeftTap() {
        return Mathf.RoundToInt(moveTap.x) < 0;
    }

    public bool GetRightTap() {
        return Mathf.RoundToInt(moveTap.x) > 0;
    }

    private void LateUpdate() {
        jump = false;
        pickup = false;
        pause = false;
        dash = false;
        shootTap = false;
        moveTap = new Vector2();
        select = false;
        cancel = false;
    }

    private void OnJump(InputValue value) {
        jump = true;
    }

    private void OnPickup() {
        pickup = true;
    }

    private void OnPause() {
        pause = true;
    }

    private void OnDash() {
        dash = true;
    }

    private void OnAim(InputValue value) {
        aim = value.Get<float>() == 1;
    }

    private void OnShootTap(InputValue value) {
        shootTap = value.Get<float>() == 1;
    }

    private void OnShootHold(InputValue value) {
        shootHold = value.Get<float>() == 1;
    }

    private void OnMove(InputValue value) {
        move = value.Get<Vector2>();
        horizontal = move.x;
        vertical = move.y;
    }

    private void OnLook(InputValue value) {
        look = value.Get<Vector2>();
    }

    private void OnMoveTap(InputValue value) {
        moveTap = value.Get<Vector2>();
    }

    private void OnSelect() {
        select = true;
    }

    private void OnCancel() {
        cancel = true;
    }
}
