using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Interface InterfaceClass;
    private Builds BuildsClass;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType = -1, idxPreFub = -1, cntPosters = 0;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }

    private void OnMouseDown() {
        // Добавление рандомного постера
    }

    public void AddPoster(ulong seed) {
        // Добавление постера по сиду
    }
}
