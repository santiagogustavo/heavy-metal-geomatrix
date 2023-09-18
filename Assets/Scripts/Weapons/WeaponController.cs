using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    [SerializeField]
    ParticleSystem shootParticles;

    [SerializeField]
    AudioSource shootSfx;

    [SerializeField]
    Animator animator;

    [SerializeField]
    public int bullets;
    int bulletCount = -1;

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

    public bool isShooting;

    void Start() {
        bulletCount = bullets;
    }

    void Update() {
        animator.SetBool("IsShooting", isShooting);
        isShooting = false;
    }

    public int GetBulletCount() {
        return bulletCount >= 0 ? bulletCount : bullets;
    }

    public void InstantiateBullet(Vector3 at) {
        Vector3 aimDirection = at - bulletHole.position;
        GameObject bulletInstance = Instantiate(bullet, bulletHole.position, Quaternion.LookRotation(aimDirection, Vector3.up));
        bulletInstance.GetComponent<BulletController>().SetTarget(at);
    }

    public void Shoot(Vector3 at) {
        if (bulletCount == 0) {
            return;
        }
        isShooting = true;
        bulletCount--;
        shootParticles.Play();
        shootSfx.Play();
        InstantiateBullet(at);
    }
}
