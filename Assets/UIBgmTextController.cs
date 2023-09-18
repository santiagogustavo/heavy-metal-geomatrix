using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBgmTextController : MonoBehaviour {
    TextMeshProUGUI text;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        string bgmName = GameManager.instance.GetBgm()?.name;

        if (bgmName != null) {
            text.text = "bgm: " + bgmName;
        } else {
            text.text = "";
        }
    }
}
