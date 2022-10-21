using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions {
    public static List<Transform> GetTransformByName(this Transform parent, string name) {
        List<Transform> gameObjects = new List<Transform>();

        foreach (Transform child in parent) {
            if (child.name == name) {
                gameObjects.Add(child);
            }
            if (child.childCount > 0) {
                gameObjects.AddRange(GetTransformByName(child, name));
            }
        }

        return gameObjects;
    }
}
