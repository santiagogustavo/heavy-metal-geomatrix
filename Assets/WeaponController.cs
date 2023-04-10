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

    void Awake() {
        particles = GetComponentInChildren<ParticleSystem>();
        sfx = GetComponentInChildren<AudioSource>();
    }

    public void Shoot() {
        particles.Play();
        sfx.Play();
    }
}
