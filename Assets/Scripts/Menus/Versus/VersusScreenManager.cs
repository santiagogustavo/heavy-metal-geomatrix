using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VersusScreenManager : MonoBehaviour {
    void Start() {
        Invoke("StartGame", 6f);
    }

    void StartGame() {
        LevelMeta level = GameManager.instance.GetLevelMeta();
        SceneManager.LoadScene(level.levelName);
    }
}
