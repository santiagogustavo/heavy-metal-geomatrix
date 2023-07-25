using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
    ParticleSystem particles;
    AudioSource sfx;

    [SerializeField]
    public float fireRate;

    [SerializeField]
    public float repeatRate;

    [SerializeField]
    public int burst;

    [SerializeField]
    Transform bulletHole;

    [SerializeField]
    GameObject bullet;

    void Awake() {
        particles = GetComponentInChildren<ParticleSystem>();
        sfx = GetComponentInChildren<AudioSource>();
    }

    public void InstantiateBullet() {
        GameObject bulletInstance = Instantiate(bullet, bulletHole.transform.position, bulletHole.transform.rotation);
        bulletInstance.transform.rotation = Quaternion.Euler(0f, bulletInstance.transform.eulerAngles.y, bulletInstance.transform.eulerAngles.z);
        bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * 50f, ForceMode.Impulse);
    }

    public void Shoot() {
        particles.Play();
        sfx.Play();
        Invoke("InstantiateBullet", 0.05f);
    }
}
