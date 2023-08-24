using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisController : MonoBehaviour {
    [SerializeField]
    GameObject sfx;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != 3 || collision.gameObject.GetComponent<DebrisController>()) {
            return;
        }
        Instantiate(sfx, collision.contacts[0].point, collision.transform.rotation, transform);
    }
}
