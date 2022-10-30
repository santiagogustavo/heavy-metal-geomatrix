using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBone : MonoBehaviour {
    public Transform bone;

    void Update() {
        transform.position = bone.position;
        transform.rotation = bone.rotation;
    }
}
