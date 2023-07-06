using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuManager : MonoBehaviour {
    public RectTransform selectBG;
    public Animator selectBGAnimator;
    public RectTransform[] options;

    int selectedOption = 0;

    void Update() {
        UpdateSelectPosition();
        if (InputManager.instance.GetDownOneShot()) {
            SelectDown();
            PlaySelectAnimation();
        } else if (InputManager.instance.GetUpOneShot()) {
            SelectUp();
            PlaySelectAnimation();
        }
        LeftRightUpdate();
    }

    void LeftRightUpdate() {
        if (InputManager.instance.GetLeftOneShot()) {
            if (selectedOption == 0) {
                SfxVolumeDown();
            } else if (selectedOption == 1) {
                MusicVolumeDown();
            }
        }
        if (InputManager.instance.GetRightOneShot()) {
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
}
