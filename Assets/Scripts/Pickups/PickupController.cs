using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour {
    private Animator animator;

    public bool pickupOnPress = false;
    public string slot;
    public GameObject item;
    public GameObject pickupEffect;

    bool isInside = false;
    GameObject collisionObject;

    Color colorStart = Color.white;
    Color colorEnd = Color.red;
    float duration = 0.2f;
    float lerp;

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
        playerInventory.PickUpItem(slot, item, pickupOnPress);
        Destroy(gameObject);

        if (pickupEffect) {
            Instantiate(
                pickupEffect,
                transform.position,
                Quaternion.identity
            );
        }
    }
    void LerpCollisionMaterial() {
        lerp = Mathf.PingPong(Time.time, duration) / duration;
        SetCollisionMaterial(transform);
    }

    void SetCollisionMaterial(Transform parent) {
        foreach (Transform obj in parent.transform) {
            obj.GetComponentInChildren<Renderer>()?.material.SetColor("_Color", Color.Lerp(colorStart, colorEnd, lerp));
            SetCollisionMaterial(obj);
        }
    }

    void ClearCollisionMaterial(Transform parent) {
        foreach (Transform obj in parent.transform) {
            obj.GetComponentInChildren<Renderer>()?.material.SetColor("_Color", Color.white);
            ClearCollisionMaterial(obj);
        }
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
        if (other.gameObject.tag != "Player") {
            return;
        }

        ClearCollisionMaterial(transform);
        isInside = false;
        collisionObject = null;
    }

    void Update() {
        if (!pickupOnPress) {
            return;
        }

        if (isInside) {
            LerpCollisionMaterial();

            if (Input.GetButtonDown("Fire2")) {
                PickUpItemAndDestroy();
            }
        }
    }
}
