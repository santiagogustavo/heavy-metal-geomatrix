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
    Animator bgmText;

    [SerializeField]
    AudioSource roundBase;

    [SerializeField]
    AudioSource round1;

    [SerializeField]
    AudioSource fight;

    [SerializeField]
    AudioSource impact;

    [SerializeField]
    float roundAnnounceTime = 0.55f;

    [SerializeField]
    float fightStartTime = 2f;

    [SerializeField]
    bool debug = false;

    void Start() {
        FightText.SetActive(false);
        RoundText.SetActive(false);
        RoundInvertBg.SetActive(false);

        if (debug) {
            FightStart(false);
            return;
        }
        StartCoroutine(AnnounceRound());
    }

    IEnumerator AnnounceRound() {
        yield return new WaitForSeconds(roundAnnounceTime);

        RoundText.SetActive(true);
        RoundInvertBg.SetActive(true);

        roundBase.Play();

        yield return new WaitForSeconds(0.75f);

        round1.Play();
        bgmText.Play("CloseBGM");

        yield return new WaitForSeconds(fightStartTime);

        FightStart();
    }

    void FightStart(bool playEffects = true) {
        GameManager.instance.SetMatchWasStarted(true);

        if (playEffects) {
            fight.Play();
            impact.Play();
            RoundText.SetActive(false);
            FightText.SetActive(true);
        }

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
