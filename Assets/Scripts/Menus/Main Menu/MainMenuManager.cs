using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public static MainMenuManager instance;
    [SerializeField] GameObject[] selectCards;

    GameObject pressStartAnchor;
    GameObject menuList;
    GameObject optionsMenu;
    Animator fadeAnimator;

    AudioSource cursorSfx;
    AudioSource select1Sfx;
    AudioSource select2Sfx;

    int selectedCard = 0;
    bool selected = false;
    bool isMenuOpen = false;
    bool isOptionsMenuOpen = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        cursorSfx = GameObject.Find("SFX Cursor").GetComponent<AudioSource>();
        select1Sfx = GameObject.Find("SFX Select 1").GetComponent<AudioSource>();
        select2Sfx = GameObject.Find("SFX Select 2").GetComponent<AudioSource>();

        fadeAnimator = GameObject.Find("Fade").GetComponent<Animator>();
        pressStartAnchor = GameObject.Find("Press Start Anchor");
        menuList = GameObject.Find("Menu List");
        menuList.SetActive(false);

        optionsMenu = GameObject.Find("Options Menu");
        optionsMenu.SetActive(false);
    }

    void RedirectToSelectPlayer() {
        SceneManager.LoadScene("Select Player");
    }

    public void StartArcadeGame() {
        selected = true;
        select1Sfx.Play();
        fadeAnimator.Play("Fade Out");
        Invoke("RedirectToSelectPlayer", 1f);
    }

    public void ExitGame() {
        selected = true;
        Application.Quit();
    }

    void OpenMenu() {
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

    public void SelectCurrent() {
        switch (selectedCard) {
            case 0:
                StartArcadeGame();
                break;
            case 1:
                OpenOptionsMenu();
                break;
            case 2:
                ExitGame();
                break;
            default:
                break;
        }
    }

    void OpenOptionsMenu() {
        select2Sfx.Play();
        isOptionsMenuOpen = true;
        optionsMenu.SetActive(true);
    }

    void CloseOptionsMenu() {
        isOptionsMenuOpen = false;
        optionsMenu.SetActive(false);
    }

    void Update() {
        if (InputManager.instance.cancel) {
            if (isOptionsMenuOpen) {
                CloseOptionsMenu();
            }
        }
        
        if (InputManager.instance.pause && !isMenuOpen) {
            OpenMenu();
        }

        if (!isMenuOpen || isOptionsMenuOpen || selected) {
            return;
        }

        if (InputManager.instance.select) {
            SelectCurrent();
        }
        if (InputManager.instance.GetDownTap()) {
            SelectDown();
        } else if (InputManager.instance.GetUpTap()) {
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

    public void SelectCard(string name) {
        cursorSfx.Play();
        int index = 0;
        foreach (GameObject card in selectCards) {
            if (card.name == name) {
                selectedCard = index;
                return;
            }
            index++;
        }
    }
}
