using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    SelectPlayer,
    Versus,
    Paused,
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameState state;
    public static event Action<GameState> OnGameStateChanged;

    static PlayerMeta player1;

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
            case GameState.SelectPlayer:
                break;
            case GameState.Versus:
                break;
            case GameState.Paused:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    public void SetPlayer1Meta(PlayerMeta meta) {
        player1 = meta;
    }

    public PlayerMeta GetPlayer1Meta() {
        return player1;
    }
}