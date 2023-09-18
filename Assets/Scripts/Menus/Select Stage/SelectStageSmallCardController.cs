using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectStageSmallCardController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    Image image;
    LevelMeta meta;

    void Start() {
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

    public void OnPointerEnter(PointerEventData pd) {
        if (SelectStageManager.instance.IsSelected()) {
            return;
        }
        SelectStageManager.instance.SelectCard(gameObject.name);
    }

    public void OnPointerClick(PointerEventData pd) {
        if (SelectStageManager.instance.IsSelected()) {
            return;
        }
        SelectStageManager.instance.SelectStage();
    }
}
