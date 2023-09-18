using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistController : MonoBehaviour {
    [SerializeField]
    GameObject hit;

    [SerializeField]
    GameObject playerHitSfx;

    [SerializeField]
    float damageApplied;

    ParticleSystem trail;

    public bool isPlaying = false;
    public bool canInflictDamage = false;

    private void Start() {
        trail = GetComponent<ParticleSystem>();
    }

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

    void InstantiateImpact(Collision collision) {
        Vector3 contactPoint = collision.contacts[0].point;
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, contactPoint);
        Instantiate(hit, contactPoint, rotation);
        Instantiate(playerHitSfx, contactPoint, rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if (!isPlaying) {
            return;
        }

        /* CHARACTER */
        if (collision.gameObject.layer == 12) {
            if (canInflictDamage) {
                canInflictDamage = false;
                PlayerComboController.instance.ComboHit();
                InstantiateImpact(collision);
            }
        }
    }
}
