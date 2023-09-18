using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {
    [SerializeField] Animator fade;
    [SerializeField] GameObject capcom;
    [SerializeField] GameObject santiago;

    void Start() {
        StartCoroutine(PlayIntro());
    }

    IEnumerator PlayIntro() {
        capcom.SetActive(true);
        santiago.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        fade.Play("Fade Out");

        yield return new WaitForSeconds(0.9f);
        capcom.SetActive(false);
        santiago.SetActive(true);
        fade.Play("Fade In");

        yield return new WaitForSeconds(2.5f);
        fade.Play("Fade Out");

        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene("Main Menu");
    }
}
