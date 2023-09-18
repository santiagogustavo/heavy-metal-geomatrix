using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigCardController : MonoBehaviour {
    Animator animator;
    Material material;

    void Start() {
        animator = GetComponent<Animator>();
        material = GetComponent<Renderer>().material;
    }

    void Update() {
        material.mainTexture = SelectPlayerManager.instance.GetCurrentBigCardTexture();

        if (SelectPlayerManager.instance.IsSelected()) {
            animator.Play("Select");
        }
    }
}
