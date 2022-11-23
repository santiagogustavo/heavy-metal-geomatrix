using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigCardController : MonoBehaviour {
    Material material;

    void Awake() {
        material = GetComponent<Renderer>().material;
    }
    void Update() {
        material.mainTexture = SelectPlayerManager.instance.GetCurrentBigCardTexture();
    }
}
