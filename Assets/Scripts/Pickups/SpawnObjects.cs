using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour {
    public GameObject[] gameObjects;
    public Vector3 size;
    public float minSpawnDelay = 3f;
    public float maxSpawnDelay = 5f;
    public bool prewarm;

    void Start() {
        if (prewarm) {
            SpawnRandomObject();
        } else {
            InvokeSpawnWithDelay();
        }
    }

    void InvokeSpawnWithDelay() {
        float randomDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        Invoke("SpawnRandomObject", randomDelay);
    }

    void SpawnRandomObject() {
        int randomIndex = Random.Range(0, gameObjects.Length);
        Vector3 randomSpawnPosition = transform.position + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );

        Instantiate(gameObjects[randomIndex], randomSpawnPosition, Quaternion.identity);

        InvokeSpawnWithDelay();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0f, 1f, 1f, 0.5f);
        Gizmos.DrawCube(transform.position, size);
    }
}
