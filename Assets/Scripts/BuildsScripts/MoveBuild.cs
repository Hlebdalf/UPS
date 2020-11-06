using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuild : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;

    private void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }

    void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) {
            BuildsClass.DeleteObject(gameObject);
        }
    }
}
