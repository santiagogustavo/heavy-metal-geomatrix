using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour {
    Image bar;

    void Awake() {
        bar = GetComponent<Image>();
    }

    void Update() {
        bar.material.SetFloat("_Health", GameManager.instance.GetPlayerHealth());
    }
}
