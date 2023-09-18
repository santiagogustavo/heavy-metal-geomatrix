using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
    [SerializeField] GameObject playerHitEffect;
    [SerializeField] GameObject playerHitSfx;
    [SerializeField] float damageApplied;
    [SerializeField] float speed = 50f;

    Rigidbody rb;
    GameObject sparks;
    GameObject bulletHole;
    List<GameObject> hitSfxList;
    Vector3 target;

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

    public void SetTarget(Vector3 at) {
        target = at;
    }

    void InstantiateHitPlayer(Collision collision) {
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 position = contactPoint.point + (contactPoint.normal * 0.1f);
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
        Instantiate(playerHitEffect, position, rotation);
        Instantiate(playerHitSfx, position, rotation);
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

        /* LEVEL GEOMETRY */
        if (collision.gameObject.layer == 3 && collision.gameObject.tag != "Invisible Wall") {
            collision.gameObject.GetComponent<BreakableProp>()?.InflictDamage(damageApplied);
            InstantiateHitLevelGeometry(collision);
        }

        /* CHARACTER */
        if (collision.gameObject.layer == 12) {
            PlayerComboController.instance.ComboHit();
            InstantiateHitPlayer(collision);
        }
        Destroy(gameObject);
    }
}
