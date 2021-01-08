using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarsAnimation : MonoBehaviour {
    public float marsSpeed = 1;

    private void Update() {
        transform.Rotate(0, marsSpeed, 0);
    }
}
