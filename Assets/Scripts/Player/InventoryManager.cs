using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private ThirdPersonController playerController;
    private Rumbler rumbler;

    GameObject bodySlot;
    string bodySlotItem;

    bool hasPlayedJumpAnimation = false;
    bool hasPlayedDashAnimation = false;

    void Awake() {
        playerController = GetComponent<ThirdPersonController>();
        rumbler = GetComponent<Rumbler>();
    }

    public bool HasJetpack() {
        return bodySlotItem == "Items/Jetpack";
    }

    public JetController GetJetpack() {
        return bodySlot?.GetComponent<JetController>();
    }

    Transform GetTransform(string name) {
        foreach (Transform child in gameObject.GetComponentInChildren<Transform>()) {
            if (child.name == name) {
                return child;
            }
        }
        return transform;
    }

    public void PickUpItem(string slot, string item) {
        rumbler.RumbleConstant(1f, 1f, 0.2f);

        if (slot == "Body") {
            if (bodySlotItem == item) {
                if (HasJetpack()) {
                    GetJetpack().PlayJetBeam();
                }
                return;
            }

            bodySlotItem = item;
            bodySlot = Instantiate(
                Resources.Load(item) as GameObject,
                new Vector3(0f, -0.1f, -0.25f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            bodySlot.transform.SetParent(GetTransform("Body Slot"), false);   
        }
    }

    private void UpdateJump(JetController jetpack) {
        if (!jetpack) {
            return;
        }
        if (playerController.hasDoubleJumped) {
            if (!hasPlayedJumpAnimation) {
                jetpack.PlayJetBeam();
                hasPlayedJumpAnimation = true;
            }
        } else {
            hasPlayedJumpAnimation = false;
        }
    }

    private void UpdateDash(JetController jetpack) {
        if (!jetpack) {
            return;
        }
        if (playerController.isDashing) {
            if (!hasPlayedDashAnimation) {
                hasPlayedDashAnimation = true;
                jetpack.PlayJetStream(true);
            }
        } else {
            if (hasPlayedDashAnimation) {
                hasPlayedDashAnimation = false;
                jetpack.PlayJetStream(false);
            }
        }
    }

    private void Update() {
        JetController jetpack = GetJetpack();
        UpdateJump(jetpack);
        UpdateDash(jetpack);
    }
}
