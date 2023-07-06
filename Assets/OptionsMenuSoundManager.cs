using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenuSoundManager : MonoBehaviour {
    public TextMeshProUGUI text;

    void Update() {
        text.text = SaveManager.instance.GetSfxVolume().ToString();
    }
}
