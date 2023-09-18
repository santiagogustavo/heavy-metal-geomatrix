using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlayerManager : MonoBehaviour {
    public static SelectPlayerManager instance;

    [SerializeField]
    GameObject selectMarker;

    GameObject[] selectCards;
    GameObject markerInstance;

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
        selectCards = GameObject.FindGameObjectsWithTag("Select Card");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(selectCards, myComparer);

        cursorSfx = GameObject.Find("SFX Cursor").GetComponent<AudioSource>();
        selectSfx = GameObject.Find("SFX Select").GetComponent<AudioSource>();
        InstantiateMarker();
    }

    void InstantiateMarker() {
        if (markerInstance) {
            DestroyMarker();
        }
        markerInstance = Instantiate(
                selectMarker,
                new Vector3(0f, 0.002f, 0f),
                Quaternion.Euler(0f, 0f, 0f)
            );
        markerInstance.transform.SetParent(selectCards[selectedCard].transform, false);
    }

    void DestroyMarker() {
        Destroy(markerInstance);
    }

    void GoToRightCard() {
        cursorSfx.Play();
        if (selectedCard == selectCards.Length - 1) {
            selectedCard = 0;
        } else {
            selectedCard++;
        }
        InstantiateMarker();
    }
    void GoToLeftCard() {
        cursorSfx.Play();
        if (selectedCard == 0) {
            selectedCard = selectCards.Length - 1;
        } else {
            selectedCard--;
        }
        InstantiateMarker();
    }

    void RedirectToStageSelection() {
        DontDestroyOnLoad(GameObject.Find("BGM Loop"));
        SceneManager.LoadScene("Select Stage");
        Destroy(instance);
    }

    public void SelectPlayer() {
        selectSfx.Play();
        selected = true;
        PlayerMeta player = selectCards[selectedCard].GetComponentInChildren<PlayerMeta>();
        GameManager.instance.SetPlayer1Meta(player);
        markerInstance.GetComponent<Animator>().Play("Selected", 0);
        Invoke("RedirectToStageSelection", 1f);
    }

    void Update() {
        if (selected) {
            return;
        }
        if (InputManager.instance.GetRightTap()) {
            GoToRightCard();
        } else if (InputManager.instance.GetLeftTap()) {
            GoToLeftCard();
        } else if (InputManager.instance.select) {
            SelectPlayer();
        }
    }

    public Texture2D GetCurrentBigCardTexture() {
        PlayerMeta selectedMeta = selectCards[selectedCard].GetComponentInChildren<PlayerMeta>();
        return selectedMeta.selectCardBig;
    }

    public string GetCurrentPlayerName() {
        PlayerMeta selectedMeta = selectCards[selectedCard].GetComponentInChildren<PlayerMeta>();
        return selectedMeta.characterName;
    }

    public bool IsSelected() {
        return selected;
    }

    public void SelectCard(string name) {
        cursorSfx.Play();
        int index = 0;
        foreach (GameObject card in selectCards) {
            if (card.name == name) {
                selectedCard = index;
                InstantiateMarker();
                return;
            }
            index++;
        }
    }
}
