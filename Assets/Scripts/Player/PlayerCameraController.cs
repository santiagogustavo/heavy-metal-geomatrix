using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {
    [SerializeField]
    GameObject mainCamera;

    [SerializeField]
    GameObject matchCamera;

    [SerializeField]
    Animator matchCameraAnimator;

    void Start() {
        if (GameManager.instance.MatchWasStarted()) {
            return;
        }
        StartCoroutine(PlayMatchAnimations());
    }

    IEnumerator PlayMatchAnimations() {
        matchCameraAnimator.Play("Match 1");

        yield return new WaitForSeconds(1.55f);

        matchCameraAnimator.Play("Match Start");
    }

    void Update() {
        if (GameManager.instance.MatchWasStarted()) {
            mainCamera.SetActive(true);
            matchCamera.SetActive(false);
        } else {
            mainCamera.SetActive(false);
            matchCamera.SetActive(true);
        }
    }
}
