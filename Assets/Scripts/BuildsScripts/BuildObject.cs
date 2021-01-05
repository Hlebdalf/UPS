using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }
}
