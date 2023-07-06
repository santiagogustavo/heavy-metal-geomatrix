using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {
    [SerializeField]
    GameObject sparks;

    [SerializeField]
    ParticleSystem trail;

    [SerializeField]
    GameObject swordHitSfx;

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
            Instantiate(swordHitSfx, contactPoint, rotation);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer != 3 || !isPlaying) {
            return;
        }
        InstantiateSparks(collision, true);
    }

    void OnCollisionStay(Collision collision) {
        if (collision.gameObject.layer != 3 || !isPlaying) {
            return;
        }
        InstantiateSparks(collision);
    }
}
