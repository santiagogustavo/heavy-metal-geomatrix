using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour {
    public static SelectStageManager instance;

    GameObject[] selectCards;
    AudioSource cursorSfx;
    AudioSource selectSfx;

    int selectedCard = 0;
    bool selected = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        cursorSfx = GameObject.Find("SFX Cursor").GetComponent<AudioSource>();
        selectSfx = GameObject.Find("SFX Select").GetComponent<AudioSource>();
        selectCards = GameObject.FindGameObjectsWithTag("Select Card");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(selectCards, myComparer);
    }

    void RedirectToVersus() {
        Destroy(GameObject.Find("BGM Loop"));
        SceneManager.LoadScene("Versus Screen");
        Destroy(instance);
    }

    public void SelectStage() {
        selectSfx.Play();
        if (IsCurrentRandom()) {
            selectedCard = UnityEngine.Random.Range(0, selectCards.Length - 1);
            SelectStage();
        } else {
            selected = true;
            LevelMeta level = selectCards[selectedCard].GetComponentInChildren<LevelMeta>();
            GameManager.instance.SetLevelMeta(level);
            Invoke("RedirectToVersus", 1f);
        }
    }

    void GoToRightCard() {
        cursorSfx.Play();
        if (selectedCard == selectCards.Length - 1) {
            selectedCard = 0;
        } else {
            selectedCard++;
        }
    }
    void GoToLeftCard() {
        cursorSfx.Play();
        if (selectedCard == 0) {
            selectedCard = selectCards.Length - 1;
        } else {
            selectedCard--;
        }
    }

    void Update() {
        if (selected) {
            return;
        }
        if (InputManager.instance.GetRightOneShot()) {
            GoToRightCard();
        } else if (InputManager.instance.GetLeftOneShot()) {
            GoToLeftCard();
        } else if (InputManager.instance.fire1) {
            SelectStage();
        }
    }

    public Sprite GetCurrentBigCardSprite() {
        LevelMeta selectedMeta = selectCards[selectedCard].GetComponentInChildren<LevelMeta>();
        return selectedMeta.selectCardBig;
    }

    public string GetCurrentLevelName() {
        LevelMeta selectedMeta = selectCards[selectedCard].GetComponentInChildren<LevelMeta>();
        return selectedMeta.levelName;
    }

    public bool IsCardSelected(string name) {
        return GetCurrentLevelName() == name;
    }

    public bool IsCurrentRandom() {
        return GetCurrentLevelName() == "Random";
    }

    public bool IsSelected() {
        return selected;
    }
}
