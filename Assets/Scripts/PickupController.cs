using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {
    private Animator animator;

    public bool pickupOnPress = false;
    public string slot;
    public string item;

    bool isInside = false;
    GameObject collisionObject;

    void Awake() {
        animator = GetComponent<Animator>();
        Invoke("DestroyPickup", 5f);
    }

    void DestroyPickup() {
        animator.Play("Destroy");
        Destroy(gameObject, 0.75f);
    }

    void PickUpItemAndDestroy() {
        InventoryManager playerInventory = collisionObject.GetComponent<InventoryManager>();
        playerInventory.PickUpItem(slot, item);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag != "Player") {
            return;
        }
        collisionObject = collider.gameObject;

        if (!pickupOnPress) {
            PickUpItemAndDestroy();
        } else {
            isInside = true;
        }
    }

    void OnTriggerExit(Collider other) {
        isInside = false;
        collisionObject = null;
    }

    void Update() {
        if (!pickupOnPress) {
            return;
        }
        if (isInside && Input.GetButtonDown("Fire3")) {
            PickUpItemAndDestroy();
        }
    }
}
