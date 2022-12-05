using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectStageTimerController : MonoBehaviour {
    TextMeshProUGUI text;
    int timer = 30;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        Invoke("TickClock", 1f);
    }
    void TickClock() {
        if (timer == 0) {
            SelectStageManager.instance.SelectStage();
        } else if (!SelectStageManager.instance.IsSelected()) {
            timer--;
            Invoke("TickClock", 1f);
        }
    }

    void Update() {
        text.text = timer.ToString();
    }
}
