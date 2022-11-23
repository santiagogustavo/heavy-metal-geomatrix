using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    void Update() {
        if (InputManager.instance.pause) {
            if (GameManager.state == GameState.Paused) {
                GameManager.instance.UpdateGameState(GameState.Versus);
            } else {
                GameManager.instance.UpdateGameState(GameState.Paused);
            }
        }
    }
}
