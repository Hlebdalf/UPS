using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildsButtonManagment : MonoBehaviour {
    public Builds BuildsClass;

    private void Start() {
        BuildsClass = BuildsClass.GetComponent <Builds> ();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            BuildsClass.CreateObjects();
        }
    }
}
