using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Versus,
    Paused,
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameState state;
    public static event Action<GameState> OnGameStateChanged;

    float playerHealth = 0.75f;

    void Awake() {
        if (!instance) {
            instance = this;
        }
    }

    void Start() {
        UpdateGameState(GameState.Versus);
    }

    public float GetPlayerHealth() {
        return playerHealth;
    }

    public void UpdateGameState(GameState newState) {
        state = newState;

        switch (newState) {
            case GameState.Versus:
                break;
            case GameState.Paused:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }
}