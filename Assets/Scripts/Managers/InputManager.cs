using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance;
    public float horizontal;
    public float vertical;
    public float dPadX;
    public float dPadY;
    public bool jump;
    public bool fire1;
    public bool fire2;
    public bool fire3;
    public bool fire4;
    public bool fire5;
    public bool pause;

    public float leftTrigger;
    public float rightTrigger;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    float GetMouseLeft() {
        return Input.GetMouseButtonDown(1) ? 1f : 0f;
    }

    float GetMouseRight() {
        return Input.GetMouseButtonDown(0) ? 1f : 0f;
    }
    public bool GetLeftAnalog() {
        return Mathf.RoundToInt(horizontal) == -1f;
    }

    public bool GetRightAnalog() {
        return Mathf.RoundToInt(horizontal) == 1f;
    }

    public bool GetLeftDpad() {
        return Mathf.RoundToInt(dPadX) == -1f;
    }

    public bool GetRightDpad() {
        return Mathf.RoundToInt(dPadX) == 1f;
    }

    public bool GetLeft() {
        return GetLeftAnalog() || GetLeftDpad();
    }

    public bool GetRight() {
        return GetRightAnalog() || GetRightDpad();
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        fire1 = Input.GetButtonDown("Fire1");
        fire2 = Input.GetButtonDown("Fire2");
        fire3 = Input.GetButtonDown("Fire3");
        fire4 = Input.GetButtonDown("Fire4");
        fire5 = Input.GetButtonDown("Fire5");
        pause = Input.GetButtonDown("Pause");
        leftTrigger = Mathf.Max(Input.GetAxis("Left Trigger"), GetMouseLeft());
        rightTrigger = Mathf.Max(Input.GetAxis("Right Trigger"), GetMouseRight());
        dPadX = Input.GetAxisRaw("DPad X");
        dPadY = Input.GetAxisRaw("DPad Y");
    }
}
