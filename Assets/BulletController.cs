using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    GameObject sparks;
    GameObject bulletHole;
    List<GameObject> hitSfxList;

    void Start() {
        sparks = Resources.Load("Particles/Gun Sparks") as GameObject;
        bulletHole = Resources.Load("Particles/Bullet Hole") as GameObject;
        hitSfxList = new List<GameObject>();
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 1") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 2") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 3") as GameObject);
        hitSfxList.Add(Resources.Load("Sounds/Weapons/Common/Bullet Hit 4") as GameObject);
    }

    void InstantiateHitLevelGeometry(Collision collision) {
        int randomSfx = Random.Range(0, hitSfxList.Count);
        Instantiate(hitSfxList[randomSfx], collision.transform.position, collision.transform.rotation);

        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(sparks, contactPoint, rotation);
        Instantiate(bulletHole, contactPoint, rotation, collision.gameObject.transform);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 3) {
            InstantiateHitLevelGeometry(collision);
        }
        Destroy(gameObject);
    }
}
