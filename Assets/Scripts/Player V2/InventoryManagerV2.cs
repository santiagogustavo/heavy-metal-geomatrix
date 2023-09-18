using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerV2 : MonoBehaviour {
    PlayerMovementController playerController;
    Animator playerAnimator;

    Transform bodySlot;
    GameObject bodyInstance;

    Transform pickupSlot;
    GameObject pickupInstance;

    List<GameObject> fists;

    bool isDashing;
    bool isDropping;
    bool isShooting;
    bool isBursting;
    bool isAttacking;
    bool isAttackCombo;
    bool isDoubleJumping;

    int currentAnimation;
    bool lockShootingState;
    int burstCount;

    void Start() {
        playerController = GetComponent<PlayerMovementController>();
        playerAnimator = playerController.GetCharacterAnimator();
        bodySlot = Finder.GetGameObjectByName(transform, "Body Slot").transform;
        pickupSlot = Finder.GetGameObjectByName(transform, "Pickup Slot").transform;
        fists = Finder.GetGameObjectsByTagName(transform, "Fist");
    }

    void ClearAnimation() {
        playerController.SetIsPickingUp(false);
    }

    void ComputeDrop() {
        if (!pickupInstance) {
            return;
        }
        WeaponController weapon = pickupInstance?.GetComponent<WeaponController>();
        if (weapon) {
            isDropping = weapon.GetBulletCount() <= 0;
            return;
        }

        SwordController sword = pickupInstance?.GetComponent<SwordController>();
        if (sword) {
            isDropping = sword.isBroken;
            return;
        }
    }

    void UnlockShootingState() {
        lockShootingState = false;
    }

    void ComputeBurst() {
        if (burstCount <= 0 || !pickupInstance) {
            return;
        }

        WeaponController weapon = pickupInstance?.GetComponent<WeaponController>();
        if (weapon) {
            isBursting = true;
            burstCount--;
            Invoke("ComputeBurst", weapon.burstRate);
        }
    }

    void ControlShootingInputs() {
        if (!pickupInstance) {
            return;
        }
        WeaponController weapon = pickupInstance?.GetComponent<WeaponController>();
        if (isShooting && weapon) {
            lockShootingState = true;
            burstCount = weapon.burst;
            Invoke("UnlockShootingState", weapon.fireRate);
            Invoke("ComputeBurst", weapon.burstRate);
        }
    }

    void ComputeInputs() {
        if (GameManager.instance.IsGamePaused() || !GameManager.instance.MatchWasStarted()) {
            return;
        }
        isShooting = InputManager.instance.shootHold && !lockShootingState;
        isAttacking = InputManager.instance.shootTap;
        if (IsAttacking() && !IsLastComboAttack() && InputManager.instance.shootTap) {
            isAttackCombo = true;
        }
        isDashing = playerController.GetIsDashing();
        isDoubleJumping = playerController.GetIsDoubleJumping();
    }

    void PassVariablesToPlayer() {
        playerController.SetIsShooting(isShooting);
        playerController.SetIsBursting(isBursting);
        playerController.SetIsAttacking(isAttacking || isAttackCombo);
        playerController.SetIsDropping(isDropping);
    }

    void HandleJetpack() {
        if (!bodyInstance) {
            return;
        }
        JetController jetpack = bodyInstance.GetComponent<JetController>();
        jetpack.PlayJetStream(isDashing);

        if (isDoubleJumping) {
            jetpack.PlayJetBeam();
        }

        if (jetpack.fuel <= 0f) {
            playerController.SetJetpackHasFuel(false);
        }
    }

    void PickUpShoot() {
        if (!pickupInstance) {
            return;
        }
        isShooting = false;
        isBursting = false;
    }

    void UpdateComboBasedOnAnimation() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);

        if (currentAnimation != animationState.fullPathHash) {
            isAttackCombo = false;
        }

        currentAnimation = animationState.fullPathHash;
    }

    bool CanPickUp() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Look - Melee Light")
            || animationState.IsName("Look - Melee Heavy")
            || animationState.IsName("Look - Weapon Single")
            || animationState.IsName("Look - Weapon Double")
            || animationState.IsName("Look - Empty");
    }

    bool IsAttacking() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Attack - Punch 1")
            || animationState.IsName("Attack - Punch 2")
            || animationState.IsName("Attack - Punch 3")
            || animationState.IsName("Attack - Punch 4")
            || animationState.IsName("Attack - Melee Light 1")
            || animationState.IsName("Attack - Melee Light 2")
            || animationState.IsName("Attack - Melee Light 3")
            || animationState.IsName("Attack - Melee Light 4")
            || animationState.IsName("Attack - Melee Heavy 1")
            || animationState.IsName("Attack - Melee Heavy 2")
            || animationState.IsName("Attack - Melee Heavy 3")
            || animationState.IsName("Attack - Melee Heavy 4");
    }

    bool IsLastComboAttack() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Attack - Punch 4")
            || animationState.IsName("Attack - Melee Light 4")
            || animationState.IsName("Attack - Melee Heavy 4");
    }

    void ControlFists() {
        if (pickupInstance) {
            return;
        }
        foreach (GameObject fist in fists) {
            FistController fistController = fist.GetComponent<FistController>();
            fistController?.SetTrailActive(IsAttacking());
        }
    }

    void PickUpSword() {
        if (!pickupInstance) {
            return;
        }
        SwordController sword = pickupInstance.GetComponent<SwordController>();
        sword?.SetOwner(name);
        sword?.SetTrailActive(IsAttacking());
    }

    void PickUpDrop() {
        if (!isDropping || !pickupInstance) {
            return;
        }
        isDropping = false;
        playerController.SetEquip(EquipType.Body);
    }

    void PickUpEquip(GameObject item) {
        Destroy(pickupInstance);
        pickupInstance = Instantiate(
            item,
            new Vector3(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, 0f)
        );
        pickupInstance.transform.SetParent(pickupSlot, false);
    }
    
    void PickUpBody(GameObject item) {
        Destroy(bodyInstance);
        bodyInstance = Instantiate(
            item,
            new Vector3(0f, 0f, 0f),
            Quaternion.Euler(0f, 0f, 0f)
        );
        bodyInstance.transform.SetParent(bodySlot, false);
    }

    public bool PickUpItem(EquipType equip, GameObject item, bool playAnimation) {
        if (!CanPickUp() && playAnimation) {
            return false;
        }

        if (playAnimation) {
            playerController.SetIsPickingUp(true);
            Invoke("ClearAnimation", 0.05f);
        }

        if (equip != EquipType.Body) {
            playerController.SetEquip(equip);
            PickUpEquip(item);
        } else {
            playerController.SetHasJetpack(true);
            playerController.SetJetpackHasFuel(true);
            PickUpBody(item);
        }

        return true;
    }

    void Update() {
        ComputeInputs();
        ComputeDrop();
        ControlShootingInputs();
        PassVariablesToPlayer();
        UpdateComboBasedOnAnimation();
        ControlFists();
        PickUpShoot();
        PickUpSword();
        PickUpDrop();
        HandleJetpack();
    }

    public GameObject GetCurrentPickup() {
        return pickupInstance;
    }
}
