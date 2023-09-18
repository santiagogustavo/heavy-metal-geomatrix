using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    public static PauseMenuManager instance;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject[] options;

    int selectedOption = 0;
    bool isOptionsOpen = false;

    void Awake() {
        if (!instance) {
            instance = this;
        }

        pauseMenu.SetActive(false);
        GameManager.OnGameStateChanged += ChangedGameState;
    }

    void Start() {
        selectedOption = 0;
        GameManager.instance.LockCursor();
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged -= ChangedGameState;
    }

    void Update() {
        optionsMenu.SetActive(isOptionsOpen);

        if (!GameManager.instance.MatchWasStarted()) {
            return;
        }

        if (InputManager.instance.pause) {
            selectedOption = 0;
            isOptionsOpen = false;

            if (GameManager.state == GameState.Paused) {
                GameManager.instance.UpdateGameState(GameState.Versus);
            } else {
                GameManager.instance.UpdateGameState(GameState.Paused);
            }
        } else if (InputManager.instance.cancel) {
            if (isOptionsOpen) {
                isOptionsOpen = false;
            } else {
                ResumeGame();
            }
        }

        if (!GameManager.instance.IsGamePaused() || isOptionsOpen) {
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

    void SelectDown() {
        if (selectedOption == options.Length - 1) {
            selectedOption = 0;
        } else {
            selectedOption++;
        }
    }

    void SelectUp() {
        if (selectedOption == 0) {
            selectedOption = options.Length - 1;
        } else {
            selectedOption--;
        }
    }

    public void SelectCurrent() {
        switch (selectedOption) {
            case 0:
                ResumeGame();
                break;
            case 1:
                CharacterChange();
                break;
            case 2:
                OpenOptionsMenu();
                break;
            case 3:
                ExitGame();
                break;
            default:
                break;
        }
    }

    void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameManager.instance.UnlockCursor();
        GameManager.instance.SetIsGamePaused(true);
    }

    void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.LockCursor();
        GameManager.instance.SetIsGamePaused(false);
    }

    void CharacterChange() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Select Player");
    }

    void OpenOptionsMenu() {
        isOptionsOpen = true;
    }

    void ExitGame() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    void ChangedGameState(GameState newState) {
        if (newState == GameState.Paused) {
            PauseGame();
        } else {
            ResumeGame();
        }
    }

    public bool IsOptionSelected(string name) {
        return options[selectedOption].name == name;
    }

    public void SelectOption(string name) {
        int index = 0;
        foreach (GameObject option in options) {
            if (option.name == name) {
                selectedOption = index;
                return;
            }
            index++;
        }
    }
}
