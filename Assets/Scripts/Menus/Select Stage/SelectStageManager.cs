using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageManager : MonoBehaviour {
    public static SelectStageManager instance;

    GameObject[] selectCards;

    int selectedCard = 0;
    bool debounced = false;
    bool selected = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        selectCards = GameObject.FindGameObjectsWithTag("Select Card");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(selectCards, myComparer);
    }

    void ClearDebounce() {
        debounced = false;
    }

    void DebounceUpdate() {
        debounced = true;
        Invoke("ClearDebounce", 0.2f);
    }

    void RedirectToVersus() {
        SceneManager.LoadScene("Versus Screen");
        Destroy(instance);
    }

    public void SelectStage() {
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
        if (selectedCard == selectCards.Length - 1) {
            selectedCard = 0;
        } else {
            selectedCard++;
        }
        DebounceUpdate();
    }
    void GoToLeftCard() {
        if (selectedCard == 0) {
            selectedCard = selectCards.Length - 1;
        } else {
            selectedCard--;
        }
        DebounceUpdate();
    }

    void Update() {
        if (selected) {
            return;
        }
        if (InputManager.instance.GetRight() && !debounced) {
            GoToRightCard();
        } else if (InputManager.instance.GetLeft() && !debounced) {
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
