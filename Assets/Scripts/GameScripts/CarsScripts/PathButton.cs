using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathButton : MonoBehaviour {
    private GameObject MainCamera;
    private Cars CarsClass;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CarsClass = MainCamera.GetComponent <Cars> ();
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) CarsClass.SetLinePath(gameObject.transform.parent.gameObject);
    }
}
