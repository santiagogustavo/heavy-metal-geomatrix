using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerV2 : MonoBehaviour {
    PlayerMovementController playerController;

    Transform bodySlot;
    GameObject bodyInstance;

    Transform pickupSlot;
    GameObject pickupInstance;

    bool isDashing;
    bool isShooting;
    bool isDoubleJumping;

    void Start() {
        playerController = GetComponent<PlayerMovementController>();
        bodySlot = GetGameObjectByName("Body Slot").transform;
        pickupSlot = GetGameObjectByName("Pickup Slot").transform;
    }

    private GameObject GetGameObjectByName(string name) {
        GameObject found = GameObject.Find(name);
        return found.transform.IsChildOf(transform) ? found : null;
    }

    void ClearAnimation() {
        playerController.SetIsPickingUp(false);
    }

    void ComputeInputs() {
        isShooting = InputManager.instance.rightTrigger > 0f;
        isDashing = playerController.GetIsDashing();
        isDoubleJumping = playerController.GetIsDoubleJumping();

        playerController.SetIsShooting(isShooting);
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
    }

    void PickupShoot() {
        if (!isShooting || !pickupInstance) {
            return;
        }
        WeaponController weapon = pickupInstance.GetComponent<WeaponController>();
        weapon?.Shoot();

        isShooting = false;
    }

    void PickupSword() {
        if (!pickupInstance) {
            return;
        }
        SwordController sword = pickupInstance.GetComponent<SwordController>();
        sword?.SetTrailActive(false);
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

    public void PickUpItem(EquipType equip, GameObject item, bool playAnimation) {
        if (playAnimation) {
            playerController.SetIsPickingUp(true);
            Invoke("ClearAnimation", 0.05f);
        }

        if (equip != EquipType.Body) {
            playerController.SetEquip(equip);
            PickUpEquip(item);
        } else {
            playerController.SetHasJetpack(true);
            PickUpBody(item);
        }
    }

    void Update() {
        ComputeInputs();
        PickupShoot();
        PickupSword();
        HandleJetpack();
    }
}
