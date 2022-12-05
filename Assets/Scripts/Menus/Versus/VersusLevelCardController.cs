using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersusLevelCardController : MonoBehaviour {
    Image card;
    Sprite defaultSprite;

    void Awake() {
        card = GetComponent<Image>();
        defaultSprite = card.sprite;
        card.sprite = GameManager.instance?.GetLevelMeta()?.versusCard ?? defaultSprite;
    }
}
