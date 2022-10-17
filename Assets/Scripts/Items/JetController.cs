using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetController : MonoBehaviour {
    ParticleSystem[] particles;

    void Awake() {
        particles = GetComponentsInChildren<ParticleSystem>();
    }

    void Start() {
        PlayJetBeam();
    }

    public void PlayJetBeam() {
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Beam") {
                particle.Play();
            }
        }
    }

    public void PlayJetStream(bool play) {
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Stream") {
                if (play) {
                    particle.Play();
                } else {
                    particle.Stop();
                }
            }
        }
    }
}
