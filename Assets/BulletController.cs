using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    float speed = 50f;
    Rigidbody rb;
    GameObject sparks;
    GameObject bulletHole;
    List<GameObject> hitSfxList;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        sparks = Resources.Load("Particles/Gun Sparks") as GameObject;
        bulletHole = Resources.Load("Weapons/Bullet Hole") as GameObject;
        hitSfxList = new List<GameObject>();
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 1") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 2") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 3") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 4") as GameObject);
    }

    void InstantiateHitLevelGeometry(Collision collision) {
        int randomSfx = Random.Range(0, hitSfxList.Count);
        Instantiate(hitSfxList[randomSfx], collision.transform.position, collision.transform.rotation);

        ContactPoint contactPoint = collision.contacts[0];
        Vector3 position = contactPoint.point + (contactPoint.normal * 0.1f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        Quaternion sparksRotation = Quaternion.FromToRotation(-Vector3.forward, contactPoint.normal);
        Instantiate(sparks, position, sparksRotation);
        Instantiate(bulletHole, position, rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Invisible Wall") {
            InstantiateHitLevelGeometry(collision);
        }
        Destroy(gameObject);
    }
}
