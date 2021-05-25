using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFPS : MonoBehaviour {
    void OnGUI() {
        GUILayout.Label("FPS: " + (int)(1.0f / Time.deltaTime));
    }
}