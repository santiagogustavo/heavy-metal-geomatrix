using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OptionsMenuMusicManager : MonoBehaviour, IPointerEnterHandler {
    public TextMeshProUGUI text;

    void Update() {
        text.text = SaveManager.instance.GetMusicVolume().ToString();
    }

    public void OnPointerEnter(PointerEventData pd) {
        OptionsMenuManager.instance.SelectCard(gameObject.name);
    }
}
