using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSmallCardController : MonoBehaviour {
    Material material;
    PlayerMeta meta;

    void Start() {
        meta = GetComponentInChildren<PlayerMeta>();
        material = GetComponentsInChildren<Renderer>()[1].material;
        material.mainTexture = meta.selectCardSmall;
    }

    public void OnMouseEnter() {
        if (SelectPlayerManager.instance.IsSelected()) {
            return;
        }
        SelectPlayerManager.instance.SelectCard(gameObject.name);
    }


    public void OnMouseOver() {
        if (SelectPlayerManager.instance.IsSelected()) {
            return;
        }
        if (InputManager.instance.mouseSelect) {
            SelectPlayerManager.instance.SelectPlayer();
        }
    }
}
