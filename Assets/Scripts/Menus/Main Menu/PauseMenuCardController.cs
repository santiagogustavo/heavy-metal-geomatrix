using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuCardController : MonoBehaviour {
    Image image;
    void Awake() {
        image = GetComponent<Image>();
    }

    void Update() {
        if (PauseMenuManager.instance.IsOptionSelected(name)) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        } else {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        }
    }
}
