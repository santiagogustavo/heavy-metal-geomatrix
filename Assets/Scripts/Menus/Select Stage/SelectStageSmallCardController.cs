using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageSmallCardController : MonoBehaviour {
    Image image;
    LevelMeta meta;

    void Awake() {
        meta = GetComponentInChildren<LevelMeta>();
        image = GetComponent<Image>();
        image.sprite = meta.selectCardSmall;
    }

    void Update() {
        if (SelectStageManager.instance.IsCardSelected(meta.levelName)) {
            image.color = new Color(1f, 1f, 1f);
        } else {
            image.color = new Color(0.35f, 0.35f, 0.35f);
        }
    }
}
