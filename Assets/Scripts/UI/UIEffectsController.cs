using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffectsController : MonoBehaviour {
    public ParticleSystem effect;
    public float minYRotation;
    public float maxYRotation;

    void Update() {
        float cameraYRotation = transform.eulerAngles.x;
        if (cameraYRotation >= minYRotation && cameraYRotation <= maxYRotation) {
            if (!effect.isPlaying) {
                effect.Play();
            }
        } else {
            if (effect.isPlaying) {
                effect.Stop();
            }
        }
    }
}
