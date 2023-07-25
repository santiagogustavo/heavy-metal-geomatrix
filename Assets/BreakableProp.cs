using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProp : MonoBehaviour {
    [SerializeField]
    GameObject pristineObject;
    
    [SerializeField]
    GameObject brokenObject;

    [SerializeField]
    float health;

    void Start() {
        pristineObject.SetActive(true);
        brokenObject.SetActive(false);
    }

    void BreakObject() {
        Destroy(gameObject.GetComponent<MeshCollider>());
        pristineObject.SetActive(false);
        brokenObject.SetActive(true);
        health = 0;
    }

    public void InflictDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            BreakObject();
        }
    }
}
