using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectLeftController : MonoBehaviour {
    TextMeshProUGUI text;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        text.text = SelectPlayerManager.instance.GetCurrentPlayerName();
    }
}
