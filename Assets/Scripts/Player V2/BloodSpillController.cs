using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpillController : MonoBehaviour {
    ParticleSystem blood;
    GameObject splatter;
    List<ParticleCollisionEvent> collisionEvents;

    void Start() {
        blood = GetComponent<ParticleSystem>();
        splatter = Resources.Load("Particles/Blood Splatter") as GameObject;
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        ParticlePhysicsExtensions.GetCollisionEvents(blood, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++) {
            EmitAtLocation(collisionEvents[i]);
        }
    }

    void EmitAtLocation(ParticleCollisionEvent collision) {
        if (!collision.colliderComponent || collision.colliderComponent.gameObject.layer != 3) {
            return;
        }
        Vector3 position = collision.intersection;
        Quaternion rotation = Quaternion.LookRotation(collision.normal);
        Instantiate(splatter, position, rotation);
    }
}
