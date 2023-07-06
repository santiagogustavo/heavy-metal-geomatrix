using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour {
    [SerializeField]
    AudioMixer mixer;

    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            mixer.SetFloat("musicVolume", AudioMixerManager.instance.absoluteMusicVolume -10f);
        } else {
            mixer.SetFloat("musicVolume", AudioMixerManager.instance.absoluteMusicVolume);
        }
    }
}
