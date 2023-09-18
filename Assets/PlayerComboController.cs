using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboController : MonoBehaviour {
    public static PlayerComboController instance;
    [SerializeField] float clearTime = 1f;

    public int comboCount = 0;
    bool wasComboRegistered;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void ClearComboCount() {
        if (!wasComboRegistered) {
            comboCount = 0;
        } 
    }

    void ClearComboRegister() {
        wasComboRegistered = false;
    }

    public void ComboHit() {
        comboCount++;
        wasComboRegistered = true;
        Invoke("ClearComboRegister", (3 * clearTime) / 4f);
        Invoke("ClearComboCount", clearTime);
    }
}
