using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenuMusicManager : MonoBehaviour {
    public TextMeshProUGUI text;

    void Update() {
        text.text = SaveManager.instance.GetMusicVolume().ToString();
    }
}
