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

    public bool isPlaying = false;

    public void SetTrailActive(bool active) {
        if (active) {
            if (!isPlaying) {
                isPlaying = true;
                trail.Play();
            }
        } else {
            isPlaying = false;
            trail.Stop();
        }
    }

    void InstantiateSparks(Collision collision, bool hit = false) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(sparks, contactPoint, rotation, collision.transform);

        if (hit) {
            Instantiate(levelHitSfx, contactPoint, rotation);
        }
    }

    void InstantiateImpact(Collision collision) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(hit, contactPoint, rotation, collision.transform);
        Instantiate(playerHitSfx, contactPoint, rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if (!isPlaying) {
            return;
        }
        if (collision.gameObject.layer == 3) {
            collision.gameObject.GetComponent<BreakableProp>()?.InflictDamage(damageApplied);
            InstantiateSparks(collision, true);
        }
        if (collision.gameObject.layer == 6) {
            InstantiateImpact(collision);
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
}
