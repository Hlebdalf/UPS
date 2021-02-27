using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTransitionBetweenScenes : MonoBehaviour {
    void Start() {
        DontDestroyOnLoad(gameObject);
    }
}
