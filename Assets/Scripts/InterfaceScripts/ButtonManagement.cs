using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour {
    public Builds BuildsClass;

    void Start() {
        BuildsClass = BuildsClass.GetComponent <Builds> ();
    }

    void Update() {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            BuildsClass.CreateObjects();
        }
    }
}
