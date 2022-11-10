using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    Button[] buttons;

    void Awake() {
        buttons = GetComponentsInChildren<Button>();
        gameObject.SetActive(false);
        GameManager.OnGameStateChanged += ChangedGameState;
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangedGameState;
    }

    void PauseGame() {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void ExitGame() {
        Application.Quit();
    }

    void ChangedGameState(GameState newState) {
        if (newState == GameState.Paused) {
            PauseGame();
        } else {
            ResumeGame();
        }
    }

    void OnEnable () {
        buttons[0]?.Select();
    }
}
