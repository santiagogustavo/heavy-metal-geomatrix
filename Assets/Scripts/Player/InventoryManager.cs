using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    private ThirdPersonController playerController;
    private AnimationController animationController;

    Transform bodySlot;
    Transform leftHandSlot;
    Transform rightHandSlot;

    GameObject bodyItem;
    GameObject leftHandItem;
    GameObject rightHandItem;

    string bodySlotName;
    string leftHandSlotName;
    string rightHandSlotName;

    bool hasPlayedJumpAnimation = false;
    bool hasPlayedDashAnimation = false;
    bool hasPlayedShootAnimation = false;

    int weaponBurstCount = 0;

    void Awake() {
        playerController = GetComponent<ThirdPersonController>();
        animationController = GetComponentInChildren<AnimationController>();

        List<GameObject> characterSlots = GetGameObjectsByTagName("Character Slot");
        bodySlot = GetTransformFromGameObjects(characterSlots, "Body Slot");
        leftHandSlot = GetTransformFromGameObjects(characterSlots, "Left Hand Slot");
        rightHandSlot = GetTransformFromGameObjects(characterSlots, "Right Hand Slot");
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
        return bodyItem?.GetComponent<JetController>();
    }

    public void PickUpItem(string slot, GameObject item, bool playAnimation) {
        if (playAnimation) {
            animationController.ChangeAnimationState("Pickup");
        }
        Rumbler.instance.RumbleConstant(1f, 1f, 0.2f);

        if (slot == "Body") {
            Destroy(bodyItem);

            bodySlotName = item.name;
            bodyItem = Instantiate(
                item,
                new Vector3(0f, -0.1f, -0.25f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            bodyItem.transform.SetParent(bodySlot, false);
        } else if (slot == "Left Hand") {
            Destroy(leftHandItem);

            leftHandSlotName = item.name;
            leftHandItem = Instantiate(
                item,
                new Vector3(0f, 0f, 0f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            leftHandItem.transform.SetParent(leftHandSlot, false);
        } else if (slot == "Right Hand") {
            Destroy(rightHandItem);

            rightHandSlotName = item.name;
            rightHandItem = Instantiate(
                item,
                new Vector3(0f, 0f, 0f),
                Quaternion.Euler(0f, 0f, 0f)
            );
            rightHandItem.transform.SetParent(rightHandSlot, false);
        }
    }

    private void UpdateAnimatorActiveLayer() {
        int layer = 0;

        if (rightHandItem && !leftHandItem) {
            layer = 1;
        } else if (leftHandItem) {
            layer = 2;
        }

        if (animationController.GetActiveLayer() != layer) {
            animationController.ChangeAnimatorLayer(layer);
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

    void UnlockShoot() {
        weaponBurstCount = 0;
        if (InputManager.instance.rightTrigger == 1f) {
            Shoot();
            return;
        }
        hasPlayedShootAnimation = false;
        playerController.isShooting = false;
        animationController.ChangeAnimationState("Idle", 0.1f);
    }

    private void Shoot() {
        weaponBurstCount++;
        WeaponController weapon = leftHandItem.GetComponent<WeaponController>();
        hasPlayedShootAnimation = true;
        playerController.isShooting = true;
        animationController.ChangeAnimationState("Shoot 1", 0.05f, true);
        weapon.Shoot();

        if (weaponBurstCount == weapon.burst) {
            Invoke("UnlockShoot", weapon.repeatRate);
        } else {
            Invoke("Shoot", weapon.fireRate);
        }
    }

    private void Update() {
        UpdateAnimatorActiveLayer();
        JetController jetpack = GetJetpack();
        UpdateJump(jetpack);
        UpdateDash(jetpack);

        if (
            leftHandSlotName != null
            && !hasPlayedShootAnimation
            && animationController.IsIdlingOrWalkingOrDashing()
            && InputManager.instance.rightTrigger == 1
        ) {
            Shoot();
        }
    }
}
