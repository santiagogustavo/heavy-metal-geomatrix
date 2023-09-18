using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
    [SerializeField] GameObject character;
    [SerializeField] GameObject playerCamera;
    [SerializeField] ParticleSystem dashParticles;

    PlayerCameraControllerV2 cameraController;
    PlayerAnimatorEventListener characterAnimatorEventListener;
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
    Vector2 mouse;
    Vector2 turn;
    Vector3 velocity;

    EquipType equip = 0;
    bool isAiming;
    bool isShooting;
    bool isBursting;
    bool isAttacking;
    bool isWalking;
    bool isJumping;
    bool isDoubleJumping;
    bool isGrounded = true;
    bool isDashing;
    bool isPickingUp;
    bool isDropping;
    bool hasJetpack;
    bool jetpackHasFuel;

    bool lastPauseState;
    bool lastGroundState;

    float lookUpDownLimit = 60f;

    void Start() {
        GameManager.instance.SetCurrentPlayerInstance(gameObject);

        turn.x = transform.rotation.eulerAngles.y;
        characterAnimator = character.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterAnimatorEventListener = character.GetComponent<PlayerAnimatorEventListener>();
        cameraController = playerCamera.GetComponent<PlayerCameraControllerV2>();
    }

    void ComputeInputs() {
        if (GameManager.instance.IsGamePaused() || !GameManager.instance.MatchWasStarted()) {
            return;
        }
        position.x = InputManager.instance.move.x;
        position.y = InputManager.instance.move.y;
        mouse.x = InputManager.instance.look.x;
        mouse.y = InputManager.instance.look.y;
        turn.x += mouse.x;
        turn.y += mouse.y;
        turn.y = Mathf.Clamp(turn.y, -lookUpDownLimit, lookUpDownLimit);
        isGrounded = characterController.isGrounded;
        isAiming = InputManager.instance.aim;
        isJumping = InputManager.instance.jump;
        isDoubleJumping = isJumping && !isGrounded && hasJetpack;

        cameraController.ApplyAim(isAiming);
        cameraController.ApplyDash(isDashing);
        characterAnimatorEventListener.SetIsWalking(isWalking);
    }

    void MovePosition() {
        Vector3 direction = new Vector3(position.x, 0f, position.y).normalized;

        isWalking = direction.magnitude >= 0.1f;
        if (!isWalking) {
            return;
        }

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + turn.x;
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        float currentSpeed = speed;
        if (isDashing && (!hasJetpack || !jetpackHasFuel)) {
            currentSpeed = dashingSpeed;
        } else if (isDashing && hasJetpack && jetpackHasFuel) {
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
        characterAnimator.SetFloat("Turn", mouse.x / 2f, 0.3f, Time.deltaTime);
        characterAnimator.SetFloat("Aim", turn.y / 90f, blendTreesDamp, Time.deltaTime);
        characterAnimator.SetInteger("Equip", (int) equip);
        characterAnimator.SetBool("IsAiming", isAiming);
        characterAnimator.SetBool("IsWalking", isWalking);
        characterAnimator.SetBool("IsShooting", isShooting);
        characterAnimator.SetBool("IsBursting", isBursting);
        characterAnimator.SetBool("IsAttacking", isAttacking);
        characterAnimator.SetBool("IsJumping", isJumping);
        characterAnimator.SetBool("IsDashing", isDashing);
        characterAnimator.SetBool("IsGrounded", isGrounded);
        characterAnimator.SetBool("IsPickingUp", isPickingUp);
        characterAnimator.SetBool("IsDropping", isDropping);
    }

    void UpdateGravity() {
        if (isGrounded) {
            velocity.y = -0.5f;
        } else {
            velocity.y += Physics.gravity.y * weight * Time.deltaTime;
        }
    }

    void UpdateJump() {
        if (isGrounded && isJumping) {
            velocity.y = jumpHeight;
        } else if (isDoubleJumping && jetpackHasFuel) {
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

    void RememberLastGroundStateBeforePause() {
        if (!GameManager.instance.IsGamePaused() && lastPauseState) {
            isGrounded = lastGroundState;
        }
    }

    void MemorizeGroundAndPauseStates() {
        lastPauseState = GameManager.instance.IsGamePaused();
        lastGroundState = isGrounded;
    }

    void Update() {
        ComputeInputs();
        MovePosition();
        MoveRotation();
        UpdateGravity();
        UpdateJump();
        UpdateDash();

        RememberLastGroundStateBeforePause();
        SetAnimatorVariables();
        MemorizeGroundAndPauseStates();
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

    public void SetIsBursting(bool bursting) {
        isBursting = bursting;
    }

    public void SetIsAttacking(bool attacking) {
        isAttacking = attacking;
    }

    public void SetHasJetpack(bool jetpack) {
        hasJetpack = jetpack;
    }

    public void SetJetpackHasFuel(bool hasJetpackFuel) {
        jetpackHasFuel = hasJetpackFuel;
    }

    public void SetIsDropping(bool dropping) {
        isDropping = dropping;
    }

    public bool GetIsDashing() {
        return isDashing;
    }

    public bool GetIsDoubleJumping() {
        return isDoubleJumping;
    }

    public Animator GetCharacterAnimator() {
        return character.GetComponent<Animator>();
    }
}
