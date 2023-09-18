using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OptionsMenuSoundManager : MonoBehaviour, IPointerEnterHandler {
    public TextMeshProUGUI text;

    void Update() {
        text.text = SaveManager.instance.GetSfxVolume().ToString();
    }

    public void OnPointerEnter(PointerEventData pd) {
        OptionsMenuManager.instance.SelectCard(gameObject.name);
    }
}
