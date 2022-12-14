using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    SelectPlayer,
    SelectLevel,
    Versus,
    Paused,
}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static GameState state;
    public static event Action<GameState> OnGameStateChanged;

    static LevelMeta level;
    static PlayerMeta player1;
    static bool isPaused;

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
            case GameState.SelectLevel:
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

    public void SetLevelMeta(LevelMeta meta) {
        level = meta;
    }

    public LevelMeta GetLevelMeta() {
        return level;
    }

    public void LockCursor() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetIsGamePaused(bool paused) {
        isPaused = paused;
    }

    public bool IsGamePaused() {
        return isPaused;
    }
}
