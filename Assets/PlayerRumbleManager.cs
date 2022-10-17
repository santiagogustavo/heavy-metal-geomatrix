using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRumbleManager : MonoBehaviour {
    InventoryManager inventory;
    ThirdPersonController controller;
    Rumbler rumbler;

    bool isRumblingJump;
    bool isDashing;

    void Awake() {
        inventory = GetComponent<InventoryManager>();
        controller = GetComponent<ThirdPersonController>();
        rumbler = GetComponent<Rumbler>();
    }

    void Update() {
        if (controller.hasDoubleJumped) {
            if (!isRumblingJump) {
                rumbler.RumbleConstant(1f, 1f, 0.2f);
                isRumblingJump = true;
            }
        } else {
            isRumblingJump = false;
        }

        if (controller.isDashing && inventory.HasJetpack()) {
            rumbler.RumblePulse(0.25f, 0.5f, 0.1f, 0.1f);
        }
    }
}
