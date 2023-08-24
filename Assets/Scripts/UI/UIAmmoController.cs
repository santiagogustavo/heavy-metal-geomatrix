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
        } else if (pickup && text) {
            WeaponController weapon = pickup?.GetComponent<WeaponController>();
            SwordController sword = pickup?.GetComponent<SwordController>();

            if (weapon) {
                text.text = weapon.bulletCount + "/" + weapon.bullets;
            } else if (sword) {
                text.text = sword.health + "%";
            } else {
                text.text = "";
            }
        }
    }
}
