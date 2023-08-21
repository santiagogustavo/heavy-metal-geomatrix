using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Finder {
    public static GameObject GetGameObjectByName(Transform transform, string name) {
        GameObject found = GameObject.Find(name);
        return found.transform.IsChildOf(transform) ? found : null;
    }

    public static List<GameObject> GetGameObjectsByTagName(Transform transform, string tag) {
        List<GameObject> found = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag)).FindAll(g => g.transform.IsChildOf(transform));
        return found;
    }

    public static Transform GetTransformFromGameObjects(List<GameObject> gameObjects, string name) {
        return gameObjects.Find(g => g.name == name)?.transform;
    }
}
