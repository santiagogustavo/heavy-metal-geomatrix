using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    Animator animator;
    int activeLayer = 0;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public int GetActiveLayer() {
        return activeLayer;
    }

    public void ChangeAnimatorLayer(int layer) {
        activeLayer = layer;

        for (int i = 0; i < animator.layerCount; i++) {
            if (i != layer) {
                animator.SetLayerWeight(i, 0f);
            } else {
                animator.SetLayerWeight(i, 1f);
            }
        }
    }

    public bool IsCurrentAnimation(string name) {
        return animator.GetCurrentAnimatorStateInfo(activeLayer).IsName(name);
    }

    public bool IsNextAnimation(string name) {
        return animator.GetNextAnimatorStateInfo(activeLayer).IsName(name);
    }

    public bool IsWalking() {
        return IsCurrentAnimation("Walk");
    }

    public bool IsDashing() {
        return IsCurrentAnimation("Dash") || IsCurrentAnimation("Dash Loop");
    }

    public bool IsJumpingOrFalling() {
        return IsCurrentAnimation("Jump") || IsCurrentAnimation("Land") || IsCurrentAnimation("Fall");
    }

    public bool IsPickingUp() {
        return IsCurrentAnimation("Pickup");
    }

    public void SetIsWalking(bool value) {
        animator.SetBool("Walking", value);
    }

    public void ChangeAnimationState(string newState, float crossFadeDuration = 0f) {
        if (IsCurrentAnimation(newState) || IsNextAnimation(newState)) {
            return;
        }

        if (crossFadeDuration > 0f) {
            animator.CrossFadeInFixedTime(newState, crossFadeDuration, activeLayer);
        } else {
            int layer = 0;
            for (layer = 0; layer < animator.layerCount; layer++) {
                animator.Play(newState, layer);
            }
        }
    }
}
