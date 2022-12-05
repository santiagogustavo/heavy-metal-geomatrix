using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersusLevelNameController : MonoBehaviour {
    TextMeshProUGUI text;
    string defaultName;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        defaultName = text.text;
        text.text = GameManager.instance?.GetLevelMeta()?.levelName ?? defaultName;
    }
}
