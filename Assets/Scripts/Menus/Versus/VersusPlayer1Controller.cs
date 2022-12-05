using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersusPlayer1Controller : MonoBehaviour {
    TextMeshProUGUI text;
    string defaultName;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        defaultName = text.text;
        text.text = GameManager.instance?.GetPlayer1Meta()?.characterName ?? defaultName;
    }
}
