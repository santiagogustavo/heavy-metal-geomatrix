using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAmmoController : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    InventoryManagerV2 inventory;
    GameObject pickup;

    void Start() {
        FetchInventory();
    }

    void FetchInventory() {
        GameObject player = GameManager.instance.GetCurrentPlayerInstance();
        inventory = player?.GetComponent<InventoryManagerV2>();
        pickup = inventory?.GetCurrentPickup();
    }

    void Update() {
        FetchInventory();
        if (!pickup && text) {
            text.text = "";
        } else if (pickup) {
            WeaponController weapon = pickup?.GetComponent<WeaponController>();

            if (weapon && text) {
                text.text = weapon.bulletCount + "/" + weapon.bullets; 
            } else if (text) {
                text.text = "";
            }
        }
    }
}
