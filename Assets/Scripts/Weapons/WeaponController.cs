using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    ParticleSystem particles;
    AudioSource sfx;

    [SerializeField]
    public int bullets;

    [SerializeField]
    public int bulletCount;

    [SerializeField]
    public float fireRate;

    [SerializeField]
    public int burst;

    [SerializeField]
    public float burstRate;

    [SerializeField]
    Transform bulletHole;

    [SerializeField]
    GameObject bullet;

    void Start() {
        bulletCount = bullets;
        particles = GetComponentInChildren<ParticleSystem>();
        sfx = GetComponentInChildren<AudioSource>();
    }

    public void InstantiateBullet(Vector3 at) {
        Vector3 aimDirection = (at - bulletHole.position).normalized;
        Instantiate(bullet, bulletHole.transform.position, Quaternion.LookRotation(aimDirection, Vector3.up));
    }

    public void Shoot(Vector3 at) {
        if (bulletCount == 0) {
            return;
        }
        bulletCount--;
        particles.Play();
        sfx.Play();
        InstantiateBullet(at);
    }
}
