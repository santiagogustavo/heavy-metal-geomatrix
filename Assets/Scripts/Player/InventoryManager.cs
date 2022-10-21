using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private ThirdPersonController playerController;
    private Rumbler rumbler;

    Transform characterBodySlot;
    Transform characterLeftHandSlot;
    Transform characterRightHandSlot;

    GameObject bodySlot;
    GameObject leftHandSlot;
    GameObject rightHandSlot;
    string bodySlotName;
    string leftHandSlotName;
    string rightHandSlotName;

    bool hasPlayedJumpAnimation = false;
    bool hasPlayedDashAnimation = false;

    void Awake() {
        playerController = GetComponent<ThirdPersonController>();
        rumbler = GetComponent<Rumbler>();

        List<GameObject> characterSlots = GetGameObjectsByTagName("Character Slot");
        characterBodySlot = GetTransformFromGameObjects(characterSlots, "Body Slot");
        characterLeftHandSlot = GetTransformFromGameObjects(characterSlots, "Left Hand Slot");
        characterRightHandSlot = GetTransformFromGameObjects(characterSlots, "Right Hand Slot");
    }

    private List<GameObject> GetGameObjectsByTagName(string tag) {
        List<GameObject> found = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag)).FindAll(g => g.transform.IsChildOf(transform));
        return found;
    }

    private Transform GetTransformFromGameObjects(List<GameObject> gameObjects, string name) {
        return gameObjects.Find(g => g.name == name)?.transform;
    }

    public bool HasJetpack() {
        return bodySlotName == "Jetpack";
    }

    public JetController GetJetpack() {
        return bodySlot?.GetComponent<JetController>();
    }

    public void PickUpItem(string slot, GameObject item) {
        rumbler.RumbleConstant(1f, 1f, 0.2f);

        if (slot == "Body") {
            if (bodySlotName == item.name) {
                if (HasJetpack()) {
                    GetJetpack().PlayJetBeam();
                }
                return;
            }

            bodySlotName = item.name;
            bodySlot = Instantiate(
                item,
                new Vector3(0f, -0.1f, -0.25f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            bodySlot.transform.SetParent(characterBodySlot, false);
        } else if (slot == "Left Hand") {
            leftHandSlotName = item.name;
            leftHandSlot = Instantiate(
                item,
                new Vector3(0f, 0f, 0f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            leftHandSlot.transform.SetParent(characterLeftHandSlot, false);
        } else if (slot == "Right Hand") {
            rightHandSlotName = item.name;
            rightHandSlot = Instantiate(
                item,
                new Vector3(0f, 0f, 0f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            rightHandSlot.transform.SetParent(characterRightHandSlot, false);
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
