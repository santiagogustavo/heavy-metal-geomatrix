using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour {
    public static PauseMenuManager instance;
    GameObject pauseMenu;
    GameObject[] options;

    float inputTime;

    int selectedOption = 0;

    void Awake() {
        if (!instance) {
            instance = this;
        }
        pauseMenu = GameObject.Find("Pause Menu");
        options = GameObject.FindGameObjectsWithTag("Select Card");
        IComparer myComparer = new GameObjectSorter();
        Array.Sort(options, myComparer);

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
        inputTime += Time.fixedDeltaTime;

        if (!GameManager.instance.IsGamePaused() || inputTime <= 0.75f) {
            return;
        }

        if (InputManager.instance.GetDown()) {
            SelectDown();
        } else if (InputManager.instance.GetUp()) {
            SelectUp();
        } else if (InputManager.instance.fire1) {
            SelectCurrent();
        }
    }

    void DebounceInput() {
        inputTime = 0f;
    }

    void SelectDown() {
        DebounceInput();
        if (selectedOption == options.Length - 1) {
            selectedOption = 0;
        } else {
            selectedOption++;
        }
    }

    void SelectUp() {
        DebounceInput();
        if (selectedOption == 0) {
            selectedOption = options.Length - 1;
        } else {
            selectedOption--;
        }
    }

    void SelectCurrent() {
        switch (selectedOption) {
            case 0:
                ResumeGame();
                break;
            case 1:
                CharacterChange();
                break;
            case 2:
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
}
