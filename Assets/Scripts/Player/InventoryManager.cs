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

        List<GameObject> characterSlots = Finder.GetGameObjectsByTagName(transform, "Character Slot");
        bodySlot = Finder.GetTransformFromGameObjects(characterSlots, "Body Slot");
        leftHandSlot = Finder.GetTransformFromGameObjects(characterSlots, "Left Hand Slot");
        rightHandSlot = Finder.GetTransformFromGameObjects(characterSlots, "Right Hand Slot");
    }

    public bool HasJetpack() {
        return bodySlotName == "Jetpack";
    }

    public JetController GetJetpack() {
        return bodyItem?.GetComponent<JetController>();
    }

    public SwordController GetSword() {
        return rightHandItem?.GetComponent<SwordController>();
    }

    private void ResetVariables() {
        hasPlayedDashAnimation = false;
    }

    public void PickUpItem(string slot, GameObject item, bool playAnimation) {
        if (playAnimation) {
            animationController.ChangeAnimationState("Pickup");
        }
        Rumbler.instance.RumbleConstant(1f, 1f, 0.2f);
        
        if (slot == "Body") {
            Destroy(bodyItem);
            ResetVariables();

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

    private void UpdateSwordTrail(SwordController sword) {
        if (!sword) {
            return;
        }
        if (animationController.IsAttacking()) {
            sword.SetTrailActive(true);
        } else {
            sword.SetTrailActive(false);
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
        if (InputManager.instance.shootTap) {
            Shoot();
            return;
        }
        hasPlayedShootAnimation = false;
        animationController.ChangeAnimationState("Idle", 0.1f);
    }

    private void Sword() {
        if (animationController.IsCurrentAnimation("Sword 1")) {
            //playerController.PushForward(10f);
            animationController.ChangeAnimationState("Sword 2", 0.05f);
        } else if (!animationController.IsAttacking()) {
            //playerController.PushForward(10f);
            animationController.ChangeAnimationState("Sword 1", 0, true);
        }
    }

    private void Shoot() {
        weaponBurstCount++;
        WeaponController weapon = leftHandItem.GetComponent<WeaponController>();
        hasPlayedShootAnimation = true;
        animationController.ChangeAnimationState("Shoot 1", 0.05f, true);
        weapon.Shoot(weapon.transform.position);


        if (weaponBurstCount == weapon.burst) {
            Invoke("UnlockShoot", weapon.burstRate);
        } else {
            Invoke("Shoot", weapon.fireRate);
        }
    }

    private void Update() {
        UpdateAnimatorActiveLayer();
        JetController jetpack = GetJetpack();
        UpdateJump(jetpack);
        UpdateDash(jetpack);

        SwordController sword = GetSword();
        UpdateSwordTrail(sword);

        if (!animationController.IsAttacking()) {
            animationController.SetCombo(false);
        }

        if (animationController.IsPickingUp()) {
            return;
        }

        if (
            leftHandSlotName != null
            && !hasPlayedShootAnimation
            && InputManager.instance.shootTap
        ) {
            Shoot();
        }

        if (
            rightHandSlotName != null
            && InputManager.instance.pickup
        ) {
            Sword();
        }
    }
}
