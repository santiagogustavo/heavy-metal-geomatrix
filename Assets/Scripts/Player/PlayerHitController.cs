using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitController : MonoBehaviour {
    GameObject blood;
    GameObject sfx;

    void Start() {
        blood = Resources.Load("Particles/Blood Spill") as GameObject;
        sfx = Resources.Load("Sounds/Blood Spill") as GameObject;
    }

    bool CheckIfWeaponIsAttacking(Collision collision) {
        SwordController sword = collision.gameObject.GetComponent<SwordController>();
        BulletController bullet = collision.gameObject.GetComponent<BulletController>();
        return !!bullet || (!!sword && sword.isPlaying);
    }

    void InstantiateSpill(Collision collision, bool hit = false) {
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 position = contactPoint.point + (contactPoint.normal * 0.1f);
        Quaternion rotation = Quaternion.FromToRotation(-Vector3.forward, contactPoint.normal);
        Instantiate(blood, position, rotation, collision.transform);

        if (hit) {
            Instantiate(sfx, position, rotation, collision.transform);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != 10 || !CheckIfWeaponIsAttacking(collision)) {
            return;
        }
        InstantiateSpill(collision, true);
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer != 10 || !CheckIfWeaponIsAttacking(collision)) {
            return;
        }
        InstantiateSpill(collision);
    }
}
