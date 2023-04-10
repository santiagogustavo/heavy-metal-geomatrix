using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMatchController : MonoBehaviour {
    [SerializeField]
    GameObject RoundInvertBg;

    [SerializeField]
    GameObject RoundText;

    [SerializeField]
    GameObject FightText;

    [SerializeField]
    AudioSource roundBase;

    [SerializeField]
    AudioSource round1;

    [SerializeField]
    AudioSource fight;

    [SerializeField]
    AudioSource impact;

    void Start() {
        StartCoroutine(AnnounceRound());
        FightText.SetActive(false);
        RoundText.SetActive(false);
        RoundInvertBg.SetActive(false);
    }

    IEnumerator AnnounceRound() {
        yield return new WaitForSeconds(0.55f);

        RoundText.SetActive(true);
        RoundInvertBg.SetActive(true);

        roundBase.Play();
        round1.PlayDelayed(0.75f);

        yield return new WaitForSeconds(1.5f);

        FightStart();
    }

    void FightStart() {
        GameManager.instance.SetMatchWasStarted(true);

        fight.Play();
        impact.Play();

        RoundText.SetActive(false);
        FightText.SetActive(true);

        Invoke("CloseMatchUI", 1f);
    }

    void CloseMatchUI() {
        Animator bgAnimator = RoundInvertBg.GetComponent<Animator>();
        bgAnimator.Play("Close");

        Invoke("HideMatchUI", 0.15f);
    }

    void HideMatchUI() {
        gameObject.SetActive(false);
    }
}
