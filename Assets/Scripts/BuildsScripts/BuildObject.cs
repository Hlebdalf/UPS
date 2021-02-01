using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Interface InterfaceClass;
    private Builds BuildsClass;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType = -1, idxPreFub = -1;
    public int maxCntPeople = 0, cntPosters = 0, maxCntPosters = 0;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }

    private void OnMouseDown() {
        if (Input.GetMouseButtonDown(1)) {

        }
    }

    public void AddPoster() {
        // Добавление постера по сиду
    }
}
