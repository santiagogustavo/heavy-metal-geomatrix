using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuCardController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    Image image;

    void Awake() {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData pd) {
        PauseMenuManager.instance.SelectOption(name);
    }

    public void OnPointerClick(PointerEventData pd) {
        PauseMenuManager.instance.SelectCurrent();
    }

    void Update() {
        if (PauseMenuManager.instance.IsOptionSelected(name)) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        } else {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }
}
