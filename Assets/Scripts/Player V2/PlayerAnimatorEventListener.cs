using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEventListener : MonoBehaviour {
    [SerializeField] LayerMask aimColliderMask = new LayerMask();
    Vector3 mouseWorldPosition;

    [SerializeField] AudioSource[] pickupSfx;
    int lastPickup;

    [SerializeField] AudioSource[] walkSfx;
    int lastWalk;

    [SerializeField] AudioSource jumpSfx;

    [SerializeField] AudioSource landSfx;

    [SerializeField] AudioSource dashSfx;

    [SerializeField] AudioSource dashLoopSfx;

    [SerializeField] AudioSource[] attackSfx;
    int lastAttack;

    bool isDelegationLocked;
    bool isWalking;

    void UnlockDelegation() {
        isDelegationLocked = false;
    }

    public void DestroyPickup() {
        GameObject weaponInstance = Finder.GetGameObjectsByTagName(transform, "Weapon")[0];
        Destroy(weaponInstance);
    }

    public void DelegateShootToPickup() {
        if (isDelegationLocked) {
            return;
        }
        isDelegationLocked = true;
        Invoke("UnlockDelegation", 0.05f);

        GameObject weaponInstance = Finder.GetGameObjectsByTagName(transform, "Weapon")[0];
        WeaponController weaponController = weaponInstance?.GetComponent<WeaponController>();
        weaponController?.Shoot(mouseWorldPosition);
    }

    public void AllowSwordAttack() {
        GameObject weaponInstance = Finder.GetGameObjectsByTagName(transform, "Weapon")[0];
        SwordController sword = weaponInstance?.GetComponent<SwordController>();
        if (sword) {
            sword.canInflictDamage = true;
        }
    }

    public void PlayRandomPickupSfx() {
        if (pickupSfx[lastPickup].isPlaying) {
            return;
        }
        lastPickup = Random.Range(0, pickupSfx.Length);
        pickupSfx[lastPickup].Play();
    }

    public void PlayRandomWalkSfx() {
        if (!isWalking || walkSfx[lastWalk].isPlaying) {
            return;
        }
        lastWalk = Random.Range(0, walkSfx.Length);
        walkSfx[lastWalk].Play();
    }

    public void PlayRandomAttackSfx() {
        if (attackSfx[lastAttack].isPlaying) {
            return;
        }
        lastAttack = Random.Range(0, attackSfx.Length);
        attackSfx[lastAttack].Play();
    }

    public void PlayAttackSfx(int index) {
        if (attackSfx[index].isPlaying) {
            return;
        }
        attackSfx[index].Play();
    }

    public void PlayJumpSfx() {
        if (jumpSfx.isPlaying) {
            return;
        }
        jumpSfx.Play();
    }

    public void PlayLandSfx() {
        if (landSfx.isPlaying) {
            return;
        }
        landSfx.Play();
    }

    public void PlayDashSfx() {
        if (dashSfx.isPlaying) {
            return;
        }
        dashSfx.Play();
    }

    public void PlayDashLoopSfx(int play) {
        if (play == 1 && dashLoopSfx.isPlaying) {
            return;
        }
        if (play == 1) {
            dashLoopSfx.Play();
        } else if (play == 0) {
            dashLoopSfx.Stop();
        }
    }

    void AimHitRaycast() {
        if (!Camera.main) {
            return;
        }
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask)) {
            mouseWorldPosition = raycastHit.point;
        }
    }

    public void SetIsWalking(bool walking) {
        isWalking = walking;
    }

    void Update() {
        AimHitRaycast();
    }
}
