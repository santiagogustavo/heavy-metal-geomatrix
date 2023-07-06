using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetController : MonoBehaviour {
    ParticleSystem[] particles;
    AudioSource beamSfx;
    AudioSource streamSfx;
    AudioSource loopSfx;

    void Awake() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
        particles = GetComponentsInChildren<ParticleSystem>();
        beamSfx = sfxGroup.transform.Find("Beam").gameObject.GetComponent<AudioSource>();
        streamSfx = sfxGroup.transform.Find("Stream").gameObject.GetComponent<AudioSource>();
        loopSfx = sfxGroup.transform.Find("Loop").gameObject.GetComponent<AudioSource>();
    }

    void Start() {
        PlayJetBeam();
    }

    public void PlayJetBeam() {
        beamSfx.Play();
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Beam") {
                particle.Play();
            }
        }
    }

    public void PlayJetStream(bool play) {
        streamSfx.Play();

        if (play) {
            loopSfx.Play();
        } else if (!play) {
            loopSfx.Stop();
        }

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
