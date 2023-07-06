using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour {
    AudioSource jump;
    AudioSource dash;
    AudioSource dashLoop;
    AudioSource land;
    AudioSource[] walk;
    AudioSource[] pickup;
    AudioSource[] attack;

    int lastWalk = 0;
    int lastPickup = 0;
    int lastAttack = 0;

    AnimationController animationController;

    void Awake() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
        jump = sfxGroup.transform.Find("Jump").gameObject.GetComponent<AudioSource>();
        dash = sfxGroup.transform.Find("Dash").gameObject.GetComponent<AudioSource>();
        dashLoop = sfxGroup.transform.Find("Dash Loop").gameObject.GetComponent<AudioSource>();
        land = sfxGroup.transform.Find("Land").gameObject.GetComponent<AudioSource>();
        walk = sfxGroup.transform.Find("Walk").gameObject.GetComponentsInChildren<AudioSource>();
        pickup = sfxGroup.transform.Find("Pickup").gameObject.GetComponentsInChildren<AudioSource>();
        attack = sfxGroup.transform.Find("Attack").gameObject.GetComponentsInChildren<AudioSource>();
    }

    void Start() {
        animationController = GetComponent<AnimationController>();
    }

    public void PlayJumpSound() {
        jump.Play();
    }

    public void PlayDashSound() {
        dash.Play();
    }

    public void PlayDashLoopSound(bool play) {
        if (play && !dashLoop.isPlaying) {
            dashLoop.Play();
        } else if (!play) {
            dashLoop.Stop();
        }
    }

    public void PlayLandSound() {
        land.Play();
    }

    public void PlayWalkSound() {
        if (walk[lastWalk].isPlaying) {
            return;
        }
        lastWalk = Random.Range(0, walk.Length);
        walk[lastWalk].Play();
    }

    public void PlayPickupSound() {
        if (pickup[lastPickup].isPlaying) {
            return;
        }
        lastPickup = Random.Range(0, pickup.Length);
        pickup[lastPickup].Play();
    }

    public void PlayAttackSound() {
        if (attack[lastAttack].isPlaying) {
            return;
        }
        lastAttack = Random.Range(0, attack.Length);
        attack[lastAttack].Play();
    }

    public void Update() {
        PlayDashLoopSound(animationController.IsDashing());
    }
}
