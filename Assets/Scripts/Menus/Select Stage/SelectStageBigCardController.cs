using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageBigCardController : MonoBehaviour {
    Image image;

    void Awake() {
        image = GetComponent<Image>();
    }
    void Update() {
        image.sprite = SelectStageManager.instance.GetCurrentBigCardSprite();
    }
}
