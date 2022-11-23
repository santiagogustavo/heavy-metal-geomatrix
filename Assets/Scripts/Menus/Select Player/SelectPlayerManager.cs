using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPlayerManager : MonoBehaviour {
    public static SelectPlayerManager instance;

    [SerializeField]
    GameObject selectMarker;

    GameObject[] selectCards;
    GameObject markerInstance;

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
        InstantiateMarker();
    }

    void ClearDebounce() {
        debounced = false;
    }

    void DebounceUpdate() {
        debounced = true;
        Invoke("ClearDebounce", 0.2f);
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
        if (selectedCard == selectCards.Length - 1) {
            selectedCard = 0;
        } else {
            selectedCard++;
        }
        DebounceUpdate();
        InstantiateMarker();
    }
    void GoToLeftCard() {
        if (selectedCard == 0) {
            selectedCard = selectCards.Length - 1;
        } else {
            selectedCard--;
        }
        DebounceUpdate();
        InstantiateMarker();
    }

    void RedirectToVersus() {
        SceneManager.LoadScene("Versus Screen");
    }

    public void SelectPlayer() {
        selected = true;
        PlayerMeta player = selectCards[selectedCard].GetComponentInChildren<PlayerMeta>();
        GameManager.instance.SetPlayer1Meta(player);
        markerInstance.GetComponent<Animator>().Play("Selected", 0);
        Invoke("RedirectToVersus", 1f);
    }

    void Update() {
        if (InputManager.instance.dPadX == 1f && !debounced) {
            GoToRightCard();
        } else if (InputManager.instance.dPadX == -1f && !debounced) {
            GoToLeftCard();
        } else if (InputManager.instance.fire1) {
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
}
