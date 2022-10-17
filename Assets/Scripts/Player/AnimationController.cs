using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {
    Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    public bool IsCurrentAnimation(string name) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public bool IsNextAnimation(string name) {
        return animator.GetNextAnimatorStateInfo(0).IsName(name);
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

    public void SetIsWalking(bool value) {
        animator.SetBool("Walking", value);
    }

    public void ChangeAnimationState(string newState, float crossFadeDuration = 0f) {
        if (IsCurrentAnimation(newState) || IsNextAnimation(newState)) {
            return;
        }

        if (crossFadeDuration > 0f) {
            animator.CrossFadeInFixedTime(newState, crossFadeDuration);
        } else {
            animator.Play(newState);
        }
    }
}
