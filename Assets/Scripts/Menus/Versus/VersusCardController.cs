using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersusCardController : MonoBehaviour {
    Image card;
    Sprite defaultSprite;

    void Start() {
        card = GetComponent<Image>();
        defaultSprite = card.sprite;
        card.sprite = GameManager.instance?.GetPlayer1Meta()?.versusCard ?? defaultSprite;
    }
}
