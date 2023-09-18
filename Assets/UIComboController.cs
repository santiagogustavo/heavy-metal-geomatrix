using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIComboController : MonoBehaviour {
    Animator animator;
    TextMeshProUGUI text;

    int lastComboCount;

    void Start() {
        animator = GetComponent<Animator>();
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        int comboCount = PlayerComboController.instance.comboCount;

        if (comboCount != lastComboCount) {
            animator.Play("Combo", 0, 0f);
        }
        if (comboCount == 1) {
            text.text = "1 hit";
        } else if (comboCount > 1) {
            text.text = comboCount + " hits";
        } else {
            text.text = "";
        }
        lastComboCount = comboCount;
    }
}
