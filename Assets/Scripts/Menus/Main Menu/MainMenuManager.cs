using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public static MainMenuManager instance;
    GameObject pressStartAnchor;
    GameObject menuList;
    Animator fadeAnimator;

    AudioSource cursorSfx;
    AudioSource select1Sfx;

    GameObject[] selectCards;

    int selectedCard = 0;
    bool selected = false;
    bool isMenuOpen = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        cursorSfx = GameObject.Find("SFX Cursor").GetComponent<AudioSource>();
        select1Sfx = GameObject.Find("SFX Select 1").GetComponent<AudioSource>();
        selectCards = GameObject.FindGameObjectsWithTag("Select Card");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(selectCards, myComparer);

        fadeAnimator = GameObject.Find("Fade").GetComponent<Animator>();
        pressStartAnchor = GameObject.Find("Press Start Anchor");
        menuList = GameObject.Find("Menu List");
        menuList.SetActive(false);
    }

    void RedirectToSelectPlayer() {
        SceneManager.LoadScene("Select Player");
    }

    public void StartArcadeGame() {
        select1Sfx.Play();
        fadeAnimator.Play("Fade Out");
        Invoke("RedirectToSelectPlayer", 1f);
    }

    public void ExitGame() {
        Application.Quit();
    }

    void PressedStartButton() {
        isMenuOpen = true;
        pressStartAnchor.SetActive(false);
        menuList.SetActive(true);
    }

    void SelectDown() {
        cursorSfx.Play();
        if (selectedCard == selectCards.Length - 1) {
            selectedCard = 0;
        } else {
            selectedCard++;
        }
    }

    void SelectUp() {
        cursorSfx.Play();
        if (selectedCard == 0) {
            selectedCard = selectCards.Length - 1;
        } else {
            selectedCard--;
        }
    }

    void SelectCurrent() {
        selected = true;
        switch (selectedCard) {
            case 0:
                StartArcadeGame();
                break;
            case 2:
                ExitGame();
                break;
            default:
                break;
        }
    }

    void Update() {
        if (InputManager.instance.pause && !isMenuOpen) {
            PressedStartButton();
        }

        if (!isMenuOpen || selected) {
            return;
        }

        if (InputManager.instance.fire1) {
            SelectCurrent();
        }
        if (InputManager.instance.GetDownOneShot()) {
            SelectDown();
        } else if (InputManager.instance.GetUpOneShot()) {
            SelectUp();
        }
    }

    public bool IsCardSelected(string name) {
        return selectCards[selectedCard].name == name;
    }

    public bool IsSelected() {
        return selected;
    }

    public string GetSelectedDescription() {
        return selectCards[selectedCard]?.GetComponent<MainMenuCardController>()?.itemDescription;
    }
}
