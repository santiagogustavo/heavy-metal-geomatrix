using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
    [SerializeField]
    GameObject character;

    [SerializeField]
    GameObject playerCamera;

    [SerializeField]
    ParticleSystem dashParticles;

    PlayerCameraControllerV2 cameraController;
    CharacterController characterController;
    Animator characterAnimator;

    float speed = 7.5f;
    float dashingSpeed = 10f;
    float jetpackDashingSpeed = 12.5f;
    float dashDuration = 1f;
    float blendTreesDamp = 0.1f;
    float jumpHeight = 10f;
    float weight = 2.5f;

    Vector2 position;
    Vector2 turn;
    Vector3 velocity;

    EquipType equip = 0;
    bool isAiming;
    bool isShooting;
    bool isWalking;
    bool isJumping;
    bool isDoubleJumping;
    bool isGrounded;
    bool isDashing;
    bool isPickingUp;
    bool hasJetpack;

    float lookUpDownLimit = 60f;

    void Start() {
        turn.x = transform.rotation.eulerAngles.y;
        characterAnimator = character.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraController = playerCamera.GetComponent<PlayerCameraControllerV2>();
    }

    void ComputeInputs() {
        isGrounded = characterController.isGrounded;

        if (GameManager.instance.IsGamePaused() || !GameManager.instance.MatchWasStarted()) {
            return;
        }
        position.x = InputManager.instance.horizontal;
        position.y = InputManager.instance.vertical;
        turn.x += InputManager.instance.mouseX;
        turn.y += InputManager.instance.mouseY;
        turn.y = Mathf.Clamp(turn.y, -lookUpDownLimit, lookUpDownLimit);
        isAiming = InputManager.instance.leftTrigger > 0f;
        isJumping = InputManager.instance.jump;
        isDoubleJumping = isJumping && !isGrounded && hasJetpack;

        cameraController.ApplyAim(isAiming);
        cameraController.ApplyDash(isDashing);
    }

    void MovePosition() {
        Vector3 direction = new Vector3(InputManager.instance.horizontal, 0f, InputManager.instance.vertical).normalized;

        isWalking = direction.magnitude >= 0.1f;
        if (!isWalking) {
            return;
        }

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + turn.x;
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        float currentSpeed = speed;
        if (isDashing && !hasJetpack) {
            currentSpeed = dashingSpeed;
        } else if (isDashing && hasJetpack) {
            currentSpeed = jetpackDashingSpeed;
        }
        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);
    }

    void MoveRotation() {
        transform.rotation = Quaternion.Euler(0, turn.x, 0);
    }

    void SetAnimatorVariables() {
        characterAnimator.SetFloat("X", position.x, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Y", position.y, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Turn", InputManager.instance.mouseX / 2f, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetFloat("Aim", turn.y / 90f, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetInteger("Equip", (int) equip);
        characterAnimator.SetBool("IsAiming", isAiming);
        characterAnimator.SetBool("IsWalking", isWalking);
        characterAnimator.SetBool("IsShooting", isShooting);
        characterAnimator.SetBool("IsJumping", isJumping);
        characterAnimator.SetBool("IsDashing", isDashing);
        characterAnimator.SetBool("IsGrounded", isGrounded);
        characterAnimator.SetBool("IsPickingUp", isPickingUp);
    }

    void UpdateGravity() {
        velocity.y += Physics.gravity.y * weight * Time.deltaTime;
    }

    void UpdateJump() {
        if (isGrounded) {
            velocity.y = -0.5f;

            if (isJumping) {
                velocity.y = jumpHeight;
            }
        } else if (isDoubleJumping) {
            velocity.y = jumpHeight;
        }
        characterController.Move(velocity * Time.deltaTime);
    }

    void DashStop() {
        isDashing = false;
    }

    void UpdateDash() {
        if (!isWalking) {
            isDashing = false;
        }
        if (InputManager.instance.dash && isWalking && isGrounded && !isDashing && !isJumping) {
            isDashing = true;
            Invoke("DashStop", dashDuration);
        }
        if (isDashing && !dashParticles.isPlaying) {
            dashParticles.Play();
        } else if (!isDashing) {
            dashParticles.Stop();
        }
    }

    void Update() {
        ComputeInputs();
        MovePosition();
        MoveRotation();
        UpdateGravity();
        UpdateJump();
        UpdateDash();
        SetAnimatorVariables();
    }

    public void SetEquip(EquipType newEquip) {
        equip = newEquip;
    }

    public void SetIsPickingUp(bool pickingUp) {
        isPickingUp = pickingUp;
    }

    public void SetIsShooting(bool shooting) {
        isShooting = shooting;
    }

    public void SetHasJetpack(bool jetpack) {
        hasJetpack = jetpack;
    }

    public bool GetIsDashing() {
        return isDashing;
    }

    public bool GetIsDoubleJumping() {
        return isDoubleJumping;
    }
}
