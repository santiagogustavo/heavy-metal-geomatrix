using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerManager : MonoBehaviour {
    public static AudioMixerManager instance;
    public AudioMixer mixer;

    public float absoluteMusicVolume;
    public float absoluteSfxVolume;

    int audioLevelScale = -2;
    int minAudioLevel = -80;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    public void SetSfxVolume(float volume) {
        mixer.SetFloat("sfxVolume", volume);
    }

    public void SetMusicVolume(float volume) {
        mixer.SetFloat("musicVolume", volume);
    }

    void Update() {
        int musicVolume = SaveManager.instance.GetMusicVolume();
        int sfxVolume = SaveManager.instance.GetSfxVolume();
        int relativeMusicVolume = 10 - SaveManager.instance.GetMusicVolume();
        int relativeSfxVolume = 10 - SaveManager.instance.GetSfxVolume();

        if (musicVolume == 0) {
            absoluteMusicVolume = minAudioLevel;
        } else {
            absoluteMusicVolume = audioLevelScale * relativeMusicVolume;
        }

        if (sfxVolume == 0) {
            absoluteSfxVolume = minAudioLevel;
        } else {
            absoluteSfxVolume = audioLevelScale * relativeSfxVolume;
        }

        SetMusicVolume(absoluteMusicVolume);
        SetSfxVolume(absoluteSfxVolume);
    }
}
