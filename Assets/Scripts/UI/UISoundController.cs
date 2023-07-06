using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour {
    AudioSource lockOn;

    void Awake() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
        lockOn = sfxGroup.transform.Find("Lock On").gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            return;
        }
        if (InputManager.instance.fire3 && !lockOn.isPlaying) {
            lockOn.Play();
        }
    }
}
