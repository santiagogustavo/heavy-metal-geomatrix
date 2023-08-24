using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundController : MonoBehaviour {
    void Awake() {
        GameObject sfxGroup = gameObject.transform.Find("SFX").gameObject;
    }

    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            return;
        }
    }
}
