using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleController : MonoBehaviour {
    [SerializeField] float life = 10f;

    void Start() {
        Invoke("DestroyInstance", life);
    }

    void DestroyInstance() {
        Destroy(gameObject);
    }
}
