using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBaseController : MonoBehaviour {
    [SerializeField]
    GameObject UICanvas;

    void Update() {
        UICanvas.SetActive(GameManager.instance.MatchWasStarted());
    }
}
