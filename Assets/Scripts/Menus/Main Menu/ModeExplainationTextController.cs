using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeExplainationTextController : MonoBehaviour {
    TextMeshProUGUI text;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        text.text = MainMenuManager.instance.GetSelectedDescription();
    }
}
