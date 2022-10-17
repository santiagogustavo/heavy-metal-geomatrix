using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public float horizontal;
    public float vertical;
    public bool jump;
    public bool fire3;

    public float leftTrigger;

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");
        fire3 = Input.GetButtonDown("Fire3");
        leftTrigger = Input.GetAxis("Left Trigger");
    }
}
