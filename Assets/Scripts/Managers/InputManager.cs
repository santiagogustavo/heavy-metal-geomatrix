using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance;
    public float horizontal;
    public float vertical;
    public float mouseX;
    public float mouseY;
    public float dPadX;
    public float dPadY;

    public bool horizontalInUse;
    public bool verticalInUse;
    public bool dPadXInUse;
    public bool dPadYInUse;

    public bool jump;
    public bool dash;

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
        return Input.GetMouseButton(0) ? 1f : 0f;
    }

    float GetMouseRight() {
        return Input.GetMouseButton(1) ? 1f : 0f;
    }

    public bool GetUpAnalog() {
        return vertical > 0f;
    }

    public bool GetDownAnalog() {
        return vertical < 0f;
    }

    public bool GetLeftAnalog() {
        return horizontal < 0f;
    }

    public bool GetRightAnalog() {
        return horizontal > 0f;
    }

    public bool GetUpDpad() {
        return Mathf.RoundToInt(dPadY) == 1f;
    }

    public bool GetDownDpad() {
        return Mathf.RoundToInt(dPadY) == -1f;
    }

    public bool GetLeftDpad() {
        return Mathf.RoundToInt(dPadX) == -1f;
    }

    public bool GetRightDpad() {
        return Mathf.RoundToInt(dPadX) == 1f;
    }

    public bool GetUp() {
        return GetUpAnalog() || GetUpDpad();
    }

    public bool GetDown() {
        return GetDownAnalog() || GetDownDpad();
    }

    public bool GetLeft() {
        return GetLeftAnalog() || GetLeftDpad();
    }

    public bool GetRight() {
        return GetRightAnalog() || GetRightDpad();
    }
    public bool GetUpOneShot() {
        return (GetUpAnalog() && !verticalInUse) || (GetUpDpad() && !dPadYInUse);
    }

    public bool GetDownOneShot() {
        return (GetDownAnalog() && !verticalInUse) || (GetDownDpad() && !dPadYInUse);
    }

    public bool GetLeftOneShot() {
        return (GetLeftAnalog() && !horizontalInUse) || (GetLeftDpad() && !dPadXInUse);
    }

    public bool GetRightOneShot() {
        return (GetRightAnalog() && !horizontalInUse) || (GetRightDpad() && !dPadXInUse);
    }

    public void UpdateHorizontalInUse() {
        if (horizontal != 0f) {
            horizontalInUse = true;
        } else {
            horizontalInUse = false;
        }
    }

    public void UpdateVerticalInUse() {
        if (vertical != 0f) {
            verticalInUse = true;
        } else {
            verticalInUse = false;
        }
    }

    public void UpdateDPadXInUse() {
        if (dPadX != 0f) {
            dPadXInUse = true;
        } else {
            dPadXInUse = false;
        }
    }

    public void UpdateDPadYInUse() {
        if (dPadY != 0f) {
            dPadYInUse = true;
        } else {
            dPadYInUse = false;
        }
    }

    void Update() {
        UpdateHorizontalInUse();
        UpdateVerticalInUse();
        UpdateDPadXInUse();
        UpdateDPadYInUse();

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");
        jump = Input.GetButtonDown("Jump");
        dash = Input.GetButtonDown("Dash");
        fire1 = Input.GetButtonDown("Fire1");
        fire2 = Input.GetButtonDown("Fire2");
        fire3 = Input.GetButtonDown("Fire3");
        fire4 = Input.GetButtonDown("Fire4");
        fire5 = Input.GetButtonDown("Fire5");
        pause = Input.GetButtonDown("Pause");
        leftTrigger = Mathf.Max(Input.GetAxis("Left Trigger"), GetMouseRight());
        rightTrigger = Mathf.Max(Input.GetAxis("Right Trigger"), GetMouseLeft());
        dPadX = Input.GetAxisRaw("DPad X");
        dPadY = Input.GetAxisRaw("DPad Y");
    }
}
