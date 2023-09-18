using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options {
    public int musicVolume = 10;
    public int sfxVolume = 10;
}

public class SaveManager : MonoBehaviour {
    public static SaveManager instance;

    public Options options = new Options();

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        options = FileHandler.ReadFromJSON<Options>("options.json");
    }

    public int GetMusicVolume() {
        return options.musicVolume;
    }

    public int GetSfxVolume() {
        return options.sfxVolume;
    }

    public void SetMusicVolume(int volume) {
        options.musicVolume = volume;
        SaveOptions();
    }

    public void SetSfxVolume(int volume) {
        options.sfxVolume = volume;
        SaveOptions();
    }

    public void SaveOptions() {
        FileHandler.SaveToJSON<Options>(options, "options.json");
    }
}
