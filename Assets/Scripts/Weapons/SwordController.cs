using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {
    [SerializeField]
    GameObject sparks;

    [SerializeField]
    GameObject hit;

    [SerializeField]
    ParticleSystem trail;

    [SerializeField]
    GameObject levelHitSfx;

    [SerializeField]
    GameObject playerHitSfx;

    [SerializeField]
    float damageApplied;

    public float health = 100f;

    public bool isBroken = false;
    public bool isPlaying = false;
    public bool canInflictDamage = false;

    public int currentAnimation;

    public void SetTrailActive(bool active) {
        if (active) {
            if (!isPlaying) {
                isPlaying = true;
                trail.Play();
            }
        } else {
            isPlaying = false;
            canInflictDamage = false;
            trail.Stop();
        }
    }

    void InstantiateSparks(Collision collision, bool hit = false) {
        ContactPoint contactPoint = collision.contacts[0];
        Vector3 position = contactPoint.point + (contactPoint.normal * 0.1f);
        Quaternion rotation = Quaternion.FromToRotation(-Vector3.forward, contactPoint.normal);
        Instantiate(sparks, position, rotation);

        if (hit) {
            Instantiate(levelHitSfx, position, rotation);
        }
    }

    void InstantiateImpact(Collision collision) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(hit, contactPoint, rotation);
        Instantiate(playerHitSfx, contactPoint, rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if (!isPlaying || health <= 0) {
            return;
        }

        if (collision.gameObject.layer == 3) {
            if (canInflictDamage) {
                collision.gameObject.GetComponent<BreakableProp>()?.InflictDamage(damageApplied);
                canInflictDamage = false;
            }
            InstantiateSparks(collision, true);
        }
        if (collision.gameObject.layer == 6) {
            if (canInflictDamage) {
                health -= damageApplied;
                if (health < 0f) {
                    health = 0f;
                }
                canInflictDamage = false;
                InstantiateImpact(collision);
            }
        }
    }

    void OnCollisionStay(Collision collision) {
        if (!isPlaying) {
            return;
        }
        if (collision.gameObject.layer == 3) {
            InstantiateSparks(collision);
        }
    }

    void OnCollisionExit() {
        if (!isPlaying) {
            return;
        }
        if (health <= 0) {
            isBroken = true;
        }
    }
}
