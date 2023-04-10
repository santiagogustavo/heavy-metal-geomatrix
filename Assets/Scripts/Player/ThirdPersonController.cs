using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour {
    InventoryManager inventory;
    CharacterController controller;

    public AnimationController animator;
    public ParticleSystem dashParticle;
    public Transform cameraObject;

    public float speed = 7.5f;
    public float dashSpeed = 2f;
    public float jetSpeed = 4f;
    public float jumpHeight = 1f;
    public float rotationSpeed = 0.1f;

    float turnSmoothVelocity;
    Vector3 velocity;

    public bool currGroundedState = true;
    bool lastGroundedState = true;

    public bool hasJumped = false;
    public bool hasDoubleJumped = false;
    public bool isDashing = false;
    public bool isPickingUp = false;
    public bool isShooting = false;

    void Awake() {
        inventory = GetComponent<InventoryManager>();
        controller = GetComponent<CharacterController>();
    }

    void UpdateMovement() {
        Vector3 direction = new Vector3(InputManager.instance.horizontal, 0f, InputManager.instance.vertical).normalized;

        bool isWalking = direction.magnitude >= 0.1f;   

        if (isWalking) {
            animator.SetIsWalking(true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraObject.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = speed;
            if (isDashing) {
                if (inventory.HasJetpack()) {
                    currentSpeed = speed + jetSpeed;
                } else {
                    currentSpeed = speed + dashSpeed;
                }
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        } else {
            animator.SetIsWalking(false);
        }

        if (currGroundedState) {
            if (isWalking) {
                if (!animator.IsJumpingOrFalling() && !animator.IsPickingUp() && !animator.IsShooting() && !isDashing) {
                    animator.ChangeAnimationState("Walk");
                }
            } else if (lastGroundedState && !animator.IsJumpingOrFalling() && !animator.IsPickingUp() && !animator.IsShooting()) {
                animator.ChangeAnimationState("Idle", 0.15f);
            }
        }
    }

    void UpdateDash() {
        if (InputManager.instance.leftTrigger == 1f) {
            if (currGroundedState && !animator.IsCurrentAnimation("Idle") && !isDashing) {
                animator.ChangeAnimationState("Dash");
            }
        }
    }

    void UpdateGravity() {
        velocity.y += Physics.gravity.y * Time.deltaTime;

        if (!lastGroundedState) {
            if (currGroundedState && !isDashing) {
                if (animator.IsJumpingOrFalling()) {
                    animator.ChangeAnimationState("Land");
                }
                hasJumped = false;
                hasDoubleJumped = false;
            } else if (!animator.IsCurrentAnimation("Jump") && !isDashing) {
                animator.ChangeAnimationState("Fall");
            }
        }
    }

    void UpdateJump() {
        if (currGroundedState) {
            velocity.y = -0.5f;

            if (InputManager.instance.jump) {
                animator.ChangeAnimationState("Jump");
                velocity.y = jumpHeight;
                hasJumped = true;
            }
        } else {
            if (InputManager.instance.jump && inventory.HasJetpack() && hasJumped && !hasDoubleJumped) {
                velocity.y += jumpHeight;
                hasDoubleJumped = true;
            }
        }
    }

    void UpdatePublicVariables() {
        isDashing = animator.IsDashing();
        isPickingUp = animator.IsPickingUp();
    }

    void UpdateDashParticle() {
        if (isDashing) {
            if (!dashParticle.isPlaying) {
                dashParticle.Play();
            }
        } else {
            dashParticle.Stop();
        }
    }

    void Update() {
        if (GameManager.instance.IsGamePaused()) {
            return;
        }

        currGroundedState = controller.isGrounded;

        UpdatePublicVariables();
        UpdateDashParticle();
        UpdateGravity();

        if (!isPickingUp && !isShooting && GameManager.instance.MatchWasStarted()) {
            UpdateMovement();
            UpdateDash();
            UpdateJump();
        }

        controller.Move(velocity * Time.deltaTime);
        lastGroundedState = currGroundedState;
    }
}
