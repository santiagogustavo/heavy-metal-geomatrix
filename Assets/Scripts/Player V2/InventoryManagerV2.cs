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

    bool isDashing;
    bool isDropping;
    bool isShooting;
    bool isAttacking;
    bool isAttackCombo;
    bool isDoubleJumping;

    void Start() {
        playerController = GetComponent<PlayerMovementController>();
        playerAnimator = playerController.GetCharacterAnimator();
        bodySlot = Finder.GetGameObjectByName(transform, "Body Slot").transform;
        pickupSlot = Finder.GetGameObjectByName(transform, "Pickup Slot").transform;
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
            isDropping = weapon.bulletCount <= 0;
        }

        SwordController sword = pickupInstance?.GetComponent<SwordController>();
        if (sword) {
            isDropping = sword.isBroken;
        }
    }

    void ComputeInputs() {
        isShooting = InputManager.instance.shootHold;
        isAttacking = InputManager.instance.shootTap;
        if (IsSwordAttacking() && !IsLastComboAttack() && InputManager.instance.shootTap) {
            isAttackCombo = true;
        }
        isDashing = playerController.GetIsDashing();
        isDoubleJumping = playerController.GetIsDoubleJumping();
    }

    void PassVariablesToPlayer() {
        playerController.SetIsShooting(isShooting);
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
        if (!isShooting || !pickupInstance) {
            return;
        }
        isShooting = false;
    }

    void PassAnimationInfoToSword() {
        if (!pickupInstance) {
            return;
        }

        SwordController sword = pickupInstance?.GetComponent<SwordController>();
        if (sword) {
            AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);

            if (sword.currentAnimation != animationState.fullPathHash) {
                isAttackCombo = false;
            }
            sword.currentAnimation = animationState.fullPathHash;
        }
    }

    bool CanPickUp() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Look - Melee Light")
            || animationState.IsName("Look - Melee Heavy")
            || animationState.IsName("Look - Weapon Single")
            || animationState.IsName("Look - Weapon Double")
            || animationState.IsName("Look - Empty");
    }

    bool IsSwordAttacking() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Attack - Melee Light 1")
            || animationState.IsName("Attack - Melee Light 2")
            || animationState.IsName("Attack - Melee Light 3")
            || animationState.IsName("Attack - Melee Light 4")
            || animationState.IsName("Attack - Melee Heavy 1");
    }

    bool IsLastComboAttack() {
        AnimatorStateInfo animationState = playerAnimator.GetCurrentAnimatorStateInfo(1);
        return animationState.IsName("Attack - Melee Light 4")
            || animationState.IsName("Attack - Melee Heavy 1");
    }

    void PickUpSword() {
        if (!pickupInstance) {
            return;
        }
        SwordController sword = pickupInstance.GetComponent<SwordController>();
        sword?.SetTrailActive(IsSwordAttacking());
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
        PassVariablesToPlayer();
        PassAnimationInfoToSword();
        PickUpShoot();
        PickUpSword();
        PickUpDrop();
        HandleJetpack();
    }

    public GameObject GetCurrentPickup() {
        return pickupInstance;
    }
}