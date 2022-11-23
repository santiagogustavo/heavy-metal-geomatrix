using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour {
    TextMeshProUGUI text;
    int timer = 30;

    void Awake() {
        text = GetComponent<TextMeshProUGUI>();
        Invoke("TickClock", 1f);
    }
    void TickClock() {
        if (timer == 0) {
            SelectPlayerManager.instance.SelectPlayer();
        } else if (!SelectPlayerManager.instance.IsSelected()) {
            timer--;
            Invoke("TickClock", 1f);
        }
    }

    void Update() {
        text.text = timer.ToString();
    }
}
