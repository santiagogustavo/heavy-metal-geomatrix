using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSmallCardController : MonoBehaviour {
    Material material;
    PlayerMeta meta;

    void Awake() {
        meta = GetComponentInChildren<PlayerMeta>();
        material = GetComponentsInChildren<Renderer>()[1].material;
        material.mainTexture = meta.selectCardSmall;
    }
}
