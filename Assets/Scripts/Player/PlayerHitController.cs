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
        return !!sword && sword.isPlaying;
    }

    void InstantiateSpill(Collision collision, bool hit = false) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(blood, contactPoint, rotation, collision.transform);

        if (hit) {
            Instantiate(sfx, contactPoint, rotation, collision.transform);
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
