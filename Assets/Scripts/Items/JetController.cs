using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetController : MonoBehaviour {
    public float fuel = 100f;
    [SerializeField] GameObject emptyIndicatorObject;

    ParticleSystem[] particles;
    AudioSource beamSfx;
    AudioSource streamSfx;
    AudioSource smokeSfx;
    AudioSource loopSfx;

    bool emptyStreamPlayed;

    void Start() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
        particles = GetComponentsInChildren<ParticleSystem>();
        beamSfx = sfxGroup.transform.Find("Beam").gameObject.GetComponent<AudioSource>();
        smokeSfx = sfxGroup.transform.Find("Smoke").gameObject.GetComponent<AudioSource>();
        streamSfx = sfxGroup.transform.Find("Stream").gameObject.GetComponent<AudioSource>();
        loopSfx = sfxGroup.transform.Find("Loop").gameObject.GetComponent<AudioSource>();

        PlayJetBeam(false);
    }

    public void PlayJetSmoke() {
        if (!smokeSfx.isPlaying) {
            smokeSfx.Play();
        }
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Smoke" && !particle.isPlaying) {
                particle.Play();
            }
        }
    }

    public void PlayJetBeam(bool dropFuel = true) {
        if (fuel <= 0) {
            PlayJetSmoke();
            return;
        }
        if (dropFuel) {
            fuel -= 20f;
        }
        beamSfx.Play();
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Beam") {
                particle.Play();
            }
        }
    }

    void ControlJetStream(bool play) {
        if (play && !loopSfx.isPlaying) {
            streamSfx.Play();
            loopSfx.Play();
        } else if (!play) {
            loopSfx.Stop();
        }
        foreach (ParticleSystem particle in particles) {
            if (particle.name == "Jet Stream") {
                if (play && !particle.isPlaying) {
                    particle.Play();
                } else if (!play) {
                    particle.Stop();
                }
            }
        }
    }

    public void PlayJetStream(bool play) {
        if (fuel <= 0) {
            if (play && !emptyStreamPlayed) {
                emptyStreamPlayed = true;
                PlayJetSmoke();
                ControlJetStream(false);
            } else if (!play) {
                emptyStreamPlayed = false;
            }
            return;
        }
        ControlJetStream(play);
    }

    void Update() {
        emptyIndicatorObject?.SetActive(fuel <= 0f);
        if (loopSfx.isPlaying) {
            fuel -= 15f * Time.deltaTime;
        }
    }
}
