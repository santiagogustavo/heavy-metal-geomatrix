using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Finder {
    public static GameObject GetGameObjectByName(Transform transform, string name) {
        return transform.GetComponentsInChildren<Transform>().FirstOrDefault(c => c.gameObject.name == name)?.gameObject;
    }

    public static List<GameObject> GetGameObjectsByTagName(Transform transform, string tag) {
        List<GameObject> found = new List<GameObject>(GameObject.FindGameObjectsWithTag(tag)).FindAll(g => g.transform.IsChildOf(transform));
        return found;
    }

    public static Transform GetTransformFromGameObjects(List<GameObject> gameObjects, string name) {
        return gameObjects.Find(g => g.name == name)?.transform;
    }
}
