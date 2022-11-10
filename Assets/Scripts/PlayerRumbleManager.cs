using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRumbleManager : MonoBehaviour {
    InventoryManager inventory;
    ThirdPersonController controller;

    bool isRumblingJump;
    bool isDashing;

    void Awake() {
        inventory = GetComponent<InventoryManager>();
        controller = GetComponent<ThirdPersonController>();
    }

    void Update() {
        if (controller.hasDoubleJumped) {
            if (!isRumblingJump) {
                Rumbler.instance.RumbleConstant(1f, 1f, 0.2f);
                isRumblingJump = true;
            }
        } else {
            isRumblingJump = false;
        }

        if (controller.isDashing && inventory.HasJetpack()) {
            Rumbler.instance.RumblePulse(0.25f, 0.5f, 0.1f, 0.1f);
        }
    }
}
