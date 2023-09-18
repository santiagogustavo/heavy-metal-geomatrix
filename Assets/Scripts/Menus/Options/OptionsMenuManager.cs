using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OptionsMenuVolumeOption {
    SoundUp,
    SoundDown,
    MusicUp,
    MusicDown,
}

public class OptionsMenuManager : MonoBehaviour {
    public static OptionsMenuManager instance;

    public RectTransform selectBG;
    public Animator selectBGAnimator;
    public RectTransform[] options;

    int selectedOption = 0;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Update() {
        UpdateSelectPosition();
        if (InputManager.instance.GetDownTap()) {
            SelectDown();
            PlaySelectAnimation();
        } else if (InputManager.instance.GetUpTap()) {
            SelectUp();
            PlaySelectAnimation();
        }
        LeftRightUpdate();
    }

    public void HandleVolumeEvent(OptionsMenuVolumeOption option) {
        switch (option) {
            case OptionsMenuVolumeOption.SoundUp:
                SfxVolumeUp();
                break;
            case OptionsMenuVolumeOption.SoundDown:
                SfxVolumeDown();
                break;
            case OptionsMenuVolumeOption.MusicUp:
                MusicVolumeUp();
                break;
            case OptionsMenuVolumeOption.MusicDown:
                MusicVolumeDown();
                break;
            default:
                break;
        }
    }

    void LeftRightUpdate() {
        if (InputManager.instance.GetLeftTap()) {
            if (selectedOption == 0) {
                SfxVolumeDown();
            } else if (selectedOption == 1) {
                MusicVolumeDown();
            }
        }
        if (InputManager.instance.GetRightTap()) {
            if (selectedOption == 0) {
                SfxVolumeUp();
            } else if (selectedOption == 1) {
                MusicVolumeUp();
            }
        }
    }

    void UpdateSelectPosition() {
        selectBG.anchoredPosition = new Vector3(selectBG.localPosition.x, options[selectedOption].localPosition.y, selectBG.localPosition.z);
    }

    void PlaySelectAnimation() {
        selectBGAnimator.Play("SelectBG", -1, 0f);
    }

    void SelectDown() {
        if (selectedOption == options.Length - 1) {
            selectedOption = 0;
        } else {
            selectedOption++;
        }
    }

    void SelectUp() {
        if (selectedOption == 0) {
            selectedOption = options.Length - 1;
        } else {
            selectedOption--;
        }
    }

    void SfxVolumeDown() {
        if (SaveManager.instance.GetSfxVolume() > 0) {
            SaveManager.instance.SetSfxVolume(SaveManager.instance.GetSfxVolume() - 1);
        }
    }

    void SfxVolumeUp() {
        if (SaveManager.instance.GetSfxVolume() < 10) {
            SaveManager.instance.SetSfxVolume(SaveManager.instance.GetSfxVolume() + 1);
        }
    }

    void MusicVolumeDown() {
        if (SaveManager.instance.GetMusicVolume() > 0) {
            SaveManager.instance.SetMusicVolume(SaveManager.instance.GetMusicVolume() - 1);
        }
    }

    void MusicVolumeUp() {
        if (SaveManager.instance.GetMusicVolume() < 10) {
            SaveManager.instance.SetMusicVolume(SaveManager.instance.GetMusicVolume() + 1);
        }
    }

    public void SelectCard(string name) {
        int index = 0;
        foreach (RectTransform option in options) {
            if (option.gameObject.name == name) {
                selectedOption = index;
                PlaySelectAnimation();
                return;
            }
            index++;
        }
    }
}
