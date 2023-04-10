using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour {
    AudioSource jump;
    AudioSource dash;
    AudioSource land;
    AudioSource[] walk;
    AudioSource[] pickup;

    int lastWalk = 0;
    int lastPickup = 0;

    void Awake() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
        jump = sfxGroup.transform.Find("Jump").gameObject.GetComponent<AudioSource>();
        dash = sfxGroup.transform.Find("Dash").gameObject.GetComponent<AudioSource>();
        land = sfxGroup.transform.Find("Land").gameObject.GetComponent<AudioSource>();
        walk = sfxGroup.transform.Find("Walk").gameObject.GetComponentsInChildren<AudioSource>();
        pickup = sfxGroup.transform.Find("Pickup").gameObject.GetComponentsInChildren<AudioSource>();
    }

    public void PlayJumpSound() {
        jump.Play();
    }

    public void PlayDashSound() {
        dash.Play();
    }

    public void PlayLandSound() {
        land.Play();
    }

    public void PlayWalkSound() {
        if (walk[lastWalk].isPlaying) {
            return;
        }
        lastWalk = UnityEngine.Random.Range(0, walk.Length);
        walk[lastWalk].Play();
    }

    public void PlayPickupSound() {
        if (pickup[lastPickup].isPlaying) {
            return;
        }
        lastPickup = UnityEngine.Random.Range(0, pickup.Length);
        pickup[lastPickup].Play();
    }
}
