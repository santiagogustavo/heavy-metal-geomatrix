using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    Button[] buttons;

    void Awake() {
        buttons = GetComponentsInChildren<Button>();
        gameObject.SetActive(false);
        GameManager.OnGameStateChanged += ChangedGameState;
        GameManager.instance.LockCursor();
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangedGameState;
    }

    void PauseGame() {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        GameManager.instance.UnlockCursor();
    }

    public void ResumeGame() {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.LockCursor();
    }

    public void CharacterChange() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Select Player");
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
        if (buttons[0]) {
            buttons[0].Select();
        }
    }
}
